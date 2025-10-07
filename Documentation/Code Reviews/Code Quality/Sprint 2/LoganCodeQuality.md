# Code Quality Review

**Author**: Logan

**Date**: 10/5/2025

**Sprint**: Sprint 2

**File(s) reviewed**: MarioActionSystem.cs

**Author of file(s)**: Mian

**Time spent reviewing**: 60

## Code Smell Analysis

### 1. Duplicated Code

- N/A

### 2. Long Method

- Update() method is somewhat long. This could be shortened by creating a factory for creating a fireball, and reducing branching.

### 3. Large Class

- N/A

### 4. Long Parameter List

- N/A

### 5. Cyclomatic Complexity

- Lots of branching that could be avoided by making use of the input system.

### 6. Divergent Change

- Class is responsible for three things: getting input, changing mario's state, and throwing a fireball if mario is fire mario.

### 7. Shotgun Surgery

- If the name of a powerup state is changed it would also need changed here, thats all I notice.

### 8. Feature Envy

- N/A

### 9. Data Clumps

- States stored as strings are identical between classes, as well as some magic numbers/values in creating fireballs that should probably be constants.

### 10. Switch Statements

- N/A

### 11. Lazy Class/ Middle Man

- Class performs essential functions.

### 12. Message Chains

- N/A

### 13. Deoderizing Comments

- One TODO comment to resolve

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Make a JSON file for controls for switching mario's powerup state and throwing a fireball, mapping each input to a vector value. Then, check for those inputs in a HandleInput method. This way you won't need to check for inputs directly in the system. Also, cut down on branching to cut down on length and make the code simpler. Finally, create an entity factory to create the fireball to cut down on length and reduce the scope of what the class is doing (single responsibility principle).

Creating a JSON file and changing mario's state in a HandleInput method will be straightforward and similar to other systems that use the input system (such as EnemyMovement system in the enemy refactoring branch or the DemoMovementSystem). Ideally, an enumeration for mario's state would be associated with each input, and get rid of the need for branching, but this may not be straightforward with the current input system. An entity factory should be straightforward to make referencing the EnemyFactory in the enemy refactoring branch.

- Pros: The current approach is readable and simple. I know what the code is doing just by looking at it, and it functions well.
- Cons: The class is in control of some things that could be moved elsewhare, namely the hardcoded keyboard input and the fireball creation. The need for  branching also compromises the code's maintainability (another branch would need added for a new state)
- Note: Keep in mind the fireball will need to bounce on the ground when a true ground is added.

## Conclusion

The MarioActionSystem works well and provides a good baseline for how mario's state and actions will be handled in the future. The system's flaws do not compromise its functionality, simplicity, or readability, and can be fixed easily if done before more features are added. 