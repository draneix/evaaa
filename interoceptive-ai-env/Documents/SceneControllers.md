# Scene Controllers Overview

The Scene Controller scripts in EVAAA are responsible for initializing, configuring, and managing the simulation environment. They coordinate the setup of agents, environment components, and supporting systems to ensure reproducible and flexible experiments.

## Main Components

### MasterInitializer.cs
- **Purpose:** Orchestrates the initialization of the entire simulation scene.
- **Key Features:**
  - Pauses and resumes the ML-Agents Academy during setup.
  - Loads configuration files and initializes all major systems (spawners, NavMesh, thermal grid, agent, heatmap, day/night cycle, camera, event system, data recording, and screenshot capture).
  - Ensures all dependencies are ready before starting the simulation.

### ConfigLoader.cs
- **Purpose:** Handles loading and management of configuration files for the experiment.
- **Key Features:**
  - Loads main and sub-configuration files (JSON) for agents, environment, and experiments.
  - Provides access to configuration data for other systems.
  - Supports platform-specific paths for cross-platform compatibility.

### CaptureScreenShot.cs
- **Purpose:** Manages automated screenshot capture during simulation runs.
- **Key Features:**
  - Configures output directories and recording options from configuration files.
  - Captures and saves screenshots at specified intervals or events.
  - Supports toggling recording on/off via configuration.

---

These scripts ensure that each experiment is initialized in a controlled, reproducible manner, supporting robust research and easy customization in EVAAA. 