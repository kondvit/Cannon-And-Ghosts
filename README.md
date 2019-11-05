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

Assets/Cannon/Scripts/[BallCollider.cs](/Assets/Cannon/Scripts/BallCollider.cs)
  - ball collision handling

[CannonBallController.cs](/Assets/Cannon/Scripts)
  - ball's projectile movement
  - initializes cannon ball's convex hall for collision detection

[CannonController.cs](/Assets/Cannon/Scripts)
  - cannon barrel controller 
  - ball velocity can be adjusted here or from editor

[CloudController.cs](/Assets/Clouds/Scripts)
  - cloud and wind controls

[GhostController.cs](/Assets/Ghost/Scripts)
  - ghost controller
  - collision resolution with the ground/stonehenge for ghosts

[PerlinNoise.cs](/Assets/Stonehenge/Scripts)
  - creates specified number of Perlin octaves (5 in this case)
  - computes total noise value from all octaves for given x

[PerlinOctave.cs](/Assets/Stonehenge/Scripts)
  - creates a unique octave with specified amplitude and frequency
  - returns noise value for given x

[StoneRenderer.cs](/Assets/Stonehenge/Scripts)
  - renders 4 sides of stonehenge
  - applies Perlin noise to all of them
  - aligns them with respect to the scale and position assigned from the Unity UI making the stone object generic and reusable
