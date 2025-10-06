# Code Quality Review

**Author**: Ethan

**Date**: 11/05/25

**Sprint**: 2

**File(s) reviewed**: EnemyMovementSystem.cs

**Author of file(s)**: Logan

**Time spent reviewing**: 12 minutes

## Code Smell Analysis

### 1. Duplicated Code

- HandleInput duplicates variable setting inside the if statements

### 2. Long Method

- HandleInput is slightly on the long side. See above

### 3. Large Class

- N/A

### 4. Long Parameter List

- N/A

### 5. Cyclomatic Complexity

- N/A

### 6. Divergent Change

- There are some hard-coded values in here that need removed in future

### 7. Shotgun Surgery

- Very successfull in avoiding this one. Entire enemy behavior is either delegated to existing systems or handled here very simply

### 8. Feature Envy

- Class is focused and useful. Nice work

### 9. Data Clumps

- N/A

### 10. Switch Statements

- N/A

### 11. Lazy Class/ Middle Man

- This seems like the correct scope for this system. Will likely do more in future

### 12. Message Chains

- At most 2 levels. Very good

### 13. Deoderizing Comments

- Temporary, sprint2-only values are clearly labeled. Good

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Enemy damaging behavior

The existing implementation would support this relatively easily. One could add a new subscription to a collision or damage event and then apply that state to the enemy.

- This would likely require some refactoring to work with existing behavior
- Doing it this way would mean this system generally still operates on the same set of objects

## Conclusion

Overall, this system is tightly-focused to the singular, simple task it should accomplish; it successfully leverages existing systems. It also passes the majority of the code smells tests with only a few areas for improvement.