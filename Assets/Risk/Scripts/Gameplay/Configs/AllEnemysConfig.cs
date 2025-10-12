using System.Collections.Generic;
using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/AllEnemysConfig", fileName = "AllEnemysConfig")]
    public class AllEnemysConfig : ScriptableObject
    {
        [SerializeField] private List<EnemyConfig> _enemysConfigs;

        public EnemyConfig GetCharacterById(int id)
        {
            foreach (var enemy in _enemysConfigs)
            {
                if (enemy.Id == id) return enemy;
            }

            Debug.LogError($"[AllCharacterConfig] CharacterConfig with id {id} not found");
            return null;
        }
    }
}
