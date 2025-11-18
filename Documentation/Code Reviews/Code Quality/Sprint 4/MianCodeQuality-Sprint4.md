# Code Quality Review

**Author**: Mian

**Date**: 11/16/2025

**Sprint**: 4

**File(s) reviewed**: CollisionVisualizationSystem.cs; SoundPlayer.cs

**Author of file(s)**: Ethan

**Time spent reviewing**: 20

## Code Smell Analysis

### 1. Duplicated Code

- SoundPlayer repeats the pattern ¡°iterate all keys ¡ú call single-item method (Pause/Stop/Resume)¡± in PauseAll, StopAll, and ResumeAll. This invites copy-paste errors and also hides a deeper bug (mutating the dictionary while iterating; see Shotgun Surgery).

CollisionVisualizationSystem.Update constructs four border lines every frame with similar tuple literals; consider a small helper to generate edges once or a reusable buffer.

### 2. Long Method

- Methods are short in both files. Potential future risk: CollisionVisualizationSystem.Update will grow if more debug overlays are added; split into helpers like DrawColliderOutline(Entity, Collider) and GetColliderEdges(Rectangle)

### 3. Large Class

- N/A

### 4. Long Parameter List

- N/A

### 5. Cyclomatic Complexity

- Low in both files. The only branching hotspot is CollisionVisualizationSystem.Update¡¯s per-entity loop and SoundPlayer¡¯s all-operations; both are straightforward. Keep it flat by extracting helpers before adding more conditions.

### 6. Divergent Change

- N/A

### 7. Shotgun Surgery

- SoundPlayer.StopAll() iterates PlayingSounds.Keys and calls Stop(key), which removes from the same dictionary¡ªthis can invalidate the enumerator and throw. The same pattern appears in PauseAll/ResumeAll (those don¡¯t remove, but the pattern encourages copy-paste). Collect keys via ToList() first.

### 8. Feature Envy

- N/A

### 9. Data Clumps

- SoundPlayer couples ¡°lookup ¡ú volume ¡ú loop ¡ú play¡± as a recurring cluster; extract a private StartInstance(key, volume, looped) helper to reduce repetition and standardize behavior.
- CollisionVisualizationSystem bundles (Rectangle edge, Direction) pairs repeatedly; returning a small struct Edge { Rectangle Rect; Direction Dir; } from a generator would clarify intent.

### 10. Switch Statements

- N/A

### 11. Lazy Class/ Middle Man

- N/A

### 12. Message Chains

- Message chains in the class are intentional with the ECS system.

### 13. Deoderizing Comments

- Good TODO in CollisionVisualizationSystem (¡°only in debug mode¡±), but it¡¯s not enforced. Use #if DEBUG or a runtime debug flag to align code with comment

<!-- Code smells presentation: https://osu.instructure.com/courses/190003/files/78535068?module_item_id=15761957 -->
<!-- These smells included for convenience. Feel free to add/remove as needed or put N/A when not present -->

## Future Improvement

**Proposed Change**:

- For rendering: reuse allocations and make debug drawing optional¡ªinitialize LatestCameraTransform = Matrix.Identity, reuse the collisions map/sets, precompute edge rectangles, and guard drawing with a debug flag.
- For audio: make operations safe and testable¡ªseparate SFX vs. music channels, guard all dictionary access, avoid mutating collections during iteration, and dispose instances on stop.

## Conclusion

Both files are concise and mostly clear. The biggest wins are safety and allocation hygiene: in visualization, reuse data structures and gate debug rendering; in audio, avoid modifying collections during iteration, guard all lookups, and dispose stopped instances. These changes improve correctness and performance without changing any external behavior.