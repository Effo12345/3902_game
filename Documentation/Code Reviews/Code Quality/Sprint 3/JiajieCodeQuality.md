# Code Quality Review

**Author**: Jiajie

**Date**: 10/26/25

**Sprint**: 3

**File(s) reviewed**: RenderSystem.cs

**Author of file(s)**: Stone

**Time spent reviewing**: 25 mins

## Code Smell Analysis

### 1. Duplicated Code

- N/A

### 2. Long Method

- N/A. Acceptable size

### 3. Large Class

- N/A

### 4. Long Parameter List

- N/A

### 5. Cyclomatic Complexity

- Nested branching in PositionAdjustment

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

**Proposed Change**: 
- Naming can be improved. Method and system names should explicitly indicate that they are responsible for camera rendering, as the current naming may be confusing or misleading.

## Conclusion
As mentioned above, namings can be improved to avoid confusion. Nested brancing in PositionAdjustment should be avoided. Overall, solid. 
