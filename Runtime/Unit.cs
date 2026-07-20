using System;
using System.Collections.Generic;
using UnityEngine;

namespace CupkekGames.Units
{
    /// <summary>
    /// Universal entity with composable features.
    /// What a unit IS depends on what features it has, not what class it is.
    /// Heroes, enemies, NPCs, pets — all are Unit with different features.
    /// </summary>
    [Serializable]
    public class Unit
    {
        [SerializeField] private string _id;
        [SerializeField] private string _key;

        [SerializeReference] private List<IUnitFeature> _features = new();

        public string ID
        {
            get => _id;
            set => _id = value;
        }

        /// <summary>Key referencing a <see cref="UnitDefinitionSO"/> in a catalog.</summary>
        public string Key
        {
            get => _key;
            set => _key = value;
        }

        public Unit() { }

        public Unit(string key)
        {
            _id = Guid.NewGuid().ToString();
            _key = key;
        }

        /// <summary>Deep copy preserving identity — features are cloned via <see cref="IUnitFeature.CloneFeature"/>.</summary>
        public Unit(Unit other)
        {
            if (other == null)
                return;
            _id = other._id;
            _key = other._key;
            for (int i = 0; i < other._features.Count; i++)
                _features.Add(other._features[i]?.CloneFeature());
        }

        public Unit Clone() => new Unit(this);

        // ── Features ──

        public void AddFeature(IUnitFeature feature) => _features.Add(feature);

        public T GetFeature<T>() where T : class, IUnitFeature
        {
            for (int i = 0; i < _features.Count; i++)
                if (_features[i] is T typed)
                    return typed;
            return null;
        }

        public bool HasFeature<T>() where T : class, IUnitFeature
        {
            for (int i = 0; i < _features.Count; i++)
                if (_features[i] is T)
                    return true;
            return false;
        }

        public IReadOnlyList<IUnitFeature> Features => _features;

        // ── Lifecycle ──

        public void Initialize()
        {
            for (int i = 0; i < _features.Count; i++)
                _features[i].OnInitialize(this);
        }

        public void Dispose()
        {
            for (int i = 0; i < _features.Count; i++)
                _features[i].OnDispose(this);
        }
    }
}
