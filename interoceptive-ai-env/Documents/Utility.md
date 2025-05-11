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