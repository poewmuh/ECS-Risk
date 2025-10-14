using System.Collections.Generic;
using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/Weapons/AllWeaponsConfig", fileName = "AllWeaponsConfig")]
    public class AllWeaponsConfig : ScriptableObject
    {
        [SerializeField] private List<WeaponConfig> _weapons;

        public WeaponConfig GetWeaponById(int id)                                                                                                                                                                                
        {
            foreach (var weapon in _weapons)
            {
                if (weapon.Id == id) return weapon;
            }

            Debug.Log("[AllWeaponsConfig] GetWeaponById failed for id: " + id);
            return null;
        }

        public List<WeaponConfig> GetAllWeapons() => _weapons;
    }
}
