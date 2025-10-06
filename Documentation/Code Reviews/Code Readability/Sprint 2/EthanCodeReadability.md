# Readability Review

**Author**: Ethan

**Date**: 10/05/25

**Sprint**: 2

**File(s) reviewed**: MarioActionSystem.cs

**Author of file(s)**: Mian

**Time spent reviewing**: 13 minutes

## Readability Notes

### Positive Aspects

- Taking in well-named parameters in the constructor helps understand the data flow and how internal variables should be used.
- Variables in general have good, descriptive names
- The logic is broadly simple enough to be understood at a glance (though there are some comments on this; see below)

### Areas for Improvement

- Directly integrating keyboard controls into this system fills update with a lot of if statements. Understandable given the current state of the input system, but make it difficult to track what's going on
- Avoid mixing indented and bracketed if statements, like after `else if (key == Keys.L)`. It makes it difficult to see what will be executing on different branches
- Try and keep naming conventions consistent where possible; `_previousKeys` is the only private variable to follow the underscore prefixing convention
- Consider inverting the large outer if statement, `if (!_previousKeys.Contains(key))`, and make it execute a `continue`. Both implementations do the same thing, but using `continue` removes a level of indentation from the loop