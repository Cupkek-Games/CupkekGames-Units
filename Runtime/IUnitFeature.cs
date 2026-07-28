using System;
using CupkekGames.Data;

namespace CupkekGames.Units
{
    /// <summary>
    /// Composable behavior for <see cref="Unit"/>.
    /// Add features via <see cref="Unit.AddFeature"/> before calling Initialize().
    /// Extends <see cref="IFeature"/> so the ecosystem FeatureDrawer handles the
    /// Inspector (type-picker dropdown for <c>[SerializeReference] List&lt;IUnitFeature&gt;</c>),
    /// same as <see cref="IUnitFeatureDefinition"/>.
    /// </summary>
    public interface IUnitFeature : IFeature
    {
        /// <summary>Called at the end of <see cref="Unit.Initialize"/>.</summary>
        void OnInitialize(Unit unit);

        /// <summary>Called on <see cref="Unit.Dispose"/> — unsubscribe events, clean up.</summary>
        void OnDispose(Unit unit);

        /// <summary>
        /// Deep copy of this feature's persisted state, used by <see cref="Unit.Clone"/>.
        /// Transient features (visual wiring, combat-time modifiers) are never part of a
        /// cloned unit; the default implementation therefore fails loud instead of
        /// producing a silently shallow copy.
        /// Covariant redeclaration of <see cref="IFeature.CloneFeature"/> (bridged below)
        /// so implementers keep returning <see cref="IUnitFeature"/>.
        /// </summary>
        new IUnitFeature CloneFeature() =>
            throw new NotSupportedException(
                $"{GetType().Name} does not support cloning. Persisted unit features must override CloneFeature().");

        IFeature IFeature.CloneFeature() => CloneFeature();
    }
}
