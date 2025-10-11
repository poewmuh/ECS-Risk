using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/CameraConfig", fileName = "CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [SerializeField] private float _followSpeed;
        [SerializeField] private Vector3 _offset;

        public float FollowSpeed => _followSpeed;
        public Vector3 Offset => _offset;
    }
}
