using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Systems;
using Risk.Gameplay.ECS.Systems.Player;
using Risk.Input;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace Risk.Gameplay.Boot
{
    public class GameInitialization : MonoBehaviour
    {
        private MovementConfig _movementConfig;
        private CameraConfig _cameraConfig;
        private PlayerInputActions _playerInput;
        
        private World _world;

        [Inject]
        public void Construct(MovementConfig movementConfig, CameraConfig cameraConfig, PlayerInputActions playerInput)
        {
            _playerInput = playerInput;
            _movementConfig = movementConfig;
            _cameraConfig = cameraConfig;
        }
        
        private void Start() 
        {
            _world = World.Default;
        
            var systemsGroup = _world.CreateSystemsGroup();

            systemsGroup.AddSystem(new CameraFollowSystem(_cameraConfig));
            systemsGroup.AddSystem(new PlayerMovementSystem(_movementConfig, _playerInput));
        
            _world.AddSystemsGroup(order: 0, systemsGroup);
        }
    }
}
