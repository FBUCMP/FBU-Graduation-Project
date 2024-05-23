# FBU-Graduation-Project

# 2D Platformer Shooter Roguelike Game



## Features
* Procedurally Generated Levels
* Random Room Creation: Each game session starts with a randomly generated room within a defined boundary.
* Room Arrangement: Rooms are stored in a 2D array and connected via passageways, allowing seamless transition from one room to another.
* Dynamic Walls: Using the Marching Squares algorithm, we create cave-like wall meshes. These walls are generated with Perlin Noise for a natural look.
* Destructible Environment: Players can destroy walls by shooting them, which regenerates the wall mesh dynamically.
  
## Enemies
* Spider:
  * Navigates the environment using the A* project's Seeker component.
  * Capable of climbing walls and jumping.
  * Explodes upon close contact with the player.
* Flying Bug:
  * Uses A* project's AIPath2D component to follow the player.
  * Explodes upon close contact with the player.

## Boss Room
* Boss Encounter: Features a giant fly boss with multiple attack patterns using coroutines:
  * Dash Attack: Quickly dashes towards the player.
  * Spit Attacks: Shoots projectiles at the player.
  * Mini-Fly Rain: Summons mini flies that rain down on the player.

## Player Systems
* Movement: Some movement skills like jetpack and dash in addition to the classic 2D platformer mechanics including jumping and shooting.
* Weapon System:
  * Scriptable Objects: Each weapon is a scriptable object, making it easy to add new weapons.
  * Configurable Components: Separate scriptable objects for sound, damage, shooting mechanics, trails, and knockback.
  * Available Weapons: Pistol, rifle, grenade launcher, shotgun, and minigun.
* Skill and Upgrade System: Players can acquire new skills and upgrade items to enhance their abilities.

## Surface Manager
* Layer and Impact Pairing: Pairs Unity object layers with custom impact types.
  * Example: Bullet impacts ground layer triggering specific sound and particle effects.
  * Example: Footsteps trigger step sounds on different surfaces.

## Screenshots