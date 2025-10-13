using Risk.Extensions;
using UnityEngine;

namespace Risk.Gameplay.Services
{
    public static class SpawnLocationService
    {
        public static Vector3 GetRandomSpawnPositionAround(Vector3 center, float minRadius, float maxRadius)
        {
            var distance = Random.Range(minRadius, maxRadius);
            var angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            
            var offset = new Vector3(
                Mathf.Cos(angle) * distance,
                0,
                Mathf.Sin(angle) * distance);

            return (center + offset).OnlyXZ();
        }
    }
}
