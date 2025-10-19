using Risk.Gameplay.ECS.Components.Weapons;
using Risk.Gameplay.Services;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Systems.Weapons
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProjectileLifetimeSystem : ISystem
    {
        private readonly ObjectPoolService _projectilePool;
        private Stash<ProjectileComponent> _projectileStash;
        private Stash<ProjectileLifetimeComponent> _lifetimeStash;
        
        private Filter _lifetimeFilter;
        
        public World World { get; set; }

        public ProjectileLifetimeSystem(ObjectPoolService projectilePool)
        {
            _projectilePool = projectilePool;
        }

        public void OnAwake()
        {
            _lifetimeFilter = World.Filter.With<ProjectileLifetimeComponent>().Build();
            InitStashes();
        }

        private void InitStashes()
        {
            _lifetimeStash = World.GetStash<ProjectileLifetimeComponent>();
            _projectileStash = World.GetStash<ProjectileComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_lifetimeFilter.IsEmpty()) return;
            
            foreach (var entity in _lifetimeFilter)
            {
                ref var projectile = ref _projectileStash.Get(entity);
                ref var lifeTimeComp = ref _lifetimeStash.Get(entity);
                
                lifeTimeComp.lifetime -= deltaTime;
                if (lifeTimeComp.lifetime <= 0)
                {
                    _projectilePool.ReturnToPool(projectile.weaponId, projectile.gameObject);
                    _lifetimeStash.Remove(entity);
                }
            }
        }
        
        public void Dispose() { }
    }
}