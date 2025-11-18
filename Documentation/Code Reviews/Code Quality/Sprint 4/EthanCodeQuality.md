# Code Quality Review

**Author**: Ethan

**Date**: 11-16-25

**Sprint**: 4

**File(s) reviewed**: MarioCollisionSystem.cs, NonMarioCollisionSystem.cs

**Author of file(s)**: Logan

**Time spent reviewing**: 23

## Code Smell Analysis

### 1. Duplicated Code

- Lots of duplicated code between the different cases. Consider splitting into methods somehow or making the event handling more data-driven.

### 2. Long Method

- The HandleCollision method in both classes is rather long. The helper functions in MarioCollisionSystem are a good start, but more would be better.
- Other methods are short and straightforward. Nice

### 3. Large Class

- These classes approach the upper limit of what we should be aiming for
- They are a reasonable length currently, but would probably be better split up somehow

### 4. Long Parameter List

- N/A. Stuff that gets passed around is packaged well into other classes.

### 5. Cyclomatic Complexity

- Very high cyclomatic complexity in both class' HandleCollision methods
- Try and avoid switch cases entirely if possible
- Multiple layers of if-else statements inside a case is too much nesting to be easily readable

### 6. Divergent Change

- This is one of the biggest drawbacks of this design decision. Any change that needs to be made to any kind of collision handling MUST happen in one of these two files
- Smaller, more specialized systems can reduce the number of interactions affected by a given change to the code

### 7. Shotgun Surgery

- One benefit of this design is that it centralizes all the changes for collision handling to 2 files, so this isn't an issue

### 8. Feature Envy

- N/A

### 9. Data Clumps

- There are lots of instances of position and velocity being modified together in very similar ways across multiple cases. Consider if these could be bundled together somehow

### 10. Switch Statements

- As mentioned above, the enormous switch statements at the core of both of these classes carry a very significant design weight. 
- Aim to replace these with a more data-driven approach that generates behaviors from the components an entity contains, to the greatest extent possible

### 11. Lazy Class/ Middle Man

- N/A. Both classes do significant, meaningful work

### 12. Message Chains

- Not very common, but there are a few instances of things like `secondaryCollisionInfo.HitBox.Center.X` that could be cleaned up a bit

### 13. Deoderizing Comments

- N/A. Comments are generally insightful

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: Generalize to a data-driven approach

Try to avoid directly hard-coding these collision interactions to the greatest extent possible. The particular way this was done, coding a special case for each "type" of entity, doesn't take full advantage of the ECS system.
Some suggestions:

- Give items a "CollectionSound" component, or create a more generalized "PlayerInteractionSound" component to specify what sound should be played when a player collects an item. 
- Similar approach for items setting powerup state: confer that information via a component attached to the item
- At some point, there will need to be some amount of hard-coding for specific enemies. In this case, make each one its own system. Maybe these systems don't subscribe to the collision event directly but have some sort of middleman that parses the collision event into a generic "EnemyCollision" event that can be parsed by these specialized systems more effectively
- Extend the smaller, more specialized systems approach to the rest of the design; have one system that handles ALL collisions with static blocks/ground, etc.
## Conclusion

Overall, the code is clearly functional and suited for the task given to it. That said, there is some refactoring work that could be done that would significantly improve the quality and maintainability of the code going forward