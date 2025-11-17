# Readability Review

**Author**: Ethan

**Date**: 11-16-25

**Sprint**: 4

**File(s) reviewed**: InputSystem.cs

**Author of file(s)**: Mian

**Time spent reviewing**: 7

## Readability Notes

### Positive Aspects

- The comments give clear guidance for exactly what is happening each step of the update method
- Breaking some commonly-used pieces of code into separate methods greatly reduces bloat and improves visualization of the execution path
- Relatively low levels of indentation keep things fairly clear
- Styling and use of whitespace are clear and consistent throughout the file
- Private member variables are obviously marked
- Longer logical sections being broken across multiple lines prevents exceedingly long lines

### Areas for Improvement

- Even with the above-mentioned helper methods, Update is still a bit too long to be easily glanceable
- Some of the methods have lots of parameters, making their call signatures hard to read