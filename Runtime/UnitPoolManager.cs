using System;
using System.Collections.Generic;
using UnityEngine;

namespace CupkekGames.Units
{
    /// <summary>
    /// Centralized pool for unit 3D instances, keyed by model identifier.
    /// Replaces CombatUnitPoolManager — works for all unit types (heroes, enemies, NPCs).
    /// Full implementation will be wired when CharacterFeature and CombatView migration happens.
    /// </summary>
    public class UnitPoolManager
    {
        private readonly Dictionary<string, UnitPool> _pools = new();

        public void CreatePool(string modelKey, Func<GameObject> factory, int capacity = 1, int maxSize = 5)
        {
            if (_pools.ContainsKey(modelKey))
                return;

            _pools[modelKey] = new UnitPool(factory, capacity, maxSize);
        }

        public GameObject Spawn(string modelKey, Vector3 position, Quaternion rotation)
        {
            if (!_pools.TryGetValue(modelKey, out var pool))
            {
                Debug.LogError($"UnitPoolManager: No pool for model key '{modelKey}'");
                return null;
            }

            GameObject instance = pool.Get();
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.SetActive(true);
            return instance;
        }

        public void Return(string modelKey, GameObject instance)
        {
            if (_pools.TryGetValue(modelKey, out var pool))
                pool.Return(instance);
            else
                UnityEngine.Object.Destroy(instance);
        }

        public void DisposeAll()
        {
            foreach (var pool in _pools.Values)
                pool.Dispose();
            _pools.Clear();
        }

        private class UnitPool
        {
            private readonly Func<GameObject> _factory;
            private readonly Stack<GameObject> _available = new();
            private readonly int _maxSize;

            public UnitPool(Func<GameObject> factory, int capacity, int maxSize)
            {
                _factory = factory;
                _maxSize = maxSize;

                for (int i = 0; i < capacity; i++)
                {
                    var obj = _factory();
                    obj.SetActive(false);
                    _available.Push(obj);
                }
            }

            public GameObject Get()
            {
                return _available.Count > 0 ? _available.Pop() : _factory();
            }

            public void Return(GameObject instance)
            {
                instance.SetActive(false);
                if (_available.Count < _maxSize)
                    _available.Push(instance);
                else
                    UnityEngine.Object.Destroy(instance);
            }

            public void Dispose()
            {
                while (_available.Count > 0)
                {
                    var obj = _available.Pop();
                    if (obj != null)
                        UnityEngine.Object.Destroy(obj);
                }
            }
        }
    }
}
