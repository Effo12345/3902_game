# Code Quality Review

**Author**: Logan

**Date**: 10/26/2025

**Sprint**: 3

**File(s) reviewed**: InputSystem.cs

**Author of file(s)**: Mian

**Time spent reviewing**: 20

## Code Smell Analysis

### 1. Duplicated Code

- N/A

### 2. Long Method

- The update method is fairly long, but everything it does seems relevant.

### 3. Large Class

- N/A

### 4. Long Parameter List

- N/A

### 5. Cyclomatic Complexity

- Update has a double for loop with nested if statements inside.
- The double for each loop seems necessary, but see if the branching with if else statements can be simplified.

### 6. Divergent Change

- N/A

### 7. Shotgun Surgery

- N/A

### 8. Feature Envy

- N/A

### 9. Data Clumps

- Potentially, a class or struct could be made to publish as an input event.

### 10. Switch Statements

- There's one switch statement for checking if a mouse button is down.
- This seems necessary if there isn't a way to get the mouse state in the same way you can get a keyboard state.

### 11. Lazy Class/ Middle Man

- N/A

### 12. Message Chains

- This system creates input events that get passed to the pubsub, which get passed to the systems that call for it.
- Message chains like these will always be present when publishing events through the pubsub system.

### 13. Deoderizing Comments

- If anything, some of these comments could be more descriptive.

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Create only one type of input event, instead of axis / action events. 

The complication this would add to the input system might not be worth it for the benefit it would provide, but it could be convenient to only have one type of input event for systems that care input it to check.

Pros: 
- System provides a lot of information in the events it publishes to make performing an action in game based on input much simpler to write.
- Input buttons can also easily be changed through a config file. 

Cons: 
- If we want to do something for any possible input (eg. "Press any key to start"), it seems like we have to check for three different input events.

Note: Remove commented out code

## Conclusion

The new InputSystem provides a much more robust way of checking for input in our code. The class is well designed and serves a single purpose, and inputs are easy to modify through config files. Checking for input across different systems will be made much simpler by being able to find which action was performed and which entity it applies to in an input event.