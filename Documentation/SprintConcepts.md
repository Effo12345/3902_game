# Sprint 3

## Desired Features

### Backlogged From Previous Sprint
- Advanced Mario animations

### Framework/Core Improvements
- Reflection-based entity instantiation
- More versatile input system
- System priority and execution phase
- Universal solution for readonly global data storage

### Level Loading
- Load level data from JSON
    - JSON structure should ideally allow for additional data beyond just level contents
- Instantiate level contents from loaded data

### Full Level
- Complete JSON definition for 1-1
- Mobile camera (based on Mario's position and inputs)
- Pipe interaction and sub-area loading
- Goal flag system

### Collision
- Quadtree optimization
- Item + Mario interaction (change powerup state)
- Enemy + Mario interaction (damage Mario + animation)
- Mario jump on enemy interction (damage enemy, animation, spawn turtle shell)
- Enemy + fireball (kill enemy, animation)
- Mario and enemy + static blocks (prevent vertical and horizontal movement)
- Mario + dynamic blocks (bumping + breaking blocks on collision)

### Utilities
- Debug mode and level
- Velocity and acceleration vector visualizer
- Collision box visualizer

### Other
- Player life system
- Level time system
- String-based tag component
- Scale jump height by button pressed time

## Starting Assignments
- Logan: Collision detection and publishing
- Mian: Input system, collison box and vector visualization
- Ethan: Reflection-based entity loading
- Stone: Camera rendering
- Jiajie: Level loading

# Sprint 2

## Desired Features
- ECS framework
- Basic Mario motion
- Basic Mario animation
- Advanced Mario animation
- Enemy animation
- Enemy movement
- Enemy switching with key press
- Block rendering
- Block interaction
- Item animation
- Item switching with key press