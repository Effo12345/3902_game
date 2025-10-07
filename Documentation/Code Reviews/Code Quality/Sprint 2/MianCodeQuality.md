# Code Quality Review

**Author**: Mian

**Date**: 2025-10-06

**Sprint**: 2

**File(s) reviewed**: BlockClickSystem.cs

**Author of file(s)**: Jiajie

**Time spent reviewing**: 30 minutes

## Code Smell Analysis

### 1. Duplicated Code

- Repeated `TryGetComponent` calls could be abstracted to improve maintainability.

### 2. Long Method 

- The `Update` method is too large, mixing input handling, click detection, entity iteration, and event publishing. This violates single responsibility.

### 3. Large Class 

- Class size is reasonable, not an issue.

### 4. Long Parameter List 

- No long parameter lists present.

### 5. Cyclomatic Complexity 

- `Update` has many nested conditions and early exits, increasing complexity.

### 6. Divergent Change 

- Mouse input logic and entity interaction rules are tightly coupled, leading to frequent changes in `Update` if requirements shift.

### 7. Shotgun Surgery 

- Depends on many components (Bump, Break, SpriteState, Position, Scale). Changing component structures could cause ripple effects.

### 8. Feature Envy 

- `TryGetHitRect` heavily manipulates `SpriteState` and `Position`. This calculation might belong inside the entity/related components.

### 9. Data Clumps 

- `SpriteState`, `Position`, and `Scale` often appear together, potentially forming a data clump.

### 10. Switch Statements 

* No explicit switch, but multiple conditional checks exist.

### 11. Lazy Class / Middle Man 

* Not applicable.

### 12. Message Chains 

* `Engine.PubSub.Publish` is acceptable, not excessive.

### 13.  Deodorizing Comments 

* Some comments simply restate code behavior, which could be removed for conciseness.

## Future Improvement

**Proposed Change**: 
* Extract input handling, entity filtering, and event dispatch into smaller methods.
* Allow components to expose their own hit-rects to reduce coupling.
* Reduce nested conditions by restructuring logic.

## Conclusion

The system works functionally, but `Update` is overloaded. Refactoring will make the codebase more maintainable and easier to extend.