# Readability Review

**Author**: Mian

**Date**: 2025-10-29

**Sprint**: 3

**File(s) reviewed**: StartScreenSystem.cs

**Author of file(s)**: Jiajie

**Time spent reviewing**: 15 minutes

## Readability Notes

The file is small and readable, with a straightforward input-driven loop. Most readability wins come from **making implicit intent explicit** (constants, named options), **removing tiny inconsistencies** (formatting), and **extracting micro-helpers** for input edges and selection handling. These changes reduce mental overhead without altering behavior, making the screen logic easier to scan and safer to extend. 

## Positive Aspects

* **Clear control flow**
The `Update` loop is easy to follow (guard on `_done`, read input, handle Up/Down/Space, update `_prev`).

* **Good early-return usage**
`if (_done) return;` avoids unnecessary work and keeps the frame logic readable. 

* **Minimal state surface**
Only a handful of fields are tracked (`_selected`, `_cursorOffset`, `_prev`, `_done`), which keeps cognitive load low. 

## Areas for Improvement

* **Magic numbers**    
`_cursorOffset = 30` is a literal with no context; it would read better as a named constant (e.g., `const int CursorOffset = 30;`). 

* **Hard-coded resource path**
`"Content/Config/Level/TestLevel.json"` is embedded in the input handler, obscuring intent and making changes harder to discover; a named constant or config would help. 

* **Inconsistent formatting**
Minor spacing inconsistency (`if(kb.IsKeyDown(Keys.Space)` vs `if (kb.IsKeyDown(Keys.Up)`) hinders scanability. A formatter rule would remove this noise. 

* **Implicit meanings**
`_selected == 0` implies ¡°Start game,¡± else ¡°Exit,¡± but that knowledge is not obvious in code. Naming the options (enum) or mapping to actions would make the intent explicit. 

* **Mixed responsibilities in `Update`**
Input edge detection, selection updates, cursor movement, and action dispatch all live together. Extracting small helpers would make the loop more declarative. 

* **Field mutability semantics**
`_cursorOffset` never changes after initialization¡ª`const`/`readonly` would communicate intent and prevent accidental mutation. 

* **Member ordering**
Placing the constructor before overrides (`Initialize`, `Update`) is a common convention that aids discoverability when scanning the file top-down. 

* **Comment/doc hints**
Brief XML docs for the system (what it renders/handles; exit conditions) would help future readers, especially around `_done` lifecycle and the option semantics. 