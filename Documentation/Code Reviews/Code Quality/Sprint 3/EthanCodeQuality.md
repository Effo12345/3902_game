# Code Quality Review

**Author**: Ethan Rosati

**Date**: 10-27-25

**Sprint**: 3

**File(s) reviewed**: Quadtree.cs, CollisionDetectionSystem.cs

**Author of file(s)**: Logan

**Time spent reviewing**: 12

## Code Smell Analysis

### 1. Duplicated Code

- Lots of duplication of getting the middle of rectangles in Quadtree

### 2. Long Method

- Quadtree GetCollisions is ~60 lines, which is starting to get on the long side
- Otherwise looks good

### 3. Large Class

- Both classes look well-focused

### 4. Long Parameter List

- At most 1-2 parameters per function. Nice

### 5. Cyclomatic Complexity

- Lots of branching and indentation in Quadtree GetCollisions. Generally still readable however

### 6. Divergent Change

- N/A

### 7. Shotgun Surgery

- N/A

### 8. Feature Envy

- CollisionDetectionSystem is perfectly scoped. No feature envy of the Quadtree

### 9. Data Clumps

- N/A

### 10. Switch Statements

- N/A

### 11. Lazy Class/ Middle Man

- N/A

### 12. Message Chains

- Reasonable data passing between the two classes. Good work

### 13. Deoderizing Comments

- N/A

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Consider splitting GetCollisions into multiple methods

There are some pretty logical points it could be done, such as between the two foreach loops.
This would increase readability significantly and a little bit more refactoring work could help
reduce the indentation.

## Conclusion

Nice work overall, particularly on the functionality. Just a couple small changes would make
the code a lot more pleasant to look at.