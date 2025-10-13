using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Components.Spawn;
using Risk.Gameplay.ECS.Components.Units;
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
        
        private Stash<SpawnRequestComponent> _spawnRequests;
        private Stash<MovementComponent> _movementStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        
        private Entity _spawnRequestEntity;

        private ObjectPool _objectPool;
        
        public World World { get; set;}

        public EnemySpawnSystem(AllEnemysConfig allEnemysConfig)
        {
            _addressablesLoader = new AddressablesLoader();
            _allEnemysConfig = allEnemysConfig;
        }
    
        public void OnAwake()
        {
            _objectPool = new ObjectPool();
            _movementStash = World.GetStash<MovementComponent>();
            _healthStash = World.GetStash<HealthComponent>();
            _spawnRequests = World.GetStash<SpawnRequestComponent>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
            
            _spawnRequestEntity = World.Filter.With<SpawnRequestComponent>().Build().First();
        }
    
        public void OnUpdate(float deltaTime)
        {
            ref var spawnRequest = ref _spawnRequests.Get(_spawnRequestEntity);

            foreach (var enemyData in spawnRequest.requests)
            {
                ProcessSpawn(enemyData.enemyId);
            }
            
            spawnRequest.requests.Clear();
        }
        
        private void ProcessSpawn(int enemyId)
        {
            //TODO: WORK IN PROGRESS TEST CODE
            var enemyConfig = _allEnemysConfig.GetCharacterById(enemyId);
            GameObject enemy = null;
            if (!_objectPool.IsHaveInPool(enemyId) || !_objectPool.TryPop(enemyId, out enemy))
            {
                enemy = LoadEnemy(enemyConfig);
            }

            var enemyEntity = enemy.GetComponent<EnemyMarkProvider>().Entity;
            ref var rb = ref _rigidbodyStash.Get(enemyEntity);
            rb.Rigidbody.position = GetRandomSpawnPosition();
            enemy.SetActive(true);
        }
        
        private Vector3 GetRandomSpawnPosition()
        {
            var distance = Random.Range(3, 6);
            var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            var offset = new Vector3(
                Mathf.Cos(angle) * distance,
                0,
                Mathf.Sin(angle) * distance
            );

            return offset;
        }

        private GameObject LoadEnemy(EnemyConfig enemyConfig)
        {
            var prefab = _addressablesLoader.LoadImmediate<GameObject>(enemyConfig.GetPrefabPath());
            var enemyObject = Object.Instantiate(prefab, Vector3.up * 1000, Quaternion.identity);
            var entity = enemyObject.GetComponent<EnemyMarkProvider>().Entity;
            _movementStash.Add(entity) = new MovementComponent()
            {
                currentMoveSpeed = enemyConfig.DefaultMoveSpeed,
                defaultMoveSpeed = enemyConfig.DefaultMoveSpeed
            };

            _healthStash.Add(entity) = new HealthComponent()
            {
                currentHealth = enemyConfig.DefaultHp,
                maxHealth = enemyConfig.DefaultHp
            };

            return enemyObject;
        }
    
        public void Dispose()
        {
            _objectPool.Dispose();
            _addressablesLoader.Dispose();
        }
    }
}