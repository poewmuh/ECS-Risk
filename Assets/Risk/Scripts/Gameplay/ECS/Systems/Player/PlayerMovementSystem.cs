using Risk.Gameplay.ECS.Components.Units;
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

        private Stash<MovementComponent> _movementStash;
        private Stash<RigidbodyComponent> _rigidbodyStash;
        
        private readonly InputAction _moveInput;
        
        public World World { get; set;}

        public PlayerMovementSystem(PlayerInputActions playerInput)
        {
            _moveInput = playerInput.Player.Move;
        }
    
        public void OnAwake()
        {
            _movementStash = World.GetStash<MovementComponent>();
            _rigidbodyStash = World.GetStash<RigidbodyComponent>();
            
            _filter = World.Filter
                .With<RigidbodyComponent>()
                .With<PlayerMark>()
                .With<MovementComponent>()
                .Build();
            
            _moveInput.Enable();
        }
    
        public void OnUpdate(float deltaTime)
        {
            if (_filter.IsEmpty()) return;
            
            var input = _moveInput.ReadValue<Vector2>();
            foreach (var entity in _filter)
            {
                ProcessMove(entity, input);
            }
        }

        private void ProcessMove(Entity entity, Vector2 input)
        {
            ref var rb = ref _rigidbodyStash.Get(entity);
            ref var movement = ref _movementStash.Get(entity);

            var dir = new Vector3(input.x, 0, input.y).normalized;
            var velocity = dir * movement.currentMoveSpeed;

            rb.Rigidbody.linearVelocity = velocity;
        }
    
        public void Dispose()
        {
            _moveInput.Disable();
        }
    }
}