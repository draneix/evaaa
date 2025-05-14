[![License: CC BY-SA 4.0](https://img.shields.io/badge/License-CC%20BY--SA%204.0-lightgrey.svg)](https://creativecommons.org/licenses/by-sa/4.0/)
[![Unity 2022.3.16f1](https://img.shields.io/badge/Unity-2022.3.16f1-blue.svg)](https://unity.com/releases/editor/whats-new/2022.3.16)
[![C#](https://img.shields.io/badge/C%23-9.0-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)

# EVAAA: Essential Variables in Autonomous and Adaptive Agents

## 📄 Table of Contents
- [EVAAA: Essential Variables in Autonomous and Adaptive Agents](#evaaa-essential-variables-in-autonomous-and-adaptive-agents)
  - [📄 Table of Contents](#-table-of-contents)
  - [📝 Overview](#-overview)
  - [✨ Key Features](#-key-features)
  - [🚀 Quickstart Example](#-quickstart-example)
  - [🛠️ Installation](#️-installation)
  - [📦 Project Structure](#-project-structure)
  - [🕹️ Usage](#️-usage)
    - [⚙️ Configuration System Overview](#️-configuration-system-overview)
      - [📁 Config Directory Structure](#-config-directory-structure)
      - [🔄 How Configuration is Loaded](#-how-configuration-is-loaded)
      - [📝 Example: mainConfig.json](#-example-mainconfigjson)
    - [🚦 How to Use](#-how-to-use)
  - [🌍 Environments \& Tasks](#-environments--tasks)
    - [📈 Naturalistic Training Curriculum](#-naturalistic-training-curriculum)
    - [🧪 Experimental Testbeds](#-experimental-testbeds)
      - [🧬 Basic Homeostatic Regulation](#-basic-homeostatic-regulation)
      - [🦾 Advanced Adaptive Skills](#-advanced-adaptive-skills)
  - [🛠️ Customization](#️-customization)
  - [📚 Citing EVAAA](#-citing-evaaa)
  - [📬 Contact](#-contact)
  - [🙏 Acknowledgements](#-acknowledgements)
  - [📄 License](#-license)

---

## 📝 Overview
<!-- Briefly introduce EVAAA, its motivation, and its significance for RL research (1-2 paragraphs). -->

## ✨ Key Features
- Biologically inspired agent design with internal state regulation
- Egocentric, multimodal perception (vision, olfaction, thermoception, etc.)
- Progressive curriculum and experimental testbeds
- Unified, intrinsic reward system
- Built on Unity ML-Agents
- Open-source and fully configurable

## 🚀 Quickstart Example

Follow these steps to launch your first EVAAA simulation:

```bash
# 1. Clone the repository
 git clone https://anonymous.4open.science/r/evaaa-2486
 cd evaaa-2486
```

```text
# 2. Open in Unity:
# - Launch Unity Hub.
# - Click Add and select the cloned evaaa-2486 folder.
# - Open the project with Unity Editor 2022.3.16f1.
```

```text
# 3. Select an environment/task:
# - Edit Config/mainConfig.json and set "configFolderName" to your desired experiment (e.g., "train-level-1.1-ScatteredResource").
```

```text
# 4. Run the simulation:
# - Press the Play button in the Unity Editor.
# - Observe the agent interacting with the environment.
```

---

You can now explore, modify, or extend the EVAAA environments and tasks.

## 🛠️ Installation

<details>
<summary>🖥️ Unity Editor</summary>

- **Required Version:** EVAAA is developed and tested with **Unity Editor 2022.3.16f1**.
- **Download Unity:**
  - Visit [Unity Download Archive](https://unity3d.com/get-unity/download/archive) and select version 2022.3.16f1.
  - Install via Unity Hub for best compatibility.
- **Open the Project:**
  1. Launch Unity Hub.
  2. Click **Add** and select the root folder of the cloned repository.
  3. Ensure the Editor Version is set to **2022.3.16f1**.
  4. Click the project name (`interoceptive-ai-env`) to open it in Unity.

</details>

<details>
<summary>🔧 Additional Steps</summary>

- **Clone this repository:**
  ```bash
  git clone https://anonymous.4open.science/r/evaaa-2486
  cd evaaa-2486
  ```
- **Configure your experiment:**
  - Edit `Config/mainConfig.json` to select your desired environment/task (see [Usage](#-usage)).
- **Run the simulation:**
  - Press the **Play** button in the Unity Editor to start the environment.
  - For training, use the ML-Agents Python API as described in the ML-Agents documentation.

---

**Tip:**
If you encounter package or dependency issues, ensure your Unity version matches the project and that all required packages are installed via the Unity Package Manager.

</details>

## 📦 Project Structure

The EVAAA project is organized into several core script groups, each responsible for a key aspect of the simulation environment. For detailed documentation on each group, see the linked markdown files in the `Documents/` folder.

| Group | Description | Documentation |
|-------|-------------|---------------|
| 🤖 Agent Scripts | Core logic for agent behavior, perception, and interaction | [Agent.md](Documents/Agent/Agent.md) |
| 🌳 Environment Scripts | Procedural generation and management of environment | [Environment.md](Documents/Environment/Environment.md) |
| ⚡ Event System | In-game event management and triggers | [EventSystem.md](Documents/Event/EventSystem.md) |
| 🎬 Scene Controllers | Initialization and configuration of simulation | [SceneControllers.md](Documents/SceneControllers/SceneControllers.md) |
| 🖼️ User Interface (UI) | Visualization and control interfaces | [UI.md](Documents/UI/UI.md) |
| 🧰 Utility Scripts | General-purpose functions and tools | [Utility.md](Documents/Utility/Utility.md) |

---

**Note:** For each group, the corresponding markdown file in the `Documents/` folder provides a summary of the main scripts, their purposes, and key features. Subcomponents of the environment are also documented individually for clarity and modularity.

## 🕹️ Usage

### ⚙️ Configuration System Overview

EVAAA's environments, tasks, and experiment settings are fully configurable via structured JSON files and folders in the `Config/` directory. This system enables easy switching between training levels, experimental testbeds, and custom scenarios—without modifying code.

#### 📁 Config Directory Structure
- **Config/mainConfig.json**: The entry point for all experiments. Specifies the active configuration folder (e.g., `"configFolderName": "exp-multiGoalPlanning"`) and global settings such as AI control, recording options, and data output.
- **Config/[experiment-folder]/**: Each subfolder (e.g., `train-level-1.1-ScatteredResource`, `exp-multiGoalPlanning`) contains all JSON files needed to define a specific environment, task, or experiment. These typically include:
  - `agentConfig.json`
  - `courtConfig.json`
  - `resourceConfig.json`
  - `obstacleConfig.json`
  - `thermoGridConfig.json`
  - `predatorConfig.json`
  - ...and other task-specific files

#### 🔄 How Configuration is Loaded
- The `ConfigLoader` script (see `Assets/Scripts/SceneController/ConfigLoader.cs`) reads `mainConfig.json` to determine which experiment folder to use.
- All environment, agent, and task parameters are then loaded from the selected folder.
- This design allows for rapid experiment iteration, reproducibility, and sharing of environment setups.

#### 📝 Example: mainConfig.json
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

### 🚦 How to Use

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

## 🌍 Environments & Tasks

EVAAA features a two-tiered environment architecture designed to study autonomy, adaptivity, and internal state regulation in reinforcement learning agents:

### 📈 Naturalistic Training Curriculum
A sequence of progressively challenging levels, each grounded in internal-state regulation and designed to scaffold adaptive survival behavior:

| Level | Description |
|-------|-------------|
| 1 | **Basic Resource Foraging**<br>Agents learn to regulate food and water without external interference.<br>Sublevels: Randomly distributed resources in an open field (1-1); resources relocated to a fixed corner (1-2).<br>No obstacles; isolates essential variable (EV)-driven behavior. |
| 2 | **Obstacle-Resource Mapping**<br>Increased complexity with obstacles and spatial cues.<br>Bonfires introduce localized heat; rocks and bushes obstruct navigation and inflict damage.<br>Stable layouts encourage association of cues with resources. |
| 3 | **Terrain Exploration and Navigation**<br>Agents must explore semi-structured terrain while balancing competing EVs.<br>Wall-enclosed tree regions require efficient path planning amid thermal and damage constraints.<br>Food appears at only one tree region per episode, requiring dynamic search and strategy revision. |
| 4 | **Dynamic Threatening Environment**<br>External threats and temporal changes challenge behavioral flexibility.<br>Predators inflict damage and must be evaded.<br>Day-night cycle modulates temperature and predator behavior, requiring time-aware planning and adaptation. |

### 🧪 Experimental Testbeds
A suite of controlled environments designed to isolate specific decision-making challenges and evaluate generalization:

#### 🧬 Basic Homeostatic Regulation
- **Two-Resource Choice Task:** Agents choose between two resources (e.g., food vs. water) based on internal need, under partial observability.
- **Avoiding Collision Tasks:** Agents must reach a visible resource while avoiding narrowly spaced obstacles, testing fine-grained movement control and damage minimization.

#### 🦾 Advanced Adaptive Skills
- **Thermal Risk Task:** A resource is placed behind a bonfire that elevates internal temperature. Agents must plan whether to endure the heat or cool down first.
- **Spatial Navigation (Y-maze):** Two resources are placed at separate ends of a Y-shaped layout, requiring agents to revisit and redirect based on changing internal needs.
- **Goal Manipulation (Switch EV Level):** A transparent boundary triggers a hidden change in EV levels. Agents must detect the shift and re-prioritize without external cues.
- **Multi-Goal Planning:** Agents infer urgency across multiple EVs and act in priority order, testing coordination and sequential regulation.
- **Avoid Predators with Day/Night:** Agents autonomously suppress actions during the day to avoid predators, shifting to active foraging at night—testing adaptive, time-aware survival behavior.

---

This curriculum and testbed suite enable systematic analysis of how agents learn, adapt, and generalize under internal-state-driven demands in complex, dynamic environments.

## 🛠️ Customization
<!-- How to modify agent parameters, environment settings, or curriculum via config files -->

## 📚 Citing EVAAA
<!-- BibTeX entry for citation -->

## 📬 Contact
<!-- Contact information for questions or collaboration -->

## 🙏 Acknowledgements
<!-- Credits for Unity ML-Agents, asset sources, and any collaborators or funding sources -->

## 📄 License
CC BY-SA 4.0