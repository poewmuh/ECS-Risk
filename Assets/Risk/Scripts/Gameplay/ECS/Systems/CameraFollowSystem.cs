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
        private Entity _cameraEntity;
        private Entity _playerEntity;

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
            _cameraEntity = World.Filter.With<CameraComponent>().Build().First();
            _playerEntity = World.Filter.With<PlayerMark>().With<TransformComponent>().Build().First();

            _transformStash = World.GetStash<TransformComponent>();
            _cameraStash = World.GetStash<CameraComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            ref var camera = ref _cameraStash.Get(_cameraEntity);
            ref var tr = ref _transformStash.Get(_playerEntity);
            
            var cameraTransform = camera.CameraTransform;
            var targetPosition = tr.Transform.position + _cameraConfig.Offset;

            var velocity = Vector3.zero;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition,
                _cameraConfig.FollowSpeed * deltaTime);
        }
        
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}