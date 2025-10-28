# Readability Review

**Author**: Stone

**Date**: 2025/10/05

**Sprint**: Sprint 2

**File(s) reviewed**: EnemyMovementSystem.cs

**Author of file(s)**: Logan

**Time spent reviewing**: 10 min

## Readability Notes

### Positive Aspects

- Reasonably avoids some magic numbers

### Areas for Improvement

- Give names to private fields according to CSharp convention

- It is scary to have "using System.Runtime.CompilerServices;" here. If the code does not use System.RuntimeCompilerServices, delete this line. If it does, write a comment to give a very very very specific reason for needing it.

- Use more arbitrary name for out parameters if the code doesn't use them.

- I see that the code uses detecting absence of component to vertify an entity. Refactor these vertifications into methods.
