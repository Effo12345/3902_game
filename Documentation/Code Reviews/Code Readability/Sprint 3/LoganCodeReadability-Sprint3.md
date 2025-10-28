# Readability Review

**Author**: Logan King

**Date**: 10/26/2025

**Sprint**: 3

**File(s) reviewed**: RenderSystem.cs

**Author of file(s)**: Stone

**Time spent reviewing**: 20

## Readability Notes
RenderSystem.cs is readable and easy to understand. The minor flaws it has can be quickly fixed with clarifying comments, more descriptive variable names, or changes in logic to get rid of some redundant checks.

### Positive Aspects

- The added method is very short, and I was able to get the gist of what it was doing on my first read through. 
- The Update() method looks unchanged except for the new method call in Spritebatch.Draw() that changes the position of the camera, which is great. 

### Areas for Improvement

- Add a comment explaining what the 0.4f magic number is for, or a descriptive name for a variable its held in. 
- Add a comment for the IsMario() method explaining why we know the entity is mario if it has an InputConfig.
- The second check for _center == null may be unnecessary.
- In the else branch of PositinAdjustment(), declaring "Position position" on line 44 seems unnecessary. It may not be, but a comment may be helpful.