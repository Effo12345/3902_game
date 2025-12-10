# Code Quality Review

**Author**: Logan

**Date**: 12/10

**Sprint**: 5

**File(s) reviewed**: GameManager.cs, StartScreenSystem.cs, CameraSystem.cs (locking changes)

**Author of file(s)**: Ethan

**Time spent reviewing**: 10

## Code Smell Analysis

### 1. Duplicated Code

- (GameManager): N/A
- (StartScreenSystem): N/A
- (CameraSystem): N/A

### 2. Long Method

- (GameManager): N/A
- (StartScreenSystem): Update method is very short
- (CameraSystem): N/A

### 3. Large Class

- (GameManager): N/A
- (StartScreenSystem): Very short class
- (CameraSystem): N/A

### 4. Long Parameter List

- (GameManager): N/A
- (StartScreenSystem): N/A
- (CameraSystem): N/A

### 5. Cyclomatic Complexity

- (GameManager): N/A
- (StartScreenSystem): No excessive branching.
- (CameraSystem): N/A

### 6. Divergent Change

- (GameManager): N/A
- (StartScreenSystem): N/A
- (CameraSystem): N/A

### 7. Shotgun Surgery

- (GameManager): N/A
- (StartScreenSystem): N/A
- (CameraSystem): N/A

### 8. Feature Envy

- (GameManager): N/A
- (StartScreenSystem): Really wants to get into camera system
- (CameraSystem): N/A

### 9. Data Clumps

- (GameManager): Takes advantage of InputEvent data clump
- (StartScreenSystem): CameraLockEvent is a data clump
- (CameraSystem): Takes advantage of CameraLockEvent for the camera locking

### 10. Switch Statements

- (GameManager): N/A
- (StartScreenSystem): N/A
- (CameraSystem): N/A

### 11. Lazy Class/ Middle Man

- (GameManager): N/A
- (StartScreenSystem): Purpose may be somewhat small compared to other systems, especially since most the work its doing is passing stuff through events
- (CameraSystem): N/A

### 12. Message Chains

- (GameManager): Requests input event
- (StartScreenSystem): Passes CameraLockEvent
- (CameraSystem): Requests camera lock event

### 13. Deoderizing Comments

- (GameManager): Comment warning things can break easily if this class is changed
- (StartScreenSystem): N/A
- (CameraSystem): Some comments that explain decisions not clear in code

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: 
- (GameManager): Some functions just call another function, which is somewhat awkward but maybe necessary if there's no other way to get the current file.
- (StartScreenSystem): Class is somewhat envious of camera system. It could potentially be easier to put this functionality there
- (CameraSystem): Potentially add camera locking functionality in this system, but may cause more problems than it solves.

## Conclusion

- (GameManager): Class is short, cohesive, and single purpose. 
- (StartScreenSystem): Class is useful and functional for locking the camera. Potential feature envy with camera system, but it makes sense to abstract this feature out.
- (CameraSystem): Class is functional and works well for moving the camera. 