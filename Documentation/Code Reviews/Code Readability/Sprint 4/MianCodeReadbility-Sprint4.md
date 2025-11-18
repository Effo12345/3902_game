# Readability Review

**Author**: Mian 

**Date**: 11/16/2025

**Sprint**: 4

**File(s) reviewed**: ItemRiseSystem.cs; ItemBlockConsumptionSystem.cs

**Author of file(s)**: Jiajie

**Time spent reviewing**: 10

## Readability Notes

### Positive Aspects

- Clear control flow: The Update loop is easy to follow (guard on _done, read input, handle Up/Down/Space, update _prev).
- Good early-return usage: if (_done) return; avoids unnecessary work and keeps the frame logic readable.
- Minimal state surface: Only a handful of fields are tracked (_selected, _cursorOffset, _prev, _done), which keeps cognitive load low.

### Areas for Improvement

- Magic numbers: _cursorOffset = 30 is a literal with no context; it would read better as a named constant (e.g., const int CursorOffset = 30;).
- Hard-coded resource path: "Content/Config/Level/TestLevel.json" is embedded in the input handler, obscuring intent and making changes harder to discover; a named constant or config would help.
- Inconsistent formatting: Minor spacing inconsistency (if(kb.IsKeyDown(Keys.Space) vs if (kb.IsKeyDown(Keys.Up)) hinders scanability. A formatter rule would remove this noise.