using Risk.Gameplay.ECS.Components.View;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GameTimerSystem : SimpleSystem<TimerComponent> 
    {
        private float _timer = 0f;
        
        protected override void Process(Entity entity, ref TimerComponent component, in float deltaTime)
        {
            _timer += deltaTime;
            component.UpdateTimer(_timer);
        }
    }
}