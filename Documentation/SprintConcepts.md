# Rolling Desired Features List - text encased `in blocks` indicates it was selected for the current sprint


### Framework/Core Improvements
- System priority and execution phase
- Universal solution for readonly global data storage
- Deferred entity instantiation

### Full Level
- Only have entemies move when near mario
- Stop the camera and player from moving backwards
- Pipe interaction and sub-area loading
- Goal flag system
- Kill the player when they fall in a pit
- Level HUD
- Fix mario throwing fireballs

### Collision
- Fix issues with existing interactions
- Enemy + fireball (kill enemy, animation)
- Mario + dynamic blocks (bumping + breaking blocks on collision)
- Change mario's collision box size based on powerup state

### Utilities
- Debug mode
- Constants class to extract all magic numbers

### Other
- Player life system
- Level time system
- Sound effects and background music
- Main menu screen
- Game over screen
- Pausing

# Sprint 4 Focused Features 

### Backlogged From Previous Sprint
- Advanced Mario animations
- Finish JSON definition of 1-1
- Item + Mario interaction (change powerup state)
- Enemy + Mario interaction (damage Mario + animation)
- Velocity and acceleration vector visualizer
- Collision box visualizer
- Scale jump height by button pressed time

### Framework/Core Improvements
- Deferred entity instantiation

### Full Level
- Pipe interaction and sub-area loading
- Goal flag system
- Kill the player when they fall in a pit
- Level HUD
- Fix mario throwing fireballs
- Clean up mario physics
- Item entities and behavior
- Level background

### Collision
- Fix issues with existing interactions
- Enemy + fireball (kill enemy, animation)
- Mario + dynamic blocks (bumping + breaking blocks on collision)
- Change mario's collision box size based on powerup state

### Utilities
- Debug mode
- Constants class to extract all magic numbers
- Font loading

### Other
- Player life system
- Level time system
- Sound effects and background music
- Main menu screen
- Game over screen
- Pausing
- Acceleration-based user input (in addition to velocity)
- Reorganize filesystem

## Initial Assignments
- Ethan: Collision box visualizer, Font loading, Velocity and acceleration vector visualizer, Sound effects and background music, Finish JSON definition of 1-1
- Jiajie: Item entities and behavior, Goal flag system/flag loading, Level HUD
- Logan: Fix issues with existing collision interactions, Mario + dynamic blocks (bumping + breaking blocks on collision)
- Mian: Deferred entity instantiation, Acceleration-based user input (in addition to velocity), Pipe interaction and sub-area loading, Fix mario throwing fireballs
- Stone: Player life system, level time system

# Sprint 3 Focused Features

### Backlogged From Previous Sprint
- Advanced Mario animations

### Framework/Core Improvements
- Reflection-based entity instantiation
- More versatile input system

### Level Loading
- Load level data from JSON
    - JSON structure should ideally allow for additional data beyond just level contents
- Instantiate level contents from loaded data

### Full Level
- Complete JSON definition for 1-1
- Mobile camera (based on Mario's position and inputs)

### Collision
- Quadtree optimization
- Item + Mario interaction (change powerup state)
- Enemy + Mario interaction (damage Mario + animation)
- Mario jump on enemy interction (damage enemy, animation, spawn turtle shell)
- Mario and enemy + static blocks (prevent vertical and horizontal movement)

### Utilities
- Debug mode and level
- Velocity and acceleration vector visualizer
- Collision box visualizer

### Other
- String-based tag component
- Scale jump height by button pressed time

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