using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Risk.Gameplay.ECS.Components.Weapons
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ProjectileDataComponent : IComponent
    {
        public int weaponId;
        public int damage;
        public float size;
        public float lifeTime;
        public float projectileSpeed;
        public string projectilePrefabAddress;
    }
}