# EVAAA: Essential Variables in Autonomous and Adaptive Agents

## Table of Contents
- [EVAAA: Essential Variables in Autonomous and Adaptive Agents](#evaaa-essential-variables-in-autonomous-and-adaptive-agents)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Key Features](#key-features)
  - [Quickstart Example](#quickstart-example)
  - [Installation](#installation)
    - [1. Unity Editor](#1-unity-editor)
    - [2. Python \& ML-Agents (Optional, for training)](#2-python--ml-agents-optional-for-training)
    - [3. Additional Steps](#3-additional-steps)
  - [Project Structure](#project-structure)
    - [1. Agent Scripts](#1-agent-scripts)
    - [2. Environment Scripts](#2-environment-scripts)
    - [3. Event System](#3-event-system)
    - [4. Scene Controllers](#4-scene-controllers)
    - [5. User Interface (UI)](#5-user-interface-ui)
    - [6. Utility Scripts](#6-utility-scripts)
  - [Usage](#usage)
    - [Configuration System Overview](#configuration-system-overview)
      - [1. Config Directory Structure](#1-config-directory-structure)
      - [2. How Configuration is Loaded](#2-how-configuration-is-loaded)
      - [3. Example: mainConfig.json](#3-example-mainconfigjson)
    - [How to Use](#how-to-use)
  - [Environments \& Tasks](#environments--tasks)
    - [1. Naturalistic Training Curriculum](#1-naturalistic-training-curriculum)
    - [2. Experimental Testbeds](#2-experimental-testbeds)
      - [Basic Homeostatic Regulation](#basic-homeostatic-regulation)
      - [Advanced Adaptive Skills](#advanced-adaptive-skills)
  - [Baselines \& Evaluation](#baselines--evaluation)
  - [Customization](#customization)
  - [Citing EVAAA](#citing-evaaa)
  - [License](#license)
  - [Contact](#contact)
  - [Acknowledgements](#acknowledgements)

---

## Overview
<!-- Briefly introduce EVAAA, its motivation, and its significance for RL research (1-2 paragraphs). -->

## Key Features
- Biologically inspired agent design with internal state regulation
- Egocentric, multimodal perception (vision, olfaction, thermoception, etc.)
- Progressive curriculum and experimental testbeds
- Unified, intrinsic reward system
- Built on Unity ML-Agents
- Open-source and fully configurable

## Quickstart Example

Follow these steps to launch your first EVAAA simulation:

1. **Clone the repository:**
   ```bash
   git clone https://anonymous.4open.science/r/evaaa-2486
   cd evaaa-2486
   ```

2. **Open in Unity:**
   - Launch Unity Hub.
   - Click **Add** and select the cloned `evaaa-2486` folder.
   - Open the project with **Unity Editor 2022.3.16f1**.

3. **Select an environment/task:**
   - Edit `Config/mainConfig.json` and set `"configFolderName"` to your desired experiment (e.g., `"train-level-1.1-ScatteredResource"`).

4. **Run the simulation:**
   - Press the **Play** button in the Unity Editor.
   - Observe the agent interacting with the environment.

---

You can now explore, modify, or extend the EVAAA environments and tasks.

## Installation

### 1. Unity Editor

- **Required Version:** EVAAA is developed and tested with **Unity Editor 2022.3.16f1**.
- **Download Unity:**
  - Visit [Unity Download Archive](https://unity3d.com/get-unity/download/archive) and select version 2022.3.16f1.
  - Install via Unity Hub for best compatibility.
- **Open the Project:**
  1. Launch Unity Hub.
  2. Click **Add** and select the root folder of the cloned repository.
  3. Ensure the Editor Version is set to **2022.3.16f1**.
  4. Click the project name (`interoceptive-ai-env`) to open it in Unity.

### 2. Python & ML-Agents (Optional, for training)

- **Python Version:** 3.10.x recommended.
- **ML-Agents:**
  - Install via pip:
    ```bash
    pip install mlagents==1.0.0 mlagents-envs==1.0.0
    ```
  - For more details, see [Unity ML-Agents Toolkit](https://github.com/Unity-Technologies/ml-agents).

### 3. Additional Steps

- **Clone this repository:**
  ```bash
  git clone https://anonymous.4open.science/r/evaaa-2486
  cd evaaa-2486
  ```
- **Configure your experiment:**
  - Edit `Config/mainConfig.json` to select your desired environment/task (see [Usage](#usage)).
- **Run the simulation:**
  - Press the **Play** button in the Unity Editor to start the environment.
  - For training, use the ML-Agents Python API as described in the ML-Agents documentation.

---

**Tip:**
If you encounter package or dependency issues, ensure your Unity version matches the project and that all required packages are installed via the Unity Package Manager.

## Project Structure

The EVAAA project is organized into several core script groups, each responsible for a key aspect of the simulation environment. For detailed documentation on each group, see the linked markdown files in the `Documents/` folder.

### 1. Agent Scripts
Defines the core logic for agent behavior, perception, and interaction with the environment, including both learning agents and non-player agents (e.g., predators).
- [See detailed documentation → Documents/Agent.md](Documents/Agent.md)

### 2. Environment Scripts
Handles procedural generation and management of environmental elements and dynamics. The environment system is modular, with each component documented separately:
- [Environment System Overview → Documents/Environment.md](Documents/Environment.md)
  - [ThermoGridSpawner](Documents/ThermoGridSpawner_Introduction.md)
  - [CourtSpawner](Documents/CourtSpawner_Introduction.md)
  - [ObstacleSpawner](Documents/ObstacleSpawner_Introduction.md)
  - [ResourceSpawner](Documents/ResourceSpawner_Introduction.md)
  - [PredatorSpawner](Documents/PredatorSpawner_Introduction.md)
  - [DayAndNight](Documents/DayAndNight_Introduction.md)

### 3. Event System
Provides a flexible mechanism for managing and triggering in-game events, enabling dynamic interactions between agents and the environment.
- [See detailed documentation → Documents/EventSystem.md](Documents/EventSystem.md)

### 4. Scene Controllers
Responsible for initializing, configuring, and managing the simulation environment, ensuring reproducible and flexible experiments.
- [See detailed documentation → Documents/SceneControllers.md](Documents/SceneControllers.md)

### 5. User Interface (UI)
Provides real-time visualization and control interfaces for agents, environment states, and experiment feedback.
- [See detailed documentation → Documents/UI.md](Documents/UI.md)

### 6. Utility Scripts
General-purpose functions, data structures, and editor tools that support core simulation, data collection, and environment setup.
- [See detailed documentation → Documents/Utility.md](Documents/Utility.md)

---

**Note:** For each group, the corresponding markdown file in the `Documents/` folder provides a summary of the main scripts, their purposes, and key features. Subcomponents of the environment are also documented individually for clarity and modularity.

## Usage

### Configuration System Overview

EVAAA's environments, tasks, and experiment settings are fully configurable via structured JSON files and folders in the `Config/` directory. This system enables easy switching between training levels, experimental testbeds, and custom scenarios—without modifying code.

#### 1. Config Directory Structure
- **Config/mainConfig.json**: The entry point for all experiments. Specifies the active configuration folder (e.g., `"configFolderName": "exp-multiGoalPlanning"`) and global settings such as AI control, recording options, and data output.
- **Config/[experiment-folder]/**: Each subfolder (e.g., `train-level-1.1-ScatteredResource`, `exp-multiGoalPlanning`) contains all JSON files needed to define a specific environment, task, or experiment. These typically include:
  - `agentConfig.json`
  - `courtConfig.json`
  - `resourceConfig.json`
  - `obstacleConfig.json`
  - `thermoGridConfig.json`
  - `predatorConfig.json`
  - ...and other task-specific files

#### 2. How Configuration is Loaded
- The `ConfigLoader` script (see `Assets/Scripts/SceneController/ConfigLoader.cs`) reads `mainConfig.json` to determine which experiment folder to use.
- All environment, agent, and task parameters are then loaded from the selected folder.
- This design allows for rapid experiment iteration, reproducibility, and sharing of environment setups.

#### 3. Example: mainConfig.json
```json
{
    "isAIControlled": true,
    "configFolderName": "exp-multiGoalPlanning",
    "recordingScreen": {
        "recordEnable": "false",
        "recordingFolderName": "SampleRecording"
    },
    "experimentData": {
        "recordEnable": "false",
        "baseFolderName": "ExperimentData",
        "fileNamePrefix": "data"
    }
}
```

---

### How to Use

1. **Select or Create a Configuration**
   - To run a specific environment or task, set the `"configFolderName"` in `Config/mainConfig.json` to the desired experiment folder (e.g., `"train-level-2.1-Obstacles"`, `"exp-multiGoalPlanning"`).
   - Each folder contains all necessary JSON files to define the environment, agent, and task.

2. **Launch the Environment**
   - Open the Unity project and ensure the `Config/mainConfig.json` points to your desired configuration.
   - Run the simulation in the Unity Editor or build and execute the standalone player.

3. **Customization**
   - To create new tasks or modify existing ones, copy an existing config folder, edit the JSON files, and update `mainConfig.json` to point to your new folder.
   - You can adjust agent parameters, environment layout, resource placement, obstacles, thermal fields, predator behavior, and more—all via config files.

4. **Data Recording**
   - Enable or disable data and screen recording in `mainConfig.json` under `"recordingScreen"` and `"experimentData"`.
   - Output folders and file prefixes can also be set here.

---

This configuration system enables rapid, reproducible experimentation and makes EVAAA highly extensible for new research scenarios.

## Environments & Tasks

EVAAA features a two-tiered environment architecture designed to study autonomy, adaptivity, and internal state regulation in reinforcement learning agents:

### 1. Naturalistic Training Curriculum
A sequence of progressively challenging levels, each grounded in internal-state regulation and designed to scaffold adaptive survival behavior:

- **Level 1: Basic Resource Foraging**
  - Agents learn to regulate food and water without external interference.
  - Sublevels: Randomly distributed resources in an open field (1-1); resources relocated to a fixed corner (1-2).
  - No obstacles; isolates essential variable (EV)-driven behavior.

- **Level 2: Obstacle-Resource Mapping**
  - Increased complexity with obstacles and spatial cues.
  - Bonfires introduce localized heat; rocks and bushes obstruct navigation and inflict damage.
  - Stable layouts encourage association of cues with resources.

- **Level 3: Terrain Exploration and Navigation**
  - Agents must explore semi-structured terrain while balancing competing EVs.
  - Wall-enclosed tree regions require efficient path planning amid thermal and damage constraints.
  - Food appears at only one tree region per episode, requiring dynamic search and strategy revision.

- **Level 4: Dynamic Threatening Environment**
  - External threats and temporal changes challenge behavioral flexibility.
  - Predators inflict damage and must be evaded.
  - Day-night cycle modulates temperature and predator behavior, requiring time-aware planning and adaptation.

### 2. Experimental Testbeds
A suite of controlled environments designed to isolate specific decision-making challenges and evaluate generalization:

#### Basic Homeostatic Regulation
- **Two-Resource Choice Task:** Agents choose between two resources (e.g., food vs. water) based on internal need, under partial observability.
- **Avoiding Collision Tasks:** Agents must reach a visible resource while avoiding narrowly spaced obstacles, testing fine-grained movement control and damage minimization.

#### Advanced Adaptive Skills
- **Thermal Risk Task:** A resource is placed behind a bonfire that elevates internal temperature. Agents must plan whether to endure the heat or cool down first.
- **Spatial Navigation (Y-maze):** Two resources are placed at separate ends of a Y-shaped layout, requiring agents to revisit and redirect based on changing internal needs.
- **Goal Manipulation (Switch EV Level):** A transparent boundary triggers a hidden change in EV levels. Agents must detect the shift and re-prioritize without external cues.
- **Multi-Goal Planning:** Agents infer urgency across multiple EVs and act in priority order, testing coordination and sequential regulation.
- **Avoid Predators with Day/Night:** Agents autonomously suppress actions during the day to avoid predators, shifting to active foraging at night—testing adaptive, time-aware survival behavior.

---

This curriculum and testbed suite enable systematic analysis of how agents learn, adapt, and generalize under internal-state-driven demands in complex, dynamic environments.

## Baselines & Evaluation
<!-- List of supported baseline algorithms (DQN, PPO, DreamerV3, etc.) -->
<!-- How to reproduce results from the paper -->
<!-- Evaluation metrics -->

## Customization
<!-- How to modify agent parameters, environment settings, or curriculum via config files -->

## Citing EVAAA
<!-- BibTeX entry for citation -->

## License
<!-- License type and summary -->

## Contact
<!-- Contact information for questions or collaboration -->

## Acknowledgements
<!-- Credits for Unity ML-Agents, asset sources, and any collaborators or funding sources -->
