# MasterInitializer.cs

## Purpose
The `MasterInitializer` class orchestrates the initialization of the entire simulation scene in EVAAA. It ensures that all core systems—configuration, spawners, navigation, agent, heatmap, day/night cycle, camera, event system, data recording, and screenshot capture—are set up in the correct order for reproducible experiments.

## Key Features
- Pauses and resumes the ML-Agents Academy during setup.
- Loads and applies experiment configuration via `ConfigLoader`.
- Initializes all major systems: spawners, NavMesh, thermal grid, agent, heatmap, day/night cycle, camera, event system, data recording, and screenshot capture.
- Dynamically bakes the NavMesh for agent and predator navigation.
- Ensures all dependencies are ready before starting the simulation.
- Provides runtime initialization and reset logic for robust experimentation.

## Configuration Options
- References to all major system components (assigned via Unity Inspector):
  - `ConfigLoader`, `SpawnerManager`, `NavMeshSurface`, `ThermoGridSpawner`, `InteroceptiveAgent`, `HeatMap`, `DayAndNight`, `AgentFollowCamera`, etc.
- No direct JSON configuration, but relies on `ConfigLoader` for experiment settings.

## Main Methods
- `Start()`: Entry point; calls `InitializeScene()`.
- `InitializeScene()`: Main initialization sequence for all systems.
- `InitializeSystems()`: Initializes data recording, event system, and screenshot capture.
- `ResetSceneInOrder()`: (Coroutine) Resets the scene in a controlled order (see code for details).

## Integration Notes
- Designed to be attached to a central GameObject in the Unity scene.
- All referenced components must be assigned in the Unity Inspector.
- Works in conjunction with all major systems for a fully integrated experiment setup.
- Initialization order is critical for reproducibility and correct system behavior.

## Example Usage
- Attach the `MasterInitializer` script to a GameObject in the Unity scene.
- Assign all required component references in the Inspector.
- The script will automatically initialize the scene and all systems when the simulation starts.

---

For more details on advanced usage and initialization order, see the code comments in `Assets/Scripts/SceneController/MasterInitializer.cs`. 