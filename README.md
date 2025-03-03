# Indiana Jones Style 2D Adventure Game

A short 2D platformer game inspired by Indiana Jones, built with Unity and C#.

## Game Features

- **Player Character**: Indiana Jones-style adventurer with running, jumping, and climbing abilities
- **Enemy AI**: Smart enemies that patrol, chase, and attack the player
- **Treasure Collection**: Collect all treasures to complete the level
- **Traps and Hazards**: Spike traps and other obstacles
- **Climbing System**: Ladders for vertical movement
- **Camera Follow**: Smooth camera that follows the player
- **UI System**: Complete game UI with lives, treasure count, and game over screens

## Setup Instructions

### 1. Unity Project Setup
1. Open Unity Hub
2. Create a new 2D project
3. Copy all scripts from `Assets/Scripts/` to your Unity project's `Assets/Scripts/` folder
4. Set up the following layers in Unity:
   - Player
   - Enemy
   - Ground
   - Treasure
   - Ladder
   - Trap

### 2. Scene Setup

#### Player Setup
1. Create an empty GameObject and name it "Player"
2. Add the following components:
   - SpriteRenderer (with your Indiana Jones sprite)
   - Rigidbody2D (Gravity Scale: 1)
   - Collider2D (set as trigger for treasure/enemy detection)
   - PlayerController script
   - AudioSource
3. Tag the player as "Player"
4. Set the layer to "Player"

#### Ground Setup
1. Create ground platforms using sprites or primitive shapes
2. Add Collider2D components
3. Set layer to "Ground"
4. Create a GroundCheck child object under the player

#### Enemies Setup
1. Create enemy GameObjects with:
   - SpriteRenderer
   - Rigidbody2D
   - Collider2D
   - EnemyAI script
   - AudioSource
2. Tag as "Enemy"
3. Set layer to "Enemy"
4. Create patrol points as child objects

#### Treasures Setup
1. Create treasure GameObjects with:
   - SpriteRenderer (treasure sprite)
   - Collider2D (set as trigger)
   - Treasure script
   - AudioSource
2. Tag as "Treasure"
3. Set layer to "Treasure"

#### Ladders Setup
1. Create ladder GameObjects with:
   - Collider2D (set as trigger)
   - Ladder script
2. Tag as "Ladder"

#### Traps Setup
1. Create spike trap GameObjects with:
   - SpriteRenderer
   - Collider2D (set as trigger)
   - SpikeTrap script
   - AudioSource
2. Tag as "Trap"

### 3. Camera Setup
1. Select the Main Camera
2. Add the CameraFollow script
3. Set the target to the Player
4. Configure bounds for your level

### 4. UI Setup
1. Create a Canvas
2. Add UI elements:
   - Text for treasure count
   - Text for lives
   - Game Over panel
   - Pause menu
3. Add UIManager script to a GameObject
4. Connect all UI references in the inspector

### 5. Game Manager Setup
1. Create an empty GameObject named "GameManager"
2. Add the GameManager script
3. Set total treasures and player lives
4. Connect UI references

## Controls

- **Arrow Keys / WASD**: Move left/right
- **Space**: Jump
- **W/S**: Climb ladders (when touching them)
- **Escape**: Pause/Resume game

## Game Objective

Collect all treasures while avoiding enemies and traps. Use ladders to reach higher platforms and explore the level. Complete the level by collecting all treasures without losing all your lives.

## Scripts Overview

- **PlayerController.cs**: Handles player movement, jumping, climbing, and collision detection
- **GameManager.cs**: Manages game state, scoring, and level completion
- **EnemyAI.cs**: Controls enemy behavior (patrol, chase, attack)
- **Treasure.cs**: Manages treasure collection and effects
- **CameraFollow.cs**: Smooth camera following with bounds
- **Ladder.cs**: Enables climbing mechanics
- **SpikeTrap.cs**: Trap activation and damage system
- **UIManager.cs**: Handles all UI interactions and menus
- **CollectibleEffect.cs**: Visual effects for collectibles

## Customization

- Adjust movement speeds in PlayerController
- Modify enemy behavior in EnemyAI
- Change treasure values and effects in Treasure script
- Customize camera bounds in CameraFollow
- Add more trap types by extending the trap system

## Audio Setup

Add audio clips to the respective components:
- Player: jump and collect sounds
- Enemies: attack and death sounds
- Treasures: collect sounds
- Traps: activation and damage sounds

Enjoy your Indiana Jones adventure!
