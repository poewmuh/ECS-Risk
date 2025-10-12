using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Initilizers;
using Risk.Gameplay.ECS.Systems;
using Risk.Gameplay.ECS.Systems.Crystal;
using Risk.Gameplay.ECS.Systems.Player;
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
        
        private World _world;

        [Inject]
        public void Construct(AllHeroesConfig allHeroesConfig, AllEnemysConfig allEnemysConfig, CameraConfig cameraConfig, PlayerInputActions playerInput)
        {
            _playerInput = playerInput;
            _allHeroesConfig = allHeroesConfig;
            _cameraConfig = cameraConfig;
            _allEnemysConfig = allEnemysConfig;
        }
        
        private void Start() 
        {
            _world = World.Default;
        
            var systemsGroup = _world.CreateSystemsGroup();
            
            systemsGroup.AddInitializer(new PlayerInitilizer(0, 0, _allHeroesConfig));

            systemsGroup.AddSystem(new CrystalMoveSystem());
            systemsGroup.AddSystem(new CameraFollowSystem(_cameraConfig));
            systemsGroup.AddSystem(new PlayerMovementSystem(_playerInput));
        
            _world.AddSystemsGroup(order: 0, systemsGroup);
        }
    }
}
