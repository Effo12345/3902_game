# Readability Review

**Author**: Jiajie

**Date**: 10/5/25

**Sprint**: 2

**File(s) reviewed**: GravitySystem.cs, AnimationSystem.cs

**Author of file(s)**: Ethan

**Time spent reviewing**: ~15m

## Readability Notes

### Positive Aspects

- Both files are structurally clear and easy to understand. 

### Areas for Improvement

- In the GravitySystem, the line "acceleration.Value += mass.Value * GravityAcceleration;" can be misleading. If it represents acceleration, gravity should be applied independent of mass: acceleration.Value += GravityAcceleration. If you want to update velocity, do: velocity.Value += GravityAcceleration * deltaTime; I do not understand the purpose of the mass component (are we trying to represent force?).