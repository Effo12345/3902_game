# Code Quality Review

**Author**: Stone Jiang

**Date**: 2025/10/27

**Sprint**: Sprint 3

**File(s) reviewed**: StartScreenSystem.cs

**Author of file(s)**: Jiajie

**Time spent reviewing**: 20 min

## Code Smell Analysis

### 1. Duplicated Code

- Should be ok

### 2. Long Method

- Not particularly long.

### 3. Large Class

- No large class.

### 4. Long Parameter List

- No long parameter list.

### 5. Cyclomatic Complexity

- Not too bad. Maybe the lots of ifs are not the best way.

### 6. Divergent Change

- Should be ok

### 7. Shotgun Surgery

- Should be ok

### 8. Feature Envy

- Should be ok

### 9. Data Clumps

- Should be ok

### 10. Switch Statements

- Again, despite not literally being "switch" statements, be careful about lots of ifs

### 11. Lazy Class/ Middle Man

- Should be ok

### 12. Message Chains

- No long message chains

### 13. Deoderizing Comments

- No deorderizing comments

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Discuss something that could make the code better

Discuss whether the current implementation supports this change and why (how it would be hard if not supported,
or how it could be implemented if supported)

- Change if statements to mapping
