using Risk.Gameplay.ECS.Components.Weapons;
using Risk.Gameplay.Services;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Systems.Weapons
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class WeaponAttackSystem : ISystem 
    {
        private Filter _weaponsFilter;
        
        private Stash<WeaponComponent> _weaponsStash;
        private Stash<TimeAttackComponent> _timeAttackStash;
        private Stash<ProjectileRequestComponent> _projectileRequestStash;
        private Stash<MagazineComponent> _magazineStash;
        
        public World World { get; set;}
    
        public void OnAwake()
        {
            _weaponsFilter = World.Filter.With<WeaponComponent>().Build();

            _weaponsStash = World.GetStash<WeaponComponent>();
            _timeAttackStash = World.GetStash<TimeAttackComponent>();
            _projectileRequestStash = World.GetStash<ProjectileRequestComponent>();
            _magazineStash = World.GetStash<MagazineComponent>();
        }
    
        public void OnUpdate(float deltaTime)
        {
            if (_weaponsFilter.IsEmpty()) return;
            
            foreach (var entity in _weaponsFilter)
            {
                ref var time = ref _timeAttackStash.Get(entity);
                ref var magazine = ref _magazineStash.Get(entity);
                if (magazine.isReloading) continue;
                
                if (time.timeBetweenAttacks > 0)
                {
                    time.timeBetweenAttacks -= deltaTime;
                    continue;
                }
                
                ref var weaponComp = ref _weaponsStash.Get(entity);
                if (!weaponComp.requiresTargeting)
                {
                    CreateProjectileRequest(entity);
                }

                magazine.currentAmmo--;
                time.timeBetweenAttacks = weaponComp.timeBetweenAttacks;
            }
        }

        private void CreateProjectileRequest(Entity entity)
        {
            var requestEntity = World.CreateEntity();
            _projectileRequestStash.Add(requestEntity) = new ProjectileRequestComponent()
            {
                weaponEntity = entity
            };
        }
    
        public void Dispose()
        {
    
        }
    }
}