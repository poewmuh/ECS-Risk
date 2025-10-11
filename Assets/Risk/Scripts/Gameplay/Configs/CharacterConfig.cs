using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/CharacterConfig", fileName = "CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [Header("Stats")]
        [SerializeField] private float _defaultMoveSpeed;
        [SerializeField] private int _defaultHP;

        public int Id => _id;
        
        public float DefaultMS => _defaultMoveSpeed;
        public int DefaultHP => _defaultHP;
        
        public string MeshPath => "CharacterMesh_" + _id;
        public string SpritePath => "CharacterSprite_" + _id;
    }
}
