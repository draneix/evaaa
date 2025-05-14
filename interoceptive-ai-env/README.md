[![License: CC BY-SA 4.0](https://img.shields.io/badge/License-CC%20BY--SA%204.0-lightgrey.svg)](https://creativecommons.org/licenses/by-sa/4.0/)
[![Unity 2022.3.16f1](https://img.shields.io/badge/Unity-2022.3.16f1-blue.svg)](https://unity.com/releases/editor/whats-new/2022.3.16)
[![C#](https://img.shields.io/badge/C%23-9.0-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)

# EVAAA: Essential Variables in Autonomous and Adaptive Agents

## 📄 Table of Contents
- [EVAAA: Essential Variables in Autonomous and Adaptive Agents](#evaaa-essential-variables-in-autonomous-and-adaptive-agents)
  - [📄 Table of Contents](#-table-of-contents)
  - [📝 Overview](#-overview)
  - [🚀 Quickstart Example](#-quickstart-example)
  - [🛠️ Installation](#️-installation)
    - [1. Prerequisites](#1-prerequisites)
    - [2. Clone the Repository](#2-clone-the-repository)
    - [3. Open the Project in Unity](#3-open-the-project-in-unity)
    - [4. Configure Your Experiment](#4-configure-your-experiment)
    - [5. Run the Simulation](#5-run-the-simulation)
  - [🕹️ Usage](#️-usage)
    - [⚙️ Configuration System: Step-by-Step Guide](#️-configuration-system-step-by-step-guide)
      - [1. How the Config System Works](#1-how-the-config-system-works)
      - [2. Selecting an Experiment or Environment](#2-selecting-an-experiment-or-environment)
      - [3. Modifying Environment, Agent, or Task Settings](#3-modifying-environment-agent-or-task-settings)
      - [4. Example: Adding More Food](#4-example-adding-more-food)
      - [5. Creating a New Experiment](#5-creating-a-new-experiment)
      - [6. Tips and Troubleshooting](#6-tips-and-troubleshooting)
  - [📦 Project Structure](#-project-structure)
  - [🌍 Environments \& Tasks](#-environments--tasks)
    - [📈 Naturalistic Training Curriculum](#-naturalistic-training-curriculum)
    - [🧪 Experimental Testbeds](#-experimental-testbeds)
      - [Basic Homeostatic Regulation](#basic-homeostatic-regulation)
      - [Advanced Adaptive Skills](#advanced-adaptive-skills)
  - [🛠️ Customization](#️-customization)
  - [📚 Citing EVAAA](#-citing-evaaa)
  - [📬 Contact](#-contact)
  - [🙏 Acknowledgements](#-acknowledgements)
  - [📄 License](#-license)

---

## 📝 Overview
EVAAA (Essential Variables in Autonomous and Adaptive Agents) is a biologically inspired 3D simulation platform for reinforcement learning (RL) research. Unlike traditional RL environments that rely on externally defined, task-specific rewards, EVAAA grounds agent motivation in the regulation of internal physiological variables—such as food, water, thermal balance, and damage—mirroring the homeostatic drives found in biological organisms.

A unique strength of EVAAA is its dual-environment architecture:
- **Progressive Survival Curriculum:** Agents are trained in a sequence of naturalistic environments of increasing complexity, where they must autonomously maintain essential variables under dynamic, multimodal conditions. This curriculum scaffolds the emergence of adaptive survival behaviors, from basic resource foraging to environments with obstacles, predators, and temporal changes.
- **Unseen Experimental Testbeds:** Beyond the training curriculum, EVAAA provides a suite of controlled, previously unseen test environments. These testbeds are designed to isolate and rigorously evaluate specific decision-making challenges—such as resource prioritization, collision avoidance, thermal risk, multi-goal planning, and adaptive behavior under novel conditions—enabling systematic assessment of generalization and internal-state-driven control.

Key features include:
- **Multimodal Perception:** Agents experience the world through vision, olfaction, thermoception, collision detection, and interoception.
- **Unified, Intrinsic Reward System:** Rewards are derived from internal state dynamics, enabling autonomous goal generation and reducing the need for manual reward engineering.
- **Modular & Extensible Design:** All core systems (Agent, Environment, Event, SceneControllers, UI, Utility) are highly modular and configurable via JSON, supporting rapid experiment iteration and reproducibility.

EVAAA thus provides a principled, extensible framework for studying autonomy, adaptivity, and internal-state-driven control in RL agents, bridging the gap between artificial and biological models of adaptive behavior, and enabling both the development and systematic evaluation of robust, generalizable agent behaviors.

## 🚀 Quickstart Example

Follow these steps to launch your first EVAAA simulation:

1. **Clone the Repository**
   ```bash
   git clone https://github.com/cocoanlab/interoceptive-ai-env.git
   cd interoceptive-ai-env
   ```

2. **Open the Project in Unity**
   - Launch **Unity Hub**.
   - Click **Add** and select the cloned `interoceptive-ai-env` folder.
   - Open the project using **Unity Editor 2022.3.16f1**.

3. **Select an Environment or Task**
   - Open `Config/mainConfig.json` in your preferred editor.
   - Set the `"configFolderName"` field to your desired experiment (e.g., `"train-level-1.1-ScatteredResource"`, `"exp-multiGoalPlanning"`).

4. **Run the Simulation**
   - In the Unity Editor, press the **Play** button.
   - Observe the agent interacting with the environment.

---

You can now explore, modify, or extend the EVAAA environments and tasks.

## 🛠️ Installation

Follow these detailed steps to set up Unity and run EVAAA:

### 1. Prerequisites
- **Unity Editor:** Install [Unity Hub](https://unity3d.com/get-unity/download) and use it to install **Unity Editor 2022.3.16f1** (LTS version). This is the only tested version for EVAAA.
- **Git:** Make sure you have [Git](https://git-scm.com/downloads) installed to clone the repository.

### 2. Clone the Repository
Open a terminal (Command Prompt, PowerShell, or Terminal) and run:
```bash
git clone https://github.com/cocoanlab/interoceptive-ai-env.git
cd interoceptive-ai-env
```

### 3. Open the Project in Unity
1. Launch **Unity Hub**.
2. Click **Add** (or the plus `+` icon) and select the folder where you cloned `interoceptive-ai-env`.
3. In Unity Hub, make sure the project is set to use **Unity Editor 2022.3.16f1**. If not, click the three dots next to the project and select **Add Editor Version** to install it.
4. Click the project name to open it in Unity.

### 4. Configure Your Experiment
1. In your file browser or code editor, open `Config/mainConfig.json`.
2. Set the `"configFolderName"` field to the experiment you want to run (e.g., `"train-level-1.1-ScatteredResource"`, `"exp-multiGoalPlanning"`).
3. You can further customize agent, environment, and task parameters by editing the JSON files in the corresponding config folder.

### 5. Run the Simulation
1. In the Unity Editor, open the *MainSurvivalEnvironment* scene in the `Assets/Scene` folder (double-click to open).
2. Press the **Play** button at the top of the Unity Editor window.
3. All environment and task changes are controlled via the configuration files—there is no need to switch scenes.

**Manual Control:**
- In the Unity Editor, you can control the agent manually using your keyboard:
  - **Left:** ← (Left Arrow)
  - **Right:** → (Right Arrow)
  - **Forward:** ↑ (Up Arrow)
  - **Eat:** Spacebar
- This allows you to test and explore the environment as a human player.

---

**Tips for Beginners:**
- If you see any package or dependency errors, open the **Window > Package Manager** in Unity and let it resolve or install missing packages.
- For more information on Unity basics, see the [Unity Learn](https://learn.unity.com/) portal.
- You can always reset your configuration by restoring the original JSON files from the repository.

You are now ready to explore, modify, or extend EVAAA environments and tasks!

## 🕹️ Usage

### ⚙️ Configuration System: Step-by-Step Guide

EVAAA is designed so that all environment, agent, and task settings are controlled through easy-to-edit configuration (config) files. You do **not** need to change any code or Unity scenes to create new experiments or modify existing ones. This makes the system accessible even if you are new to Unity or programming.

#### 1. How the Config System Works
- The main config file (`Config/mainConfig.json`) tells EVAAA which experiment setup to use.
- Each experiment setup is a folder inside `Config/` (e.g., `train-level-1.1-ScatteredResource`, `train-level-2.1-Obstacles`).
- Each folder contains several JSON files, each controlling a different aspect of the environment (agent, resources, obstacles, etc.).
- When you run the simulation, EVAAA loads all settings from these files automatically.

#### 2. Selecting an Experiment or Environment
- Open `Config/mainConfig.json` in a text editor.
- Find the line: `"configFolderName": "train-level-1.1-ScatteredResource"` (the value may differ).
- Change the value to the name of any other folder in `Config/` to switch to a different environment or task.
- **Example:**
  ```json
  {
    "isAIControlled": true,
    "configFolderName": "train-level-2.1-Obstacles",
    ...
  }
  ```
- Save the file. The next time you press Play in Unity, the new environment will load.

**Pre-existing config folders you can use:**

| Survival Curriculum                | Experimental Testbeds                |
|------------------------------------|--------------------------------------|
| train-level-1.1-ScatteredResource  | exp-multiGoalPlanning                |
| train-level-1.2-CorneredResource   | exp-two-resource-food                |
| train-level-2.1-Obstacles          | exp-two-resource-water               |
| train-level-2.2-ResourceMapping    | exp-two-resource-thermo              |
| train-level-3.1-Navigation         | exp-Ymaze                            |
| train-level-3.2-Exploration        | exp-goal-manipulation-FoodToWater    |
| train-level-4.1-Predator           | exp-goal-manipulation-WaterToFood    |
| train-level-4.2-DayAndNight        | exp-riskTaking                       |
|                                    | exp-damage                           |
|                                    | exp-predator                         |


#### 3. Modifying Environment, Agent, or Task Settings
- Open the folder named in `configFolderName` (e.g., `Config/train-level-2.1-Obstacles/`).
- Each JSON file in this folder controls a different part of the simulation:
  - `agentConfig.json`: Agent movement, sensors, internal state, etc.
  - `resourceConfig.json`: Types, number, and placement of resources (food, water, etc.).
  - `obstacleConfig.json`: Types, number, and placement of obstacles (rocks, bushes, bonfires, etc.).
  - `daynightConfig.json`: Day/night cycle and lighting.
  - `thermoGridConfig.json`: Temperature grid settings.
  - ...and more.
- Open any of these files in a text editor. Change values as needed. **Examples:**
  - To add more food, increase the `count` in the `Food` group in `resourceConfig.json`.
  - To make the agent start with less health, lower the `startHealthLevel` in `agentConfig.json`.
  - To add obstacles, increase the `count` for rocks or bushes in `obstacleConfig.json`.
- Save your changes. Press Play in Unity to see the effect.

#### 4. Example: Adding More Food
Suppose you want to double the amount of food in the environment:
- Open `Config/train-level-1.1-ScatteredResource/resourceConfig.json`.
- Find the section:
  ```json
  {
    "prefabName": "Food",
    ...
    "count": 50,
    ...
  }
  ```
- Change `"count": 50` to `"count": 100`.
- Save the file and run the simulation.

#### 5. Creating a New Experiment
- Copy any existing config folder (e.g., `train-level-1.1-ScatteredResource`) and give it a new name (e.g., `my-custom-experiment`).
- Edit the JSON files in your new folder to set up your custom environment.
- In `Config/mainConfig.json`, set `"configFolderName"` to your new folder name.
- Save and run. You now have a new, reproducible experiment setup!

#### 6. Tips and Troubleshooting
- If you make a mistake in a JSON file, Unity may show an error or the simulation may not load as expected. Double-check your edits for typos or missing commas/brackets.
- You can always restore the original config files from your Git repository if needed.
- For more details on what each parameter means, see the documentation in the `Documents/` folder or ask for help.

---

By using the config system, you can easily explore, modify, and extend EVAAA's environments and tasks—no coding required!

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

## 🌍 Environments & Tasks

EVAAA features a two-tiered environment architecture designed to study autonomy, adaptivity, and internal state regulation in reinforcement learning agents:

### 📈 Naturalistic Training Curriculum

EVAAA's training environments are structured as a sequence of four progressively challenging levels, each designed to scaffold adaptive survival behavior through internal-state regulation. Each level introduces new behavioral demands, environmental complexity, and learning opportunities:

| **Level** | **Name**                        | **Config Name**                        | **Description** |
|-----------|---------------------------------|----------------------------------------|-----------------|
| 1         | Basic Resource Foraging         | train-level-1.1-ScatteredResource      | Randomly distributed food and water in an open field with variable temperatures. Establishes baseline consumption behavior. |
|           |                                 | train-level-1.2-CorneredResource       | Resources relocated to a fixed corner, requiring spatial targeting based on internal need. Both sublevels are obstacle-free to isolate essential variable (EV)-driven behavior. |
| 2         | Obstacle-Resource Mapping       | train-level-2.1-Obstacles              | Bonfires introduce localized heat, requiring agents to regulate thermal state. Rocks and bushes obstruct navigation and inflict damage. |
|           |                                 | train-level-2.2-ResourceMapping        | Stable layouts (e.g., pond location, food near trees) encourage agents to associate specific cues with essential resources. Environmental complexity increases with obstacles and cues. |
| 3         | Terrain Exploration & Navigation| train-level-3.1-Navigation             | Wall-enclosed tree regions demand efficient path planning amid thermal and damage constraints. |
|           |                                 | train-level-3.2-Exploration            | Food appears at only one tree region per episode, forcing dynamic search and strategy revision as availability shifts. Agents must balance competing EVs while exploring semi-structured terrain. |
| 4         | Dynamic Threatening Environment | train-level-4.1-Predator               | A predator inflicts damage and must be evaded. |
|           |                                 | train-level-4.2-DayAndNight            | A day-night cycle modulates temperature and predator behavior, requiring time-aware planning and adaptation to shifting external demands. External threats and temporal changes challenge behavioral flexibility. |

---

### 🧪 Experimental Testbeds

EVAAA includes a suite of controlled testbed environments, each designed to isolate specific decision-making challenges and evaluate generalization. These are organized into two categories:

#### Basic Homeostatic Regulation
| **Category** | **Task Name**              | **Config Name**              | **Description** |
|--------------|---------------------------|------------------------------|-----------------|
| Basic        | Two-Resource Choice (Food) | exp-two-resource-food        | Agents choose between food and water based on internal need, under partial observability. Tests internal-state-driven prioritization. |
| Basic        | Two-Resource Choice (Water)| exp-two-resource-water       | Similar to above, but with different initial internal states or resource distributions. |
| Basic        | Two-Resource Choice (Thermo)| exp-two-resource-thermo     | Agents must regulate thermal state versus another essential variable. |
| Basic        | Avoiding Collision         | exp-riskTaking               | Agents must reach a visible resource while avoiding narrowly spaced obstacles, testing fine-grained movement control and damage minimization. |

#### Advanced Adaptive Skills
| **Category** | **Task Name**                | **Config Name**                   | **Description** |
|--------------|-----------------------------|-----------------------------------|-----------------|
| Advanced     | Multi-Goal Planning         | exp-multiGoalPlanning             | Agents infer urgency across multiple EVs and act in priority order, testing coordination and sequential internal-state regulation. |
| Advanced     | Spatial Navigation (Y-maze) | exp-Ymaze                         | Two resources are placed at separate ends of a Y-shaped layout, requiring agents to revisit and redirect based on changing internal needs. |
| Advanced     | Goal Manipulation (Food→Water) | exp-goal-manipulation-FoodToWater | A transparent boundary triggers a hidden change in EV levels. Agents must detect the shift and re-prioritize without external cues. |
| Advanced     | Goal Manipulation (Water→Food) | exp-goal-manipulation-WaterToFood | Similar to above, but with the opposite EV change. |
| Advanced     | Thermal Risk Task            | exp-predator                      | A resource is placed behind a bonfire or near a predator. Agents must plan whether to endure the risk or find an alternative strategy. |
| Advanced     | Predator Avoidance           | exp-predator                      | Agents autonomously suppress actions during the day to avoid predators, shifting to active foraging at night—testing adaptive, time-aware survival behavior. |

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