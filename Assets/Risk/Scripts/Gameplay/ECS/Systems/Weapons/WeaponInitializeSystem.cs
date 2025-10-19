using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Components.Weapons;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Systems.Weapons
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class WeaponInitializeSystem : ISystem 
    {
        private Filter _weaponInitializeFilter;
        
        private Stash<WeaponInitializeComponent> _weaponInitializeStash;
        private Stash<WeaponComponent> _weaponStash;
        private Stash<ProjectileDataComponent> _projectileStash;
        private Stash<MagazineComponent> _magazineStash;
        private Stash<TimeAttackComponent> _timeAttackStash;

        private readonly AllWeaponsConfig _allWeaponsConfig;
        
        public World World { get; set;}
        
        public WeaponInitializeSystem(AllWeaponsConfig allWeaponsConfig)
        {
            _allWeaponsConfig = allWeaponsConfig;
        }
    
        public void OnAwake()
        {
            _weaponInitializeFilter = World.Filter.With<WeaponInitializeComponent>().Build();

            _weaponInitializeStash = World.GetStash<WeaponInitializeComponent>();
            _weaponStash = World.GetStash<WeaponComponent>();
            _magazineStash = World.GetStash<MagazineComponent>();
            _projectileStash = World.GetStash<ProjectileDataComponent>();
            _timeAttackStash = World.GetStash<TimeAttackComponent>();
        }
    
        public void OnUpdate(float deltaTime)
        {
            if (_weaponInitializeFilter.IsEmpty()) return;
            foreach (var entity in _weaponInitializeFilter)
            {
                ref var weaponInit = ref _weaponInitializeStash.Get(entity);
                var weaponId = weaponInit.weaponId;
                var weaponConfig = _allWeaponsConfig.GetWeaponById(weaponId);
                SetupWeaponComponent(entity, weaponConfig);
                SetupProjectileComponent(entity, weaponConfig);
                SetupMagazineComponent(entity, weaponConfig);
                SetupTimeAttackComponent(entity, weaponConfig);
                
                _weaponInitializeStash.Remove(entity);
            }
        }

        private void SetupWeaponComponent(Entity entity, WeaponConfig config)
        {
            _weaponStash.Add(entity) = new WeaponComponent()
            {
                weaponId = config.Id,
                requiresTargeting = config.RequiresTargeting,
                weaponType = config.WeaponType,
                timeBetweenAttacks = config.BaseTimeBetweenAttacks,
                attackRange = 20f
            };
        }
        
        private void SetupProjectileComponent(Entity entity, WeaponConfig config)
        {
            _projectileStash.Add(entity) = new ProjectileDataComponent()
            {
                weaponId = config.Id,
                size = config.BaseSize,
                damage = config.BaseDamage,
                lifeTime = config.LifeTime,
                projectileSpeed = config.ProjectileSpeed,
                projectilePrefabAddress = config.GetPrefabPath()
            };
        }
        
        private void SetupTimeAttackComponent(Entity entity, WeaponConfig config)
        {
            _timeAttackStash.Add(entity) = new TimeAttackComponent()
            {
                timeBetweenAttacks = config.BaseTimeBetweenAttacks
            };
        }
        
        private void SetupMagazineComponent(Entity entity, WeaponConfig config)
        {
            _magazineStash.Add(entity) = new MagazineComponent()
            {
                maxAmmo = config.BaseAmmo,
                currentAmmo = config.BaseAmmo,
                reloadTime = config.BaseReloadTime,
                currentReloadTimer = config.BaseReloadTime,
                isReloading = false
            };
        }
    
        public void Dispose()
        {
    
        }
    }
}