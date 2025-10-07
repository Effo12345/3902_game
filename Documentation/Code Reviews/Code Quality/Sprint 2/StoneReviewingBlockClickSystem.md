# Code Quality Review

**Author**: Stone

**Date**: 2025/10/05

**Sprint**: Sprint 2

**File(s) reviewed**: BlockClickSystem.cs

**Author of file(s)**: Jiajie

**Time spent reviewing**: 20 min

## Code Smell Analysis

### 1. Duplicated Code

- The code reasonably avoids copy-paste logic.

### 2. Long Method

- Methods are short enough.

### 3. Large Class

- Only two short methods, no problem.

### 4. Long Parameter List

- No problem.

### 5. Cyclomatic Complexity

- It is just for Sprint 2, so no worries.

### 6. Divergent Change

- I would say putting Bump and Break here is a disaster, but it is just for Sprint 2, so not that bad. But be sure to minimize mentioning other's affairs, even just for testing purpose, so that systems for Bump and Break will have to do their job.

### 7. Shotgun Surgery

- Again, it is just for Sprint 2, so no worries.

### 8. Feature Envy

- Within tolerence that should be given to a system.

### 9. Data Clumps

- No problem.

### 10. Switch Statements

- Basically no problem. If any, refer to Divergent Change.

### 11. Lazy Class/ Middle Man

- It is doing its own job.

### 12. Message Chains

- No problem.

### 13. Deoderizing Comments

- Some of the comments literally repeat what the code is.

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Bump and Break should instead be implemented in their own systems

Discuss whether the current implementation supports this change and why (how it would be hard if not supported,
or how it could be implemented if supported)

- cons: Assume this is a class just for testing purpose, just like Sprint 2, so it should not do the concreate functional classes' jobs.

## Conclusion

By making sure that the class only mentions about the concept of click trigger, things should be all set.
