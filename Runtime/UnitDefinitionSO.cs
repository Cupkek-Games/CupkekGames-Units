using System.Collections.Generic;
using CupkekGames.Data;
using UnityEngine;

namespace CupkekGames.Units
{
    /// <summary>
    /// Universal unit definition. Shared identity fields + composable feature definitions.
    /// List uses <see cref="IUnitFeatureDefinition"/> (extends IFeature) so the
    /// drawer only shows unit-relevant feature types, not item features.
    /// </summary>
    [CreateAssetMenu(fileName = "UnitDefinition", menuName = "CupkekGames/Unit/Definition")]
    public class UnitDefinitionSO : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _avatar;

        [SerializeReference] private List<IUnitFeatureDefinition> _featureDefinitions = new();

        public string Name => _name;
        public string Description => _description;
        public Sprite Avatar => _avatar;
        public IReadOnlyList<IUnitFeatureDefinition> FeatureDefinitions => _featureDefinitions;

        public T GetDefinition<T>() where T : class, IUnitFeatureDefinition
        {
            for (int i = 0; i < _featureDefinitions.Count; i++)
                if (_featureDefinitions[i] is T typed)
                    return typed;
            return null;
        }

        public void AddDefinition(IUnitFeatureDefinition def) => _featureDefinitions.Add(def);

        public bool HasDefinition<T>() where T : class, IUnitFeatureDefinition
        {
            for (int i = 0; i < _featureDefinitions.Count; i++)
                if (_featureDefinitions[i] is T)
                    return true;
            return false;
        }
    }
}
