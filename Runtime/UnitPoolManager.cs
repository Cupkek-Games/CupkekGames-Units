using System;
using System.Collections.Generic;
using UnityEngine;

namespace CupkekGames.Units
{
    /// <summary>
    /// Centralized pool for unit 3D instances, keyed by model identifier.
    /// Works for all unit types (heroes, enemies, NPCs). Per-pool hooks carry
    /// consumer-specific wiring: onReturn (e.g. view unregister) and onDispose
    /// (e.g. addressable release).
    /// </summary>
    public class UnitPoolManager
    {
        private readonly Dictionary<string, UnitPool> _pools = new();

        public bool HasPool(string modelKey) => _pools.ContainsKey(modelKey);

        public void CreatePool(string modelKey, Func<GameObject> factory,
            Action<GameObject> onReturn = null, Action onDispose = null,
            int capacity = 1, int maxSize = 5)
        {
            if (_pools.ContainsKey(modelKey))
                return;

            _pools[modelKey] = new UnitPool(factory, onReturn, onDispose, capacity, maxSize);
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
            if (instance == null) return;

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
            private readonly Action<GameObject> _onReturn;
            private readonly Action _onDispose;
            private readonly Stack<GameObject> _available = new();
            private readonly int _maxSize;

            public UnitPool(Func<GameObject> factory, Action<GameObject> onReturn,
                Action onDispose, int capacity, int maxSize)
            {
                _factory = factory;
                _onReturn = onReturn;
                _onDispose = onDispose;
                _maxSize = maxSize;

                for (int i = 0; i < capacity; i++)
                {
                    var obj = _factory();
                    obj.SetActive(false);
                    _available.Push(obj);
                }
            }

            // Skips instances destroyed while pooled (scene unloads etc.).
            public GameObject Get()
            {
                while (_available.Count > 0)
                {
                    var obj = _available.Pop();
                    if (obj != null) return obj;
                }

                return _factory();
            }

            public void Return(GameObject instance)
            {
                _onReturn?.Invoke(instance);
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

                _onDispose?.Invoke();
            }
        }
    }
}
