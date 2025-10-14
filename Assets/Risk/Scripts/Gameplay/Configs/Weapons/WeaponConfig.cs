using Risk.Gameplay.Enums;
using TriInspector;
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
        [SerializeField] private float _baseDamage;
        [SerializeField] private float _baseAttackSpeed;                                                                                                                                                     
        [SerializeField] private float _baseRange;                                                                                                                              
        [SerializeField] private float _baseSize = 1f;
        
        public int Id => _id;
        public string Name => _name;
        public WeaponType WeaponType => _weaponType;
        public bool RequiresTargeting => _requiresTargeting;
        public float BaseDamage => _baseDamage;
        public float BaseAttackSpeed => _baseAttackSpeed;
        public float BaseRange => _baseRange;
        public float BaseSize => _baseSize;


        public string GetPrefabPath()
        {
            return _name + "_" + _id;
        }
    }
}
