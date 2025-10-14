using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/Enemys/EnemyConfig", fileName = "EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Basic Info")] [SerializeField]
        private int _id;

        [Header("Stats")] [SerializeField] private float _defaultMoveSpeed;
        [SerializeField] private int _defaultHp;

        public int Id => _id;
        public float DefaultMoveSpeed => _defaultMoveSpeed;
        public int DefaultHp => _defaultHp;
        
        public string GetPrefabPath()
        {
            return "Enemy" + _id;
        }
    }
}
