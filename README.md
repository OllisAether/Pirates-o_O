# Pirates o_O

A little surreal linear experience, where the player moves through a simple story loop.

You wake up in an unfamiliar place, discover you need a light source and comfort, and then follow the island's objective chain to reach a distant obelisk while navigating a desert, dialogue, and environmental interactions.

The end of the game might hold a surprise :>

Playtime: ~10 minutes

## Unity Version

- Unity 6.3 LTS. I have not tested Unity 6.4, but it should work fine. If you have issues, please try Unity 6.3 LTS.

## How to run

1. Open the project folder in Unity Hub.
2. Open the scene `Assets/Scenes/Main.unity`.
3. Press Play in the editor.

## Controls

### Keyboard + mouse:

  Default:
  - W / A / S / D: Move
  - Mouse: Look around
  - Left Shift: Sprint
  - Space: Jump
  - E: Interact with nearby objects
  - Mouse scroll: Cycle held item
  - Left mouse button: Use equipped item

  Dialog:
  - Space / Enter / Left mouse button: Continue dialog

## Mandatory requirement checklist and where each is implemented

### 1) Pathfinding

Used in Scene `Main` under `=== NPC ===`.

- Uses a `NavMeshAgent` to move an NPC/AI character toward a target.

### 2) Finite State Machine (FSM)

Implemented in `Assets/Scripts/fsm/StateMachine.cs` and `Assets/Scripts/fsm/State.cs`.
Used in Scene `Main` under `=== NPC ===`.

- The generic FSM is a simple state manager with `OnEnter`, `OnUpdate`, `OnExit`, and `RequestStateTransition`.
- It is used in `Assets/Scripts/ai/StrollBehaviour.cs` for AI patrol behavior.
- The `GoToWaypointState` transitions between waypoints in a loop or random order depending on the stroll mode.

### 3) Observer pattern

The project uses UnityEvents as an observer/event system.

Examples:

- `Assets/Scripts/ObjectiveSystem/ObjectiveManager.cs` exposes `OnObjectiveStarted`, `OnObjectiveCompleted`, `OnObjectiveStepStarted`, and `OnObjectiveStepCompleted` events.
- `Assets/Scripts/DialogSystem/DialogManager.cs` raises dialog events and listens for continue input through `OnContinueEvent`.
- `Assets/Scripts/Interactable/Interactable.cs` defines interaction events.

### 4) Performance-friendly object spawning / pooling

Implemented in `Assets/Scripts/ObjectPooling/Pool.cs`.

- A reusable pool pre-instantiates objects and reuses them instead of spawning and destroying many objects repeatedly.
- The pool expands automatically when needed, preserving performance during repeated activity.

### 5) Juice / feedback using Feel

Included in the project under `Assets/Feel`.
Used in Prefab `Assets/Items/Shark/Shark_Hold.prefab`

### 6) ScriptableObjects

ScriptableObjects are used for reusable content and game data.

Examples:

- `Assets/Scripts/ObjectiveSystem/Objective.cs` creates objective definitions as ScriptableObjects.
- `Assets/Scripts/InventorySystem/Item.cs` defines inventory/reusable item data as ScriptableObjects.
- Dialog trees also live in `Assets/Dialogs` as serializable data assets.

### 7) High-level gameplay system beyond an inventory system

The project includes multiple gameplay systems, with the main ones being:

- Objective system: `Assets/Scripts/ObjectiveSystem/`
- Dialog system: `Assets/Scripts/DialogSystem/`

### 8) Interaction system

Implemented in `Assets/Scripts/Interactable`

### 9) Cinematic camera + Timeline/Cinemachine

The project includes cutscene assets under `Assets/Cutscenes`:

- `Intro.playable`
- `Sleep.playable`
- `Obelisk.playable`
- `End.playable`
- `End Idle.playable`

The Unity packages in `Packages/manifest.json` include:

- `com.unity.cinemachine`
- `com.unity.timeline`

### 10) Visual quality beyond a default scene

The project uses Unity’s URP stack and includes visual enhancement assets such as `Assets/Better Fog`.
A custom made level with a desert island, obelisk, and NPCs is included in `Assets/Scenes/Main.unity`.

## Project structure overview

- `Assets/Scenes/` — playable scenes
- `Assets/Scripts/` — gameplay logic and systems
- `Assets/Dialogs/` — dialogue trees
- `Assets/Objectives/` — objective definitions
- `Assets/Items/` — item assets
- `Assets/Cutscenes/` — Timeline/Cinemachine sequences
- `Assets/Feel/` — feedback/juice package
- `Packages/manifest.json` — Unity package list

# ENJOY!