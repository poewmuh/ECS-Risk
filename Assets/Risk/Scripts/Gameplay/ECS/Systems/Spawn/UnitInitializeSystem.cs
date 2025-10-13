using Risk.Gameplay.Configs;
using Risk.Gameplay.ECS.Components.Spawn;
using Risk.Gameplay.ECS.Components.Units;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Systems.Spawn
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UnitInitializeSystem : ISystem 
    {
        private readonly AllEnemysConfig _allEnemysConfig;
        
        private Stash<MovementComponent> _movementStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<UnitInitializeComponent> _initializeStash;

        private Filter _initializeFilter;
        
        public World World { get; set;}

        public UnitInitializeSystem(AllEnemysConfig allEnemysConfig)
        {
            _allEnemysConfig = allEnemysConfig;
        }
    
        public void OnAwake()
        {
            _movementStash = World.GetStash<MovementComponent>();
            _healthStash = World.GetStash<HealthComponent>();

            _initializeFilter = World.Filter.With<UnitInitializeComponent>().Build();
        }
    
        public void OnUpdate(float deltaTime)
        {
            if (_initializeFilter.IsEmpty()) return;

            foreach (var entity in _initializeFilter)
            {
                InitializeUnit(entity);
            }
        }
        
        private void InitializeUnit(Entity entity)
        {
            ref var initComponent = ref _initializeStash.Get(entity);
            var config = _allEnemysConfig.GetCharacterById(initComponent.unitId);

            ref var health = ref _healthStash.Add(entity);
            health.maxHealth = config.DefaultHp;
            health.currentHealth = config.DefaultHp;
            
            ref var movement = ref _movementStash.Add(entity);
            movement.defaultMoveSpeed = config.DefaultMoveSpeed;
            movement.currentMoveSpeed = config.DefaultMoveSpeed;

            _initializeStash.Remove(entity);
        }
    
        public void Dispose()
        {
    
        }
    }
}