# Code Quality Review

**Author**: Logan

**Date**: 11/16/2025

**Sprint**: 4

**File(s) reviewed**: InputSystem.cs

**Author of file(s)**: Mian

**Time spent reviewing**: 20

## Code Smell Analysis

### 1. Duplicated Code

- Some duplicated code when checking if a button affects velocity or acceleration, and when publishing input events.

### 2. Long Method

- Update method is fairly long, about 90 lines.

### 3. Large Class

- N/A

### 4. Long Parameter List

- N/A

### 5. Cyclomatic Complexity

- Some branching that could be simplified when checking what action a button corresponds to.

### 6. Divergent Change

- N/A

### 7. Shotgun Surgery

- N/A

### 8. Feature Envy

- N/A

### 9. Data Clumps

- You could call the input event a data clump, but it's how our system works

### 10. Switch Statements

- N/A

### 11. Lazy Class/ Middle Man

- N/A

### 12. Message Chains

- Message chains in the class are intentional with the ECS system.

### 13. Deoderizing Comments

- More descriptive variable names could eliminate the need for some comments.

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**:

- More descriptive variable names could make the code more readable. Some variable names right now are abreviations or acronyms, like "ab" or "ms".
- There is not much but some duplicated code that could be abstracted using helper methods.

## Conclusion

The input system heavily simplifies getting input from the user and corresponding buttons to actions using input events. The class has some slight flaws in readability due to non-descriptive variable names and reused code, but is nonetheless well designed and functional.