# Code Quality Review

**Author**: Logan

**Date**: 12/10/25

**Sprint**: 5

**File(s) reviewed**: InputSystem.cs (for controller input) PlayerMotionSystem.cs (for jump height changes)

**Author of file(s)**: Mian

**Time spent reviewing**: 10

## Code Smell Analysis

### 1. Duplicated Code

- In InputSystem.cs, some duplicated code in gamepad section - right and left stick do the same thing

### 2. Long Method

- InputSystem.cs has a very long "OnAction()" method, PlayerMotionSystem.cs has a very long Update() method

### 3. Large Class

- Both classes are pretty large

### 4. Long Parameter List

- N/A

### 5. Cyclomatic Complexity

- Some significant branching in PlayerMotionSystem for the jumping behavior.

### 6. Divergent Change

- N/A

### 7. Shotgun Surgery

- N/A

### 8. Feature Envy

- N/A

### 9. Data Clumps

- Lots of different events made in InputSystem.cs (new one added for controller support)

### 10. Switch Statements

- N/A

### 11. Lazy Class/ Middle Man

- N/A

### 12. Message Chains

- InputSystem sends events to PlayerMotionSystem

### 13. Deoderizing Comments

- N/A

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: 

For PlayerMotionSystem, the jump control component could be modified.

- Pros: Less data is held in the PlayerMotionSystem file.
- Cons: To take advantage of ECS, this feature might make more sense as a component if the "MAX_JUMP_HOLD_FRAMES" value was held in the component. This way its specific to the entity that has the component.

For InputSystem, duplicated code for publishing events for the two sticks could be put into one section.

- Pros: The two sticks could work differently if desired.
- Cons: Duplicated code hurts maintainability.

## Conclusion

Both files are well designed. Input system takes care of all input controllers and is functional and polished for each. The variable jump height section of PlayerMotionSystem is short and simple.