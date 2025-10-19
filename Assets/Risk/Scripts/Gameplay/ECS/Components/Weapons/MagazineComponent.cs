using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Components.Weapons
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MagazineComponent : IComponent
    {
        public int maxAmmo;
        public int currentAmmo;
        public float reloadTime;
        public float currentReloadTimer;
        public bool isReloading;
    }
}