# Scene Controllers Overview

The Scene Controller scripts in EVAAA are responsible for initializing, configuring, and managing the simulation environment. They coordinate the setup of agents, environment components, and supporting systems to ensure reproducible and flexible experiments.

Below are the main components of the Scene Controller system. Each component is documented in detail in its own markdown file:

- [MasterInitializer](./MasterInitializer.md): Orchestrates the initialization of the entire simulation scene, including configuration, spawners, NavMesh, agent, heatmap, day/night cycle, camera, event system, data recording, and screenshot capture.
- [ConfigLoader](./ConfigLoader.md): Loads and manages experiment configuration from JSON files, enabling flexible switching between environments and tasks.
- [CaptureScreenShot](./CaptureScreenShot.md): Handles automated screenshot capture and recording during simulation runs, based on configuration settings.

For details on each component, see the corresponding markdown file linked above. 