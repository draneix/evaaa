# Utility Scripts Overview

The Utility scripts in EVAAA provide general-purpose functions, data structures, and editor tools that support core simulation, data collection, and environment setup. These scripts are essential for experiment reproducibility, configuration, and efficient development workflows.

## Main Components

### DataRecorder.cs
- **Purpose:** Records and exports detailed experiment data at both step and episode levels.
- **Key Features:**
  - Tracks agent state, actions, rewards, collisions, and events.
  - Exports data to CSV files for analysis and reproducibility.
  - Supports flexible configuration and output directory management.

### Utility.cs
- **Purpose:** Provides reusable data structures and helper functions for agent and environment configuration.
- **Key Features:**
  - Defines serializable vector, color, range, and coefficient classes.
  - Includes spatial overlap checking utilities for environment setup.

### ObstacleCollector.cs
- **Purpose:** Automates the collection and export of obstacle configurations from the Unity scene.
- **Key Features:**
  - Gathers position, rotation, and scale data for obstacles.
  - Exports configuration as JSON for reproducible environment generation.
  - Supports filtering and formatting options.

### Editor/ObstacleCollectorEditor.cs
- **Purpose:** Provides a Unity Editor extension for obstacle collection.
- **Key Features:**
  - Adds a custom inspector button to trigger obstacle collection from the Unity Editor.

---

These utility scripts streamline data management, configuration, and environment setup, supporting robust experimentation and efficient development in EVAAA.

# Utility.cs

## Purpose
The `Utility` module provides general-purpose data structures and helper functions to support simulation, configuration, and environment setup in EVAAA. It includes serializable classes for configuration, color, ranges, and a static utility for overlap checking.

## Key Features
- Serializable data structures for 3D vectors, colors, value ranges, coefficients, and spatial ranges.
- Static utility method for checking object overlap in the environment.
- Supports configuration, procedural generation, and spatial reasoning.
- Designed for easy integration with Unity Inspector and JSON configuration files.

## Main Data Structures
- `ThreeDVector`: Serializable 3D vector with conversion to `Vector3`.
- `ColorVector`: Serializable color with conversion to `Color`.
- `EVRange`: Range (min, max) for essential variables.
- `Coefficient`: Stores multiple change coefficients for parameterized updates.
- `PositionRange`, `RotationRange`, `ScaleRange`: Ranges for spatial configuration.

## Utility Methods
- `OverlapUtility.IsOverlapping(position, prefab, scale, boxSizeMultiplier, padding, execName)`: Checks if placing an object at a given position would overlap with existing objects, supporting procedural placement and collision avoidance.

## Integration Notes
- The utility classes are used throughout the project for configuration, agent/environment setup, and procedural generation.
- Designed to be imported and used by other scripts (e.g., spawners, config loaders, scene controllers).
- All data structures are serializable for use with Unity Inspector and JSON.

## Example Usage
- Use `ThreeDVector` and `ColorVector` in configuration files and convert to Unity types at runtime.
- Use `OverlapUtility.IsOverlapping()` to check for valid placement of objects during procedural generation.

---

For more details on data structures and advanced usage, see the code comments in `Assets/Scripts/Utility/Utility.cs`. 