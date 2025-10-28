# Readability Review

**Author**: Stone Jiang

**Date**: 2025/10/27

**Sprint**: Sprint 3

**File(s) reviewed**: EntityFactory.cs, EntityLoader.cs

**Author of file(s)**: Ethan

**Time spent reviewing**: 25 min

## Readability Notes

### Positive Aspects

- Class name and methods clearly convey intent.
- Private helpers keep things concise.
- Reasonably placed comments.

### Areas for Improvement

- Long chains like <code>
AppDomain.CurrentDomain.GetAssemblies().SelectMany().Where().ToDictionary()
</code> can be confusing.
