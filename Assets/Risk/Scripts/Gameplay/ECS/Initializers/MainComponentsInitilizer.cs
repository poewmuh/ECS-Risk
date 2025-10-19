using Risk.Gameplay.ECS.Components.Spawn;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Initilizers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MainComponentsInitilizer : IInitializer 
    {
        private Stash<WaveComponent> _waveStash;
        private Stash<SpawnRequestComponent> _spawnRequestStash;
        public World World { get; set;}
    
        public void OnAwake()
        {
            _waveStash = World.GetStash<WaveComponent>();
            _spawnRequestStash = World.GetStash<SpawnRequestComponent>();
            
            var entityForSpawn = World.CreateEntity();
            _waveStash.Add(entityForSpawn);

            var requestPoolSize = 15;
            for (int i = 0; i < requestPoolSize; i++)
            {
                var commandEntity = World.CreateEntity();
                _spawnRequestStash.Add(commandEntity);
            }
        }
    
        public void Dispose()
        {
    
        }
    }
}