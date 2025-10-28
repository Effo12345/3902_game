# Code Quality Review

**Author**: Mian

**Date**: 2025-10-27

**Sprint**: 3

**File(s) reviewed**: Quadtree.cs

**Author of file(s)**: Logan

**Time spent reviewing**: 35

## Code Smell Analysis

### 1. Duplicated Code

* Insertion logic appears both in `Insert()` and again during `subdivide()` redistribution, leading to two places that must stay in sync.
* Child traversal patterns in `GetCollisions()` are repeated for each quadrant; consider a helper that takes a child reference and the query rectangle to avoid repetition.
* Boundary calculations for the four child rectangles are repeated constructs; a small factory/helper could generate them from `Bounds` once.

### 2. Long Method

* `GetCollisions()` combines: tree traversal, local collision checks, intersection math, and direction resolution. This makes it hard to follow and test.
* Refactor into smaller, intention-revealing methods—e.g., `CollectChildCollisions`, `CollectLocalCollisions`, `ResolveDirection`, `MakeCollisionEvent`.

### 3. Large Class

* The class manages storage, spatial subdivision, collision resolution, and event creation. These are multiple responsibilities.
* Consider extracting: (a) a `QuadtreeNode` for topology, (b) a `CollisionResolver` for direction/physics semantics, and (c) a simple `CollisionEventFactory`.

### 4. Long Parameter List

* Not evident in public APIs, but several methods implicitly depend on many fields (`Bounds`, `X/Y/Width/Height`, capacity, children). This is an implicit long parameter list through shared state.
* Collapsing duplicate state (see Data Clumps) and passing compact DTOs (e.g., a `Quadrant` struct) would reduce hidden parameter surfaces.

### 5. Cyclomatic Complexity

* The direction computation branch picks vertical vs. horizontal by comparing intersection width/height, then does nested conditional checks; this inflates complexity and risks edge-case bugs.
* `GetCollisions()`’s multiple early exits and per-child branching also elevate complexity. Splitting methods and using clear `if/else` (not overwritten assignments) will help.

### 6. Divergent Change

* Any change to capacity policy or quadrant assignment requires edits in multiple places (both `Insert()` and `subdivide()`), indicating the class tends to change for more than one reason.
* Centralize the “choose quadrant vs. keep at node” rule in a single helper (e.g., `AssignNodeFor(Rectangle)`), and reuse it everywhere.

### 7. Shotgun Surgery

* Fixes to collision direction or hitbox acquisition would require touching several call sites where `TryGetComponent<CollisionInfo>` is used without a single abstraction.
* Introduce `bool TryGetHitBox(Entity, out Rectangle)` to contain these checks in one place.

### 8. Feature Envy

* Direction resolution logic operates mostly on geometric data (`Rectangle`, positions), not on quadtree structure; it “envies” `Rectangle`/collision domain.
* Move direction resolution into a dedicated `CollisionResolver` or an extension method over rectangles to reduce envy and improve reuse.

### 9. Data Clumps

* `X`, `Y`, `Width`, `Height` mirror `Bounds` and travel together across calculations. Keeping both sets introduces drift risk and cognitive load.
* Prefer a single source of truth (`Bounds`) and expose derived read-only properties if necessary.

### 10. Switch Statements

* Direction selection can be expressed as a small `switch` (or pattern matching) on an enum representing dominant axis & sign, replacing nested conditionals.
* Child quadrant selection could similarly use an index (0..3) computed from comparisons and then a `switch` to select `topLeft`/`topRight`/`bottomLeft`/`bottomRight`.

### 11. Lazy Class/ Middle Man

* Not a clear lazy class, but there is some “middle-man” behavior: the quadtree constructs `CollisionEvent` objects, which could be delegated to an event factory to keep the node focused on spatial concerns.
* If no additional logic exists around event creation, consider moving it out or at least isolating it.

### 12. Message Chains

* Potential chains like `entity.TryGetComponent(...).HitBox` (when not guarded) couple callers to internal details. Consolidate through `TryGetHitBox` to avoid chained lookups and NRE risk.
* Accessing child nodes via repeated `node.child.child` is not present, but iterating children via an array would prevent future chains.

### 13. Deoderizing Comments

* Some comments reiterate what the code says (“if overlaps multiple quadrants, keep in this node”), yet implementation deviates—this can mislead.
* Replace restatements with intent-revealing comments where logic is non-obvious (e.g., define the rule: *crosses axes ⇒ store here; otherwise ⇒ single child*). Remove noise comments.

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->

<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Unify insertion and subdivision policy so that an entity is inserted into **exactly one** child quadrant when it does **not** cross the center axes; otherwise it remains in the current node. Extract collision direction logic into a dedicated helper/service and remove redundant boundary fields.

Discuss whether the current implementation supports this change and why (how it would be hard if not supported, or how it could be implemented if supported)

* Pros/cons to current approach

  * **Pros**: Simple to reason about; easy to implement initially.
  * **Cons**: Duplicated storage (entities in multiple children), inflated query cost, higher memory use, and correctness bugs (e.g., direction overwrite).

* Pros/cons to unified insertion rule

  * **Pros**: Eliminates duplication, reduces recursion, improves query time, and clarifies invariants.
  * **Cons**: Requires a reliable `GetQuadrantIndex` and a redistribution pass in `subdivide()` following the same rule.

* Pros/cons to extracting collision logic

  * **Pros**: Lowers cyclomatic complexity in `GetCollisions()`, enables unit testing of direction resolution, and separates spatial structure from physics semantics.
  * **Cons**: Introduces another small abstraction (a resolver/factory), but with clear payoff.

## Conclusion

The quadtree’s overall design is appropriate, but correctness and maintainability suffer due to duplicated insertion logic, elevated complexity in collision handling, and redundant boundary state. By centralizing quadrant assignment, extracting collision resolution, and removing data clumps, the implementation will become faster, safer, and easier to evolve.
