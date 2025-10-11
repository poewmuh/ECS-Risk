using Cysharp.Threading.Tasks;
using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Components.Player;
using Risk.Tools;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Risk.Gameplay.ECS.Initilizers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerInitilizer : IInitializer
    {
        private Stash<HealthComponent> _healthStash;
        private Stash<MovementComponent> _movementStash;
        
        private readonly AddressablesLoader _addressablesLoader;
        private readonly CharacterConfig _characterConfig;

        private int _skinId;
        
        public World World { get; set;}

        public PlayerInitilizer(int heroId, int skinId, AllCharactersConfig allCharactersConfig)
        {
            _addressablesLoader = new AddressablesLoader();
            _skinId = skinId;
            _characterConfig = allCharactersConfig.GetCharacterById(heroId);
        }
    
        public void OnAwake()
        {
            _healthStash = World.GetStash<HealthComponent>();
            _movementStash = World.GetStash<MovementComponent>();
            
            InitilizeCharacter().Forget();
        }

        private async UniTaskVoid InitilizeCharacter()
        {
            var avatarTransform = await SpawnPlayerAvatar();
            await SpawnPlayerMesh(avatarTransform);

            var entity = avatarTransform.GetComponent<PlayerMarkProvider>().Entity;
            AddStatsForPlayer(entity);
        }

        private void AddStatsForPlayer(Entity entity)
        {
            ref var healthComponent = ref _healthStash.Add(entity);
            healthComponent.maxHealth = _characterConfig.DefaultHP;
            healthComponent.currentHealth = _characterConfig.DefaultHP;
            
            ref var movementComponent = ref _movementStash.Add(entity);
            movementComponent.defaultMoveSpeed = _characterConfig.DefaultMS;
            movementComponent.currentMoveSpeed = _characterConfig.DefaultMS;
        }
        
        private async UniTask<Transform> SpawnPlayerAvatar()
        {
            var avatarPrefab = await _addressablesLoader.LoadAsync<GameObject>("PlayerAvatar");
            return Object.Instantiate(avatarPrefab).transform;
        }

        private async UniTask SpawnPlayerMesh(Transform parent)
        {
            var meshPath = _characterConfig.GetMeshPath(_skinId);
            var playerMeshPrefab = await _addressablesLoader.LoadAsync<GameObject>(meshPath);
            Object.Instantiate(playerMeshPrefab, parent);
        }
    
        public void Dispose()
        {
            _addressablesLoader.Dispose();
        }
    }
}