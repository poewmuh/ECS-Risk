using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Components.Spawn;
using Risk.Gameplay.ECS.Components.View;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Risk.Gameplay.ECS.Systems.Spawn
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class WaveSpawnerSystem : RareUpdateSystem 
    {
        private readonly DifficultyConfig _difficultyConfig;

        private Filter _gameTimeFilter;
        private Filter _waveFilter;
        private Stash<TimerComponent> _timerStash;
        private Stash<WaveComponent> _waveStash;
        private Stash<SpawnRequestComponent> _spawnRequestStash;

        protected override bool isHaveFirstTick => true;
        protected override float updateTime => 1f;

        public WaveSpawnerSystem(DifficultyConfig difficultyConfig)
        {
            _difficultyConfig = difficultyConfig;
        }
    
        public override void OnAwake()
        {
            _spawnRequestStash = World.GetStash<SpawnRequestComponent>();
            _waveStash = World.GetStash<WaveComponent>();
            _waveFilter = World.Filter.With<WaveComponent>().With<SpawnRequestComponent>().Build();
            _gameTimeFilter = World.Filter.With<TimerComponent>().Build();
            _timerStash = World.GetStash<TimerComponent>();
            
            base.OnAwake();
        }

        protected override void RareUpdate(float time)
        {
            var timerEntity = _gameTimeFilter.First();
            ref var timer = ref _timerStash.Get(timerEntity);
            
            var difficultyLevel = _difficultyConfig.GetDifficultyByTime(timer.GameTime);
            if (difficultyLevel == null)
                return;

            ref var waveComp = ref _waveStash.Get(_waveFilter.First());

            if (waveComp.difficultyLevel != difficultyLevel.Level)
            {
                waveComp.waveCount = 0;
                waveComp.lastSpawnTime = 0;
            }
                
            waveComp.difficultyLevel = difficultyLevel.Level;
            waveComp.lastSpawnTime += time;
            var currentWave = difficultyLevel.WaveData[waveComp.waveCount];

            if (waveComp.lastSpawnTime >= currentWave.SpawnInterval)
            {
                waveComp.lastSpawnTime = 0f;

                ProcessSpawn(currentWave);
                
                if (waveComp.waveCount + 1 < difficultyLevel.WaveData.Count)
                {
                    waveComp.waveCount++;
                }
            }
        }

        private void ProcessSpawn(DifficultyWave waveData)
        {
            ref var spawnRequest = ref _spawnRequestStash.Get(_waveFilter.First());
            foreach (var wave in waveData.Waves)
            {
                Debug.Log("[WaveSpawnerSystem] Spawn wave: " + wave.EnemyId + " Count: " + wave.Count);
                for (int i = 0; i < wave.Count; i++)
                {
                    spawnRequest.requests.Add(new SpawnRequest(wave.EnemyId));
                }
            }
        }

        public override void Dispose()
        {
    
        }
    }
}