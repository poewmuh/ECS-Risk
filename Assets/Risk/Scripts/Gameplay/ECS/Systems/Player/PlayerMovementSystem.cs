using Risk.Extensions;
using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Components.Player;
using Risk.Input;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Risk.Gameplay.ECS.Systems.Player
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerMovementSystem : IFixedSystem
    {
        private Filter _filter;

        private Stash<TransformComponent> _transformStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        
        private readonly MovementConfig _movementConfig;
        private readonly InputAction _moveInput;
        
        public World World { get; set;}

        public PlayerMovementSystem(MovementConfig movementConfig, PlayerInputActions playerInput)
        {
            _movementConfig = movementConfig;
            _moveInput = playerInput.Player.Move;
        }
    
        public void OnAwake()
        {
            _transformStash = World.GetStash<TransformComponent>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
            
            _filter = World.Filter
                .With<TransformComponent>()
                .With<RigidbodyComponent>()
                .With<PlayerMark>()
                .Build();
            
            _moveInput.Enable();
        }
    
        public void OnUpdate(float deltaTime)
        {
            var input = _moveInput.ReadValue<Vector2>();
            foreach (var entity in _filter)
            {
                ProcessMove(entity, input);
            }
        }

        private void ProcessMove(Entity entity, Vector2 input)
        {
            ref var rb = ref _rigidbodyStash.Get(entity);

            var dir = new Vector3(input.x, 0, input.y).normalized;
            var velocity = dir * _movementConfig.MoveSpeed;

            rb.Rigidbody.linearVelocity = velocity;
        }
    
        public void Dispose()
        {
            _moveInput.Disable();
        }
    }
}