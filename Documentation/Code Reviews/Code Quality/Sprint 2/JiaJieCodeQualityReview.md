# Code Quality Review

**Author**: Jiajie Chen

**Date**: 10/5/25

**Sprint**: 2

**File(s) reviewed**: EntityFactory, ContentLibrary

**Author of file(s)**: Stone

**Time spent reviewing**: ~1 hrs - 2hrs

## Code Smell Analysis

### 1. Duplicated Code

In EntityFactory.cs, almost all configurators have the same component add lines like Velocity, Rotation, Scale, SpriteState, AnimationConfig and AnimationState. You can consider helper method that does all that so the file can be shorter, neater and more maintainable

In ContentLibrary, repeated pattern for Content.Load<Texture2D> and SpriteSheetLoader.Load(File.ReadAllText())

### 2. Long Method

- No comments. All configurator parameters looks good

### 3. Large Class

EntityFactory contains all configurators we are currently having in this game which is made intentional. We can consider putting all this data into external JSON file in the future. For now, it is doing what is is intended.
Same for ContentLibrary

### 4. Long Parameter List

- In EntityFactory, each configurator only takes two parameters, so that part is fine. Some AddComponent calls have many arguments, but since those belong to the component types thats out of scope for this file

### 5. Cyclomatic Complexity

- No comments. Looks good

### 6. Divergent Change

- In EntityFactory, hard coding Scale value or Position value inside the configurator can cause divergent change. The factory must be modified for any changes in sizing or placement policys in the game

### 7. Shotgun Surgery

- In EntityFactory, some concerns may be that if component definitions change (like adding or removing fields), every configurator with that component will need to be updated which can be a lot.

### 8. Feature Envy

- In EntityFactory, lines like "lib.goombaAnimationConfig.States["walking"].Frame[0]" looks into ContentLibrary
  knowing which state exists and in which frame

### 9. Data Clumps

- In EntityFactory, Position, Rotation, Scales and SpriteState, AnimationConfi, Animation State clumps together. Maybe create a component to lump them together?
  In ContentLibrary, Texture and AnimationConfig are often paired together
### 10. Switch Statements

- No comments. No switch statements

### 11. Lazy Class/ Middle Man

- No comments

### 12. Message Chains

- There are chains with three dots found in EntityFactory. None exceeding four so all is good

### 13. Deoderizing Comments

- In EntityFactory, "/** No protection for wrong name. There is no reason for one to want to put a wrong name */", explains why there is no protection feauture

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**: 
In Entity Factory, even though you explained why there's no protection feature, I still recommend adding one since it's good professional habit
In both files, putting data in a JSON file can be considered in the future

## Conclusion
Overall solid. Both does what they are intended. There are some tight coupling with EntityFactory and ContentLibrary which may lead to shotgun surgery or divergent change. Also, in ContentLibrary, I'm not sure why you choose the name HighlightData for the method. I think it can be misleading and there are better names for it.