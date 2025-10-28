# Readability Review

**Author**: Ethan

**Date**: 10-27-25

**Sprint**: 3

**File(s) reviewed**: InputSystem.cs

**Author of file(s)**: Mian

**Time spent reviewing**: 9

## Readability Notes

### Positive Aspects

- Variables are given clear names throughout. In particular, private member variables are obviously marked.
- The comments provide a consistent step-wise process for what each section is doing

### Areas for Improvement

- Update is a very large function that can be difficult to follow. Consider splitting it out into multiple to handle different kinds of input
- It can be hard to find the publishers in some places. This can make it more difficult to spot the data flow to other systems