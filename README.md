# CupkekGames Units

Universal Unit primitive. Replaces type-discriminating subclass hierarchies (HeroSO/EnemySO/NpcSO style) with composition: a single `Unit` carries identity + a list of `IUnitFeature`, while `UnitDefinitionSO` carries a list of `IUnitFeatureDefinition`. Other framework packages (Character, Combat, NPC) plug feature definitions in.

## What's inside

**Runtime** (`CupkekGames.Units.asmdef`)

- `Unit` — runtime identity + `List<IUnitFeature>`
- `UnitDefinitionSO` — ScriptableObject template with `[SerializeReference] List<IUnitFeatureDefinition>` + `GetDefinition<T>()`
- `IUnitFeature` / `IUnitFeatureDefinition` — feature contracts
- `UnitView` — shared visual MonoBehaviour on prefabs

## Dependencies

Asmdef references resolve via the CupkekGames scoped registry: `data` (for IFeature contract), `services`. Bring your own copy via the registry.
