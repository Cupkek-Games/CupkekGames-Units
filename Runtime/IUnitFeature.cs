namespace CupkekGames.Units
{
    /// <summary>
    /// Composable behavior for <see cref="Unit"/>.
    /// Add features via <see cref="Unit.AddFeature"/> before calling Initialize().
    /// </summary>
    public interface IUnitFeature
    {
        /// <summary>Called at the end of <see cref="Unit.Initialize"/>.</summary>
        void OnInitialize(Unit unit);

        /// <summary>Called on <see cref="Unit.Dispose"/> — unsubscribe events, clean up.</summary>
        void OnDispose(Unit unit);
    }
}
