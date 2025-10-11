using System.Collections.Generic;
using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/AllCharacterConfig", fileName = "AllCharacterConfig")]
    public class AllCharactersConfig : ScriptableObject
    {
        [SerializeField] private List<CharacterConfig> _characterConfigs;

        public CharacterConfig GetCharacterById(int id)
        {
            foreach (var character in _characterConfigs)
            {
                if (character.Id == id) return character;
            }

            Debug.LogError($"[AllCharacterConfig] CharacterConfig with id {id} not found");
            return null;
        }
    }
}
