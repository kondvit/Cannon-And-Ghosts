# Cannon And Ghosts

Implementation of:
  - Random 1D terrain generation using Perlin Noise (Stonehenge)
  - Collision detection from scratch (not using Unity's collider)
  - Verlet Integration with constraints (Floppy Ghosts)
  - Projectile physics (Cannon)

  <img src="sample.gif?raw=true"/>

FrameWork: Unity
Assets: All made in Gimp

How to run: Open the project in Unity and press play.

### Scripts:

Assets/Cannon/Scripts/***ballCollider.cs***
  - Ball collision handling

Assets/Cannon/Scripts/***CannonBallController.cs***
  - ball's movement
  - initializes cannon ball's convex hall

Assets/Cannon/Scripts/***CannonController.cs***
  - Barrel controls 
  - ball velocity can be adjusted here or from editor

Assets/Clouds/Scripts/***CloudController.cs***
  - Cloud and Wind controls

Assets/Ghost/Scripts/***GhostController.cs***
  - Ghost controls
  - Collision resolution with the ground/stonehenge

Assets/Stonehenge/Scripts/***PerlinNoise.cs***
  - creates specified number of Perlin octaves (5)
  - computes total noise value from all octaves for given x

Assets/Stonehenge/Scripts/***PerlinOctave.cs***
  - creates a unique octave with specified amplitude and frequency
  - returns noise value for given x

Assets/Stonehenge/Scripts/***StoneRenderer.cs***
  - renders 4 sides
  - applies Perlin noise to all of them
  - aligns them with respect to the scale and position assigned from the Unity UI making the stone object generic and reusable
