# Readability Review

**Author**: Logan

**Date**: 10/5/2025

**Sprint**: Sprint 2

**File(s) reviewed**: ContentLibrary.cs, EntityFactory.cs

**Author of file(s)**: Stone

**Time spent reviewing**: Time spent reviewing (in minutes)

## Readability Notes

### Positive Aspects

- Very clean classes!
- In ContentLibrary: Very clear what this class is for and what the code does. Variable declarations and assignments are grouped together appropriately andn the file is very concise.
- In EntityFactory: Clear to see that each method creates a different entity by adding various components.

### Areas for Improvement

- In ContentLibrary: At first glance I'm not sure what HighlightData() does. Isn't there another way to do what this is doing with json config files and the animation system? It also seems like its just getting called in the Load() method again.
- Inconsistent naming convention at blockSprites and tilesAnimationConfig. Probably use either blocks or tiles, not both.
- In EntityFactory: There's some leftover veriables and comments, like the default enemy velocity.