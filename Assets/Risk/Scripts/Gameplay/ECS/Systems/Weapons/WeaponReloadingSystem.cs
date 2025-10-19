using Risk.Gameplay.ECS.Components.Weapons;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Systems.Weapons
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class WeaponReloadingSystem : SimpleSystem<MagazineComponent> 
    {
        protected override void Process(Entity entity, ref MagazineComponent component, in float deltaTime)
        {
            if (component is { currentAmmo: <= 0, isReloading: false })
            {
                component.currentReloadTimer = 0;
                component.isReloading = true;
            }

            if (component.isReloading)
            {
                component.currentReloadTimer += deltaTime;
                if (component.currentReloadTimer >= component.reloadTime)
                {
                    component.currentAmmo = component.maxAmmo;
                    component.isReloading = false;
                }
            }
        }
    }
}