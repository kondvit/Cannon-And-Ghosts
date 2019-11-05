# Cannon And Ghosts

Implementation of:
  - Random 1D terrain generation using Perlin Noise (Stonehenge)
  - Collision detection from scratch (not using Unity's collider)
  - Verlet Integration with constraints (Floppy Ghosts)
  - Projectile physics (Cannon)

  <img src="sample.gif?raw=true"/>

***FrameWork:*** Unity

***Assets:*** All made using Gimp

***How to run:*** Open the project in Unity and press play.

### Scripts:

Assets/Cannon/Scripts/[ballCollider.cs](/Assets/Cannon/Scripts/BallCollider.cs)
  - ball collision handling

Assets/Cannon/Scripts/***CannonBallController.cs***
  - ball's projectile movement
  - initializes cannon ball's convex hall for collision detection

Assets/Cannon/Scripts/***CannonController.cs***
  - cannon barrel controller 
  - ball velocity can be adjusted here or from editor

Assets/Clouds/Scripts/***CloudController.cs***
  - cloud and wind controls

Assets/Ghost/Scripts/***GhostController.cs***
  - ghost controller
  - collision resolution with the ground/stonehenge for ghosts

Assets/Stonehenge/Scripts/***PerlinNoise.cs***
  - creates specified number of Perlin octaves (5 in this case)
  - computes total noise value from all octaves for given x

Assets/Stonehenge/Scripts/***PerlinOctave.cs***
  - creates a unique octave with specified amplitude and frequency
  - returns noise value for given x

Assets/Stonehenge/Scripts/***StoneRenderer.cs***
  - renders 4 sides of stonehenge
  - applies Perlin noise to all of them
  - aligns them with respect to the scale and position assigned from the Unity UI making the stone object generic and reusable
