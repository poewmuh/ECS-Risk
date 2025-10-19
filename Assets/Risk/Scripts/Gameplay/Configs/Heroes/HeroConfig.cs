using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/Heroes/CharacterConfig", fileName = "CharacterConfig")]
    public class HeroConfig : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [Header("Stats")]
        [SerializeField] private float _defaultMoveSpeed;
        [SerializeField] private int _defaultHP;
        [Header("Starting")]
        [SerializeField] private int _startingWeaponId;

        public int Id => _id;
        
        public float DefaultMS => _defaultMoveSpeed;
        public int DefaultHP => _defaultHP;
        public string SpritePath => "CharacterSprite_" + _id;
        
        public int StartingWeaponId => _startingWeaponId;

        public string GetMeshPath(int skinId)
        {
            return "CharacterMesh_" + _id + "_skin_" + skinId;
        }
    }
}
