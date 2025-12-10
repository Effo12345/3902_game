# Code Quality Review

**Author**: Logan

**Date**: 12/10

**Sprint**: 5

**File(s) reviewed**: PlayerMotionSystem (for sunshine content)

**Author of file(s)**: Logan King (for the section reviewed)

**Time spent reviewing**: 10

## Code Smell Analysis

### 1. Duplicated Code

- Some code duplicated from spawning fireballs

### 2. Long Method

- Added code makes the update method significantly longer

### 3. Large Class

- Larger than most systems

### 4. Long Parameter List

- N/A

### 5. Cyclomatic Complexity

- Many layers of branching.
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

**Proposed Change**: The added code to PlayerMovementSystem makes the class and working method longer and hurts its cohesiveness. Singular actions should be abstracted out into their own systems.

## Conclusion

The code is functional but not very maintainable and hurts the cohesion of the class.