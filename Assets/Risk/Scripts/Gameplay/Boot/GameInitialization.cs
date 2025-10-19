using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Initilizers;
using Risk.Gameplay.ECS.Systems;
using Risk.Gameplay.ECS.Systems.Crystal;
using Risk.Gameplay.ECS.Systems.Player;
using Risk.Gameplay.ECS.Systems.Spawn;
using Risk.Gameplay.ECS.Systems.Weapons;
using Risk.Gameplay.Services;
using Risk.Input;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace Risk.Gameplay.Boot
{
    public class GameInitialization : MonoBehaviour
    {
        private AllEnemysConfig _allEnemysConfig;
        private AllHeroesConfig _allHeroesConfig;
        private CameraConfig _cameraConfig;
        private PlayerInputActions _playerInput;
        private DifficultyConfig _difficultyConfig;
        private AllWeaponsConfig _allWeaponsConfig;
        
        private World _world;

        private ObjectPoolService _projectilePool;

        [Inject]
        public void Construct(AllHeroesConfig allHeroesConfig, AllEnemysConfig allEnemysConfig, 
            CameraConfig cameraConfig, PlayerInputActions playerInput, DifficultyConfig difficultyConfig,
            AllWeaponsConfig allWeaponsConfig)
        {
            _playerInput = playerInput;
            _allHeroesConfig = allHeroesConfig;
            _cameraConfig = cameraConfig;
            _allEnemysConfig = allEnemysConfig;
            _difficultyConfig = difficultyConfig;
            _allWeaponsConfig = allWeaponsConfig;

            _projectilePool = new ObjectPoolService();
        }
        
        private void Start() 
        {
            _world = World.Default;
        
            var systemsGroup = _world.CreateSystemsGroup();
            
            systemsGroup.AddInitializer(new MainComponentsInitilizer());
            systemsGroup.AddInitializer(new PlayerInitilizer(0, 0, _allHeroesConfig));

            systemsGroup.AddSystem(new CrystalMoveSystem());
            systemsGroup.AddSystem(new CameraFollowSystem(_cameraConfig));
            systemsGroup.AddSystem(new PlayerMovementSystem(_playerInput));
            systemsGroup.AddSystem(new GameTimerSystem());
            systemsGroup.AddSystem(new WaveSpawnerSystem(_difficultyConfig));
            systemsGroup.AddSystem(new EnemySpawnSystem(_allEnemysConfig));
            systemsGroup.AddSystem(new WeaponInitializeSystem(_allWeaponsConfig));
            systemsGroup.AddSystem(new WeaponReloadingSystem());
            systemsGroup.AddSystem(new WeaponAttackSystem());
            systemsGroup.AddSystem(new ProjectileSpawnSystem(_projectilePool));
            systemsGroup.AddSystem(new ProjectileLifetimeSystem(_projectilePool));
        
            _world.AddSystemsGroup(order: 0, systemsGroup);
        }
    }
}
