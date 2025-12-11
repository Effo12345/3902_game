# Rolling Desired Features List - text encased `in blocks` indicates it was selected for the current sprint


### Framework/Core Improvements
- System priority and execution phase
- Universal solution for readonly global data storage

### Full Level
- Only have entemies move when near mario
- Stop the camera and player from moving backwards
- `Underground area json definition`
- `Pipe animations, sideways pipes`
- `Invisible blocks`
- `Multi-coin blocks`
- `Better level transitions`

### Collision
- `Fix collision bugs (stop on ground, bump/break pipes and other static blocks)`
- Make collision more data-driven
- `Block breaking animation`

### Utilities
- Debug mode

### Other
- Player life system
- Main menu screen
- Game over screen
- Pausing
- `Sprint button`
- `Scale jump height by button press time`

### Sprint5 Ideas
- `Controller input`
- `FLUDD`
- Dive mechanic
- Galaxy

# Sprint 5 Focused Features

### Backlogged From Previous Sprint
- Advanced Mario animations
- Scale jump height by button pressed time
- Player life system
- Pipe animations
- Json definition for underground area

### Full Level
- Pipe animations, sideways pipes
- Invisible blocks
- Multi-coin blocks
- Better level transitions

### Collision
- Fix collision bugs (stop on ground, bump/break pipes and other static blocks)
- Block breaking animation

### Other
- Sprint button

### Sprint5 Ideas
- Controller input
- FLUDD

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
- Intro Animation
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
- Constants class to extract all magic numbers
- Font loading

### Other
- Player life system
- Level time system
- Sound effects and background music
- Acceleration-based user input (in addition to velocity)
- Reorganize filesystem

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