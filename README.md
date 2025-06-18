
---
# Unity Game Dev Learning Project

This is project is about building a **2D top down tile-based RTS game** using Unity and C#. 
This is a beginner-friendly collaborative project focused on exploring **Unity** and applying **Data Structures & Algorithms (DSA)** in real-world game systems like pathfinding, grid logic, and player movement logic used in 2d top down games.
The main focus of this project is not on using prebuilt Unity libraries, but on implementing the core logic and understanding the algorithms behind modern game logic.

---

## Collaborate and Join to learn more and Improve ?

This is **perfect for students, developers, and enthusiasts** who want to learn DSA practically and explore Unity game dev at the same time.
Anyone can join! Whether you're:
- A complete beginner in Unity
- Someone who loves solving DSA problems
- Curious about how algorithms work in actual games

This project is a **safe learning space** — collaboration over perfection!

---

## Tech Stack

- Unity (2022+)
- C#
- Unity Tilemap system
- Unity Custom editor tools and setup
- Git & GitHub

---

## How to Get Started

1. **Clone the Repo**
   ```bash
   git clone https://github.com/Alla-Avinash/RTS-Game-Dev.git
   ```

2. **Open in Unity**

Open Unity Hub
Click Add, then navigate to the project folder
Open the main scene: Assets/_GAME_/Scenes/Hometown


---

## Current Progress

Here’s what has been implemented so far:

- Implemented a **custom Tilemap palette** using the **Tiny Swords** asset pack for creating custom level layouts
- **Multi-floor connections** and using custom user placed graph nodes for heuristic based path finding
- **Custom Editor GUI** to add nodes and draw connections between them
- **Pathfinding algorithms** (A* and Dijkstra's Algorithm)
- **Grid position snapping** and basic mouse interaction
- Created a **player character** and implemented his movement logic to follow the path finding algorithm

---

## Future Goals & Ideas

This project is just the beginning! With collaboration and group learning, we plan to:

- Make a small **AI/player/enemies** follow graph paths
- Build interactive level puzzles or test environments
- Add multiplayer functionality

---

## Project Folder Structure

This is how the project is organized for easy contribution:

unity-graph-game/
├── Assets/
│ ├── GAME/ # All game logic and structure
│ │ ├── Editor/ # Custom editor tools (like LevelGraphEditor.cs)
│ │ ├── Levels/ # ScriptableObject assets or graph data
│ │ ├── Scripts/ # Main runtime logic (MapManager.cs, etc.)
│ │ ├── Prefabs
│ │ ├── Scenes/ # Game scenes
│ │ └── Tilemaps/ # Grid/Tilemap assets
│ ├── Resources/ # Runtime-loadable assets
│ └── Materials/
├── ProjectSettings/
├── Packages/
├── README.md
└── .gitignore


---

