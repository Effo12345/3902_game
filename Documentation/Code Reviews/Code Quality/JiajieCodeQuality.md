# Code Quality Review

**Author**: Jiajie 

**Date**: 11/16/25

**Sprint**: 4

**File(s) reviewed**: TimerSystem

**Author of file(s)**: Stone

**Time spent reviewing**: 15 mins

## Code Smell Analysis

### 1. Duplicated Code

- N/A

### 2. Long Method

- Consider seperating the finding/identifying the timer entity out of Update

### 3. Large Class

- N/A

### 4. Long Parameter List

- N/A

### 5. Cyclomatic Complexity

- Several conditional statements in Update but not too concerning

### 6. Divergent Change

- N/A

### 7. Shotgun Surgery

- N/A

### 8. Feature Envy

- N/A

### 9. Data Clumps

- N/A

### 10. Switch Statements

- N/A

### 11. Lazy Class/ Middle Man

- N/A

### 12. Message Chains

- N/A

### 13. Deoderizing Comments

- N/A

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Discuss something that could make the code better

You can consider creating a Timer Component that stores the time remaining value and timer limit value and have it configured in level loader. Eliminate magic numbers

## Conclusion

Overall, solid