using Risk.Gameplay.Enums;
using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/Weapons/WeaponConfig", fileName = "WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        
        [Header("Weapon Type")]
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private bool _requiresTargeting;
        
        [Header("Stats")]
        [SerializeField] private int _baseDamage;
        [SerializeField] private float _baseTimeBetweenAttacks;                                                                                                                                                     
        [SerializeField] private float _baseRange;                                                                                                                              
        [SerializeField] private float _baseSize = 1f;
        [SerializeField] private float _lifeTime;
        [SerializeField] private float _projectileSpeed;

        [Header("Magazine")] 
        [SerializeField] private int _baseAmmo;
        [SerializeField] private float _baseReloadTime;
        
        public int Id => _id;
        public string Name => _name;
        public WeaponType WeaponType => _weaponType;
        public bool RequiresTargeting => _requiresTargeting;
        public int BaseDamage => _baseDamage;
        public float BaseTimeBetweenAttacks => _baseTimeBetweenAttacks;
        public float BaseSize => _baseSize;
        public float LifeTime => _lifeTime;
        public float ProjectileSpeed => _projectileSpeed;
        
        public int BaseAmmo => _baseAmmo;
        public float BaseReloadTime => _baseReloadTime;


        public string GetPrefabPath()
        {
            return _name + "_" + _id;
        }
    }
}
