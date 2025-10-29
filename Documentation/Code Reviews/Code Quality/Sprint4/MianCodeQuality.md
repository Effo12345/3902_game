# Code Quality Review

**Author**: Mian

**Date**: 2025-10-29

**Sprint**: 3

**File(s) reviewed**: EntityFactory.cs

**Author of file(s)**: Ethan

**Time spent reviewing**: 20

## Code Smell Analysis

### 1. Duplicated Code

* The “append components” flow is split between `AppendGeneralComponents` and `AppendSpecializedComponents`; both translate configuration into `entity.AddComponent(...)`. A single orchestrator with pluggable mappers would remove duplication of “read-config → add component” patterns.
* Child texture/atlas/input additions repeat the same “if config present → fetch from Assets → AddComponent” shape; consider a small registry mapping keys to lambda adders.

### 2. Long Method

* Methods are short; N/A. Minor: `AppendSpecializedComponents` grows as more specialized types are added—watch for length creep.

### 3. Large Class

* The class owns: registration, asset access, config application, cloning, and specialized component wiring. This mixes “factory”, “repository”, and “mapper” roles. Extract a `ComponentMapper` (config → components) and keep the factory focused on `BuildEntity`.

### 4. Long Parameter List

* Public API is compact. Internally, reliance on globals (`Assets`, `Entities`) hides parameters, which can be harder to test. Passing dependencies explicitly (DI) would make the parameter surface clearer.

### 5. Cyclomatic Complexity

* Low today. Risk area: `AppendSpecializedComponents` is a growing branching hub tied to content types. Consider strategy/registry to keep complexity flat as features scale.

### 6. Divergent Change

* Any new component type or asset source forces edits in multiple places (`AppendSpecializedComponents`, possibly cloning behavior). Centralize component wiring in a registry keyed by component/asset kind.

### 7. Shotgun Surgery

* Changes to cloning semantics or component add policy affect `AppendComponentsFromConfig`, `Clone<T>`, and potentially specialized adders. Introduce a single “component materializer” abstraction to limit spread.

### 8. Feature Envy

* Factory reaches into `Assets` for textures, atlases, input configs—behavior arguably belongs to an asset-aware mapper rather than the factory itself.

### 9. Data Clumps

* `Texture`, `Origin`, `Color` for `SpriteState` travel together; consider a small DTO or config section representing sprite defaults to reduce parameter/clump repetition.

### 10. Switch Statements

* No explicit switches. Implicit switching via if-chains in `AppendSpecializedComponents` could be replaced by a dictionary from “config key” → handler function.

### 11. Lazy Class/ Middle Man

* `EntityFactory` acts as a middleman to `Assets` and component creation. If most logic becomes “lookup-and-forward,” consider pushing responsibility into specialized mappers or an `IEntityComponentProvider`.

### 12. Message Chains

* Access is reasonably direct. Risk: callers depend on `BuildEntity(engine, key)` + global `RegisterAssets/Entity`. Prefer injecting `Assets` and `Entities` via constructor (if factory were non-static) to avoid global chains.

### 13. Deoderizing Comments

* The TODO on reflection cost is helpful; consider replacing with actionable task references or links to design notes. Comments otherwise minimal and purposeful.

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->

<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Introduce a **component materialization registry** that maps (component/config key → handler) and perform **one-time reflection caching** for cloning and type conversion.

Discuss whether the current implementation supports this change and why (how it would be hard if not supported,
or how it could be implemented if supported)

* Pros/cons to current approach

  * **Pros**: Simple, easy to read; minimal indirection; quick to wire new types.
  * **Cons**: Growing branching in `AppendSpecializedComponents`; repeated asset fetch patterns; reflection penalty per component; global state hurts testability.

* Pros/cons to registry + caching

  * **Pros**: Single place to add new types; reduced cyclomatic complexity; pay reflection cost once via cached `MethodInfo`/delegates; easier unit testing by swapping handlers.
  * **Cons**: Slight upfront complexity; requires bootstrapping order (register assets/handlers before use).

* Pros/cons to DI (non-static factory)

  * **Pros**: Eliminates global state; clearer dependencies; enables mocking `Assets`.
  * **Cons**: Requires touching call sites to pass a factory instance.

## Conclusion

`EntityFactory` is concise and functional but mixes concerns and relies on global state. Moving to a handler registry with cached reflection, plus (optionally) a non-static, DI-friendly design, will reduce branching, improve performance, and make the system easier to evolve and test.