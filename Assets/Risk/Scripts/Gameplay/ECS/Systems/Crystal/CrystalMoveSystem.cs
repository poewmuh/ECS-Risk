using Risk.Gameplay.ECS.Components.Crystal;
using Risk.Gameplay.ECS.Components.Player;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Systems.Crystal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CrystalMoveSystem : SimpleFixedSystem<CrystalComponent, RigidbodyComponent> 
    {
        protected override void Process(Entity entity, ref CrystalComponent mainCrystal, ref RigidbodyComponent rb, in float deltaTime)
        {
            rb.Rigidbody.linearVelocity = rb.Rigidbody.transform.forward * mainCrystal.Speed;
        }
    }
}