using System.Collections.Generic;
using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/Heroes/AllCharacterConfig", fileName = "AllCharacterConfig")]
    public class AllHeroesConfig : ScriptableObject
    {
        [SerializeField] private List<HeroConfig> _heroesConfigs;

        public HeroConfig GetCharacterById(int id)
        {
            foreach (var character in _heroesConfigs)
            {
                if (character.Id == id) return character;
            }

            Debug.LogError($"[AllCharacterConfig] CharacterConfig with id {id} not found");
            return null;
        }
    }
}
