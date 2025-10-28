# Readability Review

**Author**: Mian

**Date**: 2025-10-27

**Sprint**: 3

**File(s) reviewed**: Quadtree.cs

**Author of file(s)**: Logan

**Time spent reviewing**: 15 minutes

## Readability Notes
Readability is acceptable but suffers from unclear naming, large methods, and redundant data. Applying consistent naming, extracting helper methods, and reducing hidden assumptions will make the code far easier to follow and maintain.

## Positive Aspects

* **Single Responsibility**
  The class clearly focuses on spatial partitioning and collision collection.

* **Helpful Comments**
  Notes like leave in current node if intersecting multiple quadrants clarify intent.

## Areas for Improvement

* **Naming Mismatch**
  `intersectsQuadrants` does not clearly reflect its purpose. Better alternatives: `CrossesAxes`, `OverlapsMultipleQuadrants`.

* **Field Redundancy**
  Storing both `Bounds` and `X/Y/Width/Height` forces the reader to reconcile two sources of truth.

* **Inconsistent Naming Style**
  Private fields in PascalCase (`X`, `Y`) violate standard conventions. Should be `_x`, `_y`, etc.

* **Hidden Assumptions**
  `TryGetComponent` failures are silently ignored in some places. Readers must assume all entities have `CollisionInfo`. Making this explicit improves clarity.

* **Branching Logic Clarity**
  Collision direction code overrides values without clear branching, making it confusing. A proper `if/else` structure would be easier to read and safer.

* **Method Size**
  `GetCollisions` does too much: traverses children, checks local entities, computes direction, and constructs events. Splitting into smaller methods would reduce cognitive load.