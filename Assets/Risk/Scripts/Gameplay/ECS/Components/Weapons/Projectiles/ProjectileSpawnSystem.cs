using Risk.Gameplay.ECS.Components.Units;
using Risk.Gameplay.ECS.Components.Weapons;
using Risk.Gameplay.Services;
using Risk.Tools;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Risk.Gameplay.ECS.Systems.Weapons
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProjectileSpawnSystem : SimpleSystem<ProjectileRequestComponent>
    {
        private Filter _playerFilter;
        
        private Stash<ProjectileDataComponent> _projectileStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<ProjectileLifetimeComponent> _lifeTimeStash;

        private readonly AddressablesLoader _loader;
        private readonly ObjectPoolService _objectPool;
        
        public ProjectileSpawnSystem(ObjectPoolService projectilePool)
        {
            _objectPool = projectilePool;
            _loader = new AddressablesLoader();
        }
        
        protected override void InitStashes()
        {
            base.InitStashes();
            _projectileStash = World.GetStash<ProjectileDataComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _lifeTimeStash = World.GetStash<ProjectileLifetimeComponent>();
        }

        protected override Filter BuildFilter()
        {
            _playerFilter = World.Filter.With<PlayerMark>().With<TransformComponent>().Build();
            return base.BuildFilter();
        }

        protected override void Process(Entity entity, ref ProjectileRequestComponent component, in float deltaTime)
        {
            ref var projectile = ref _projectileStash.Get(component.weaponEntity);

            CreateProjectile(ref projectile);
            
            World.RemoveEntity(entity);
        }

        private void CreateProjectile(ref ProjectileDataComponent dataComponent)
        {
            GameObject projectileObject;
            if (!_objectPool.TryGetFromPool(dataComponent.weaponId, out projectileObject))
            {
                projectileObject = InstantiateProjectile(ref dataComponent);
            }

            ref var tr = ref _transformStash.Get(_playerFilter.First());
            projectileObject.transform.position = tr.Transform.position;

            projectileObject.SetActive(true);
            var projectileEntity = projectileObject.GetComponent<ProjectileProvider>().Entity;
            _lifeTimeStash.Add(projectileEntity) = new ProjectileLifetimeComponent()
            {
                lifetime = dataComponent.lifeTime
            };
        }

        private GameObject InstantiateProjectile(ref ProjectileDataComponent dataComponent)
        {
            return _loader.LoadAndInstantiate(dataComponent.projectilePrefabAddress, ObjectPoolService.POOL_POSITION,
                Quaternion.identity);
        }
    }
}