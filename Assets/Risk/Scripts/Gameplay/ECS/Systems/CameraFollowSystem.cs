using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Components;
using Risk.Gameplay.ECS.Components.Player;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Risk.Gameplay.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CameraFollowSystem : ILateSystem
    {
        private Filter _cameraFilter;
        private Filter _playerFilter;

        private Stash<TransformComponent> _transformStash;
        private Stash<CameraComponent> _cameraStash;
        
        private readonly CameraConfig _cameraConfig;
        
        public World World { get; set; }

        public CameraFollowSystem(CameraConfig cameraConfig)
        {
            _cameraConfig = cameraConfig;
        }
        
        public void OnAwake()
        {
            _cameraFilter = World.Filter.With<CameraComponent>().Build();
            _playerFilter = World.Filter.With<PlayerMark>().With<TransformComponent>().Build();

            _transformStash = World.GetStash<TransformComponent>();
            _cameraStash = World.GetStash<CameraComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_cameraFilter.IsEmpty() || _playerFilter.IsEmpty()) return;
            
            var cameraEntity = _cameraFilter.First();
            var playerEntity = _playerFilter.First();
            
            ref var camera = ref _cameraStash.Get(cameraEntity);
            ref var tr = ref _transformStash.Get(playerEntity);
            
            var cameraTransform = camera.CameraTransform;
            var targetPosition = tr.Transform.position + _cameraConfig.Offset;

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition,
                _cameraConfig.FollowSpeed * deltaTime);
        }
        
        public void Dispose() { }
    }
}