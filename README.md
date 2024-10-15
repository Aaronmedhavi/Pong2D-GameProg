## About
Pong Star is a 2D space-themed take of the classic Pong game. In this game, players control ships to hit a ball back and forth in a space environment filled with planets and stars. 

## Installation
1. Download the game here:
   ```
   https://aaronmedhavi.itch.io/pong-star
   ```
2. Extract the files from the zip folder.
3. Find and click on Pong2DV2.exe
4. Enjoy the game.

## Installation(Open in Unity Editor)
1. Clone the repository:
   ```
   git clone https://github.com/Aaronmedhavi/Pong2D-GameProg.git
   ```
2. Open the project in Unity (version 2022.3.9f1 or later).
3. Open the "MainMenu" scene located in the "Assets/Scenes" folder.
4. Press the Play button in Unity Editor to start the game.

## How to Play
  - Move Up: W
  - Move Down: S

## Features and Script
- Single-player mode against an AI opponent
- Space-themed graphics and animations
- Retro-style sound effects
- Online multiplayer for two players

|  Script       | Description                                                  |
| ------------------- | ------------------------------------------------------------ |
| `NetworkUI.cs` | Manages the UI elements related to multiplayer. |
| `BallSpawner.cs` | Spawn the ball into the game when the host and client joined. |
| `GameManager.cs`  | Manages the score, UI elements, and special effects. |
| `BallControl.cs`  | Handle the ball movement and syncing them across client and host. |
| `etc`  | |
