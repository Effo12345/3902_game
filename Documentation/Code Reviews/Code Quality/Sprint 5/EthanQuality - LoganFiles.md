# Code Quality Review

**Author**: Ethan

**Date**: 10-10-25

**Sprint**: 5

**File(s) reviewed**: PlayerMotionSystem.cs

**Author of file(s)**: Logan

**Time spent reviewing**: 5

## Code Smell Analysis

### 1. Duplicated Code

- Multiple places look for jump input phase. Try splitting this check out

### 2. Long Method

- OnAction is too long

### 3. Large Class

- PlayerMotionSystem is on the upper end of acceptable for class length

### 4. Long Parameter List

- N/A

### 5. Cyclomatic Complexity

- Lots of nested if statements. Be wary

### 6. Divergent Change

- N/A

### 7. Shotgun Surgery

- All operations are centralized, N/A

### 8. Feature Envy

- N/A

### 9. Data Clumps

- N/A

### 10. Switch Statements

- No switch statement, but a similar logical flow

### 11. Lazy Class/ Middle Man

- N/A. Class does significant work

### 12. Message Chains

- Maximum 3 levels

### 13. Deoderizing Comments

- N/A

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Split out each if statement into a separate system or method

This approach would be more flexible and allow for greater code reuse. The system as it is now would require heavily modifying the code for any necessary changes.

## Conclusion

Overall, the system functions as expected, though the quality of the code could be improved through a bit more abstraction.