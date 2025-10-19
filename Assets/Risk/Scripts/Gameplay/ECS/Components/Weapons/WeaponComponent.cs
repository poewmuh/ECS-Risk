using Risk.Gameplay.Enums;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Components.Weapons
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct WeaponComponent : IComponent
    {
        public int weaponId;
        public WeaponType weaponType;
        public bool requiresTargeting;

        public float timeBetweenAttacks;
        public float attackRange;
    }
}