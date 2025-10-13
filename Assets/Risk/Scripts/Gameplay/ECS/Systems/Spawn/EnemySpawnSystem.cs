using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Components.Crystal;
using Risk.Gameplay.ECS.Components.Spawn;
using Risk.Gameplay.ECS.Components.Units;
using Risk.Gameplay.Services;
using Risk.Tools;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Risk.Gameplay.ECS.Systems.Spawn
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EnemySpawnSystem : ISystem
    {
        private readonly AllEnemysConfig _allEnemysConfig;
        private readonly AddressablesLoader _addressablesLoader;
        
        private Stash<TransformComponent> _transformStash;
        private Stash<SpawnRequestComponent> _requestComponentStash;
        private Stash<RequestActiveMarker> _activeMarkerStash;
        private Stash<UnitInitializeComponent> _initializeStash;
        
        private Filter _activeRequestFilter;
        private Filter _crystalFilter;

        private EnemyPoolService _objectPool;
        
        public World World { get; set;}

        public EnemySpawnSystem(AllEnemysConfig allEnemysConfig)
        {
            _addressablesLoader = new AddressablesLoader();
            _allEnemysConfig = allEnemysConfig;
        }
    
        public void OnAwake()
        {
            _objectPool = new EnemyPoolService();
            _transformStash = World.GetStash<TransformComponent>();
            _activeMarkerStash = World.GetStash<RequestActiveMarker>();
            _requestComponentStash = World.GetStash<SpawnRequestComponent>();
            _initializeStash = World.GetStash<UnitInitializeComponent>();
            

            _activeRequestFilter = World.Filter.With<SpawnRequestComponent>().With<RequestActiveMarker>().Build();
            _crystalFilter = World.Filter.With<CrystalComponent>().With<TransformComponent>().Build();
        }
    
        public void OnUpdate(float deltaTime)
        {
            if (_activeRequestFilter.IsEmpty()) return;
            
            var crystalEntity = _crystalFilter.First();
            ref var tr = ref _transformStash.Get(crystalEntity);
            var crystalPos = tr.Transform.position;

            foreach (var requestEntity in _activeRequestFilter)
            {
                ref var request = ref _requestComponentStash.Get(requestEntity);
                
                ProcessSpawn(request.enemyId, crystalPos);

                _activeMarkerStash.Remove(requestEntity);
            }
        }
        
        private void ProcessSpawn(int enemyId, Vector3 crystalPos)
        {
            if (!_objectPool.TryGetFromPool(enemyId, out var pooledEnemy))
            {
                pooledEnemy = CreateNewEnemy(enemyId);
            }

            ActivateEnemy(pooledEnemy, crystalPos);
        }

        private GameObject CreateNewEnemy(int enemyId)
        {
            var enemyConfig = _allEnemysConfig.GetCharacterById(enemyId);

            var enemyObject = _addressablesLoader.LoadAndInstantiate(
                enemyConfig.GetPrefabPath(),
                EnemyPoolService.POOL_POSITION,
                Quaternion.identity
            );

            var entity = enemyObject.GetComponent<EnemyMarkProvider>().Entity;
            _initializeStash.Add(entity) = new UnitInitializeComponent() { unitId = enemyId };
            
            enemyObject.SetActive(false);

            return enemyObject;
        }

        private void ActivateEnemy(GameObject enemyObject, Vector3 crystalPos)
        {
            var spawnPosition = SpawnLocationService.GetRandomSpawnPositionAround(
                crystalPos,
                7,
                12
            );

            enemyObject.transform.position = spawnPosition;
            enemyObject.SetActive(true);
        }
    
        public void Dispose()
        {
            _objectPool.Dispose();
            _addressablesLoader.Dispose();
        }
    }
}