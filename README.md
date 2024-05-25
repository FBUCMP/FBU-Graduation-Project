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

### Guns
![pistol](https://github.com/FBUCMP/FBU-Graduation-Project/assets/75688355/c22cc272-b756-454a-b9e1-f07fc53158a9)
![rifle](https://github.com/FBUCMP/FBU-Graduation-Project/assets/75688355/1e816283-cc16-434e-8b98-555f875b31bf)
![granadelauncher](https://github.com/FBUCMP/FBU-Graduation-Project/assets/75688355/adc09945-9713-4c60-aa5a-cb1f7f4dd739)
![shotgun](https://github.com/FBUCMP/FBU-Graduation-Project/assets/75688355/134393a7-82a3-4f17-8260-590d2b628fee)
![minigun](https://github.com/FBUCMP/FBU-Graduation-Project/assets/75688355/50527b9c-4278-4305-92ad-ee86f176de75)

### Random Generated Room with Marching Squares
![image](https://github.com/FBUCMP/FBU-Graduation-Project/assets/75688355/703cbb6b-94c7-45d2-8157-48ec6eab672d)

##


![gameplay0](https://github.com/FBUCMP/FBU-Graduation-Project/assets/75688355/b174f84b-9831-461f-84bf-9ecd42e66029)


![gameplay1](https://github.com/FBUCMP/FBU-Graduation-Project/assets/75688355/d033f264-0ff6-4614-9683-6e5c27d24f90)


![gameplay2](https://github.com/FBUCMP/FBU-Graduation-Project/assets/75688355/56d16787-cc0e-4b0f-9682-cab1d8105ca3)


![gameplay3](https://github.com/FBUCMP/FBU-Graduation-Project/assets/75688355/0f9a9ff1-88ff-44d0-9182-dd8c7728876a)


