using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/MovementConfig", fileName = "MovementConfig")]
    public class MovementConfig : ScriptableObject
    {
        [SerializeField] private float _moveSpeed;

        public float MoveSpeed => _moveSpeed;
    }
}
