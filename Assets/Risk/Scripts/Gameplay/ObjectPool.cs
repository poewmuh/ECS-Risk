using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Risk.Gameplay
{
    public class ObjectPool : IDisposable
    {
        private readonly Dictionary<int, Queue<GameObject>> _pool = new();
        
        public bool IsHaveInPool(int id) => _pool.ContainsKey(id);
        
        public void Push(int id, GameObject obj)
        {
            if (!_pool.ContainsKey(id))
            {
                _pool[id] = new Queue<GameObject>();
            }
            obj.SetActive(false);
            _pool[id].Enqueue(obj);
        }
        
        public bool TryPop(int id, out GameObject instance)
        {
            if (_pool[id].Count > 0)
            {
                instance = _pool[id].Dequeue();
                return true;
            }
            else
            {
                instance = null;
                return false;
            }
        }
        
        public void Dispose()
        {
            foreach (var pool in _pool.Values)
            {
                foreach (var obj in pool)
                {
                    Object.Destroy(obj);
                }
                pool.Clear();
            }
            _pool.Clear();
        }
    }
}
