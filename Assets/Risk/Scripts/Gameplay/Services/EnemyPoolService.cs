using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Risk.Gameplay.Services
{
    public class EnemyPoolService : IDisposable
    {
        public static readonly Vector3 POOL_POSITION = Vector3.up * 1000f;
        
        private readonly Dictionary<int, Queue<GameObject>> _pool = new();
        
        public bool HasInPool(int id) => _pool.ContainsKey(id);
        
        public void ReturnToPool(int enemyId, GameObject go, Entity entity)
        {
            if (!_pool.ContainsKey(enemyId))
            {
                _pool[enemyId] = new Queue<GameObject>();
            }
            go.SetActive(false);
            go.transform.position = POOL_POSITION;

            _pool[enemyId].Enqueue(go);
        }
        
        public bool TryGetFromPool(int id, out GameObject go)
        {
            if (HasInPool(id) && _pool[id].Count > 0)
            {
                go = _pool[id].Dequeue();
                return true;
            }

            go = null;
            return false;
        }
        
        public void Dispose()
        {
            foreach (var pool in _pool.Values)
            {
                foreach (var pooledGo in pool)
                {
                    Object.Destroy(pooledGo);
                }
                pool.Clear();
            }
            _pool.Clear();
        }
    }
}
