[![License: CC BY-SA 4.0](https://img.shields.io/badge/License-CC%20BY--SA%204.0-lightgrey.svg)](https://creativecommons.org/licenses/by-sa/4.0/)
[![Python 3.8+](https://img.shields.io/badge/python-3.8+-blue.svg)](https://www.python.org/downloads/)
[![Unity 2022.3.16f1](https://img.shields.io/badge/Unity-2022.3.16f1-blue.svg)](https://unity.com/releases/editor/whats-new/2022.3.16)

# EVAAA: Essential Variables in Autonomous and Adaptive Agents

A biologically inspired platform for reinforcement learning research, combining a Unity-based 3D simulation environment with a Python-based training and evaluation suite.

![fig1](/image/fig1.png)

---

## 📄 Table of Contents
- [EVAAA: Essential Variables in Autonomous and Adaptive Agents](#evaaa-essential-variables-in-autonomous-and-adaptive-agents)
  - [📄 Table of Contents](#-table-of-contents)
  - [📝 Overview](#-overview)
  - [📦 Repository Structure](#-repository-structure)
  - [🚀 Quickstart Navigation](#-quickstart-navigation)
  - [📚 Citing EVAAA](#-citing-evaaa)
  - [📄 License](#-license)

---

## 📝 Overview
EVAAA (Essential Variables in Autonomous and Adaptive Agents) is a research platform for studying autonomy, adaptivity, and internal-state-driven control in reinforcement learning (RL) agents. The project consists of two main components:

- **Unity Simulation Environment** (`evaaa_unity`): A 3D, multimodal, curriculum-based environment where agents must regulate internal physiological variables (food, water, thermal, damage) to survive and adapt. Built with Unity ML-Agents, supporting rich sensory input and flexible configuration.
- **Python Training Suite** (`evaaa_train`): A modular training and evaluation framework (based on SheepRL) for developing RL agents in the EVAAA environment. Includes implementations of DQN, PPO, and DreamerV3, with tools for logging, evaluation, and curriculum learning.

---

## 📦 Repository Structure

```
.
├── evaaa_unity/   # Unity simulation environment (C#, Unity ML-Agents)
│   └── README.md  # Detailed Unity environment usage & setup
├── evaaa_train/   # Python training & evaluation suite
│   └── README.md  # Detailed training usage & setup
└── README.md      # (You are here)
```

- **`evaaa_unity/`**: Contains the Unity project for the EVAAA simulation environment. See [`evaaa_unity/README.md`](evaaa_unity/README.md) for setup, configuration, and usage instructions.
- **`evaaa_train/`**: Contains the Python code for training and evaluating RL agents in EVAAA. See [`evaaa_train/README.md`](evaaa_train/README.md) for installation, training commands, and evaluation details.

---

## 🚀 Quickstart Navigation

| Component         | Description                                      | Quick Link                                  |
|-------------------|--------------------------------------------------|---------------------------------------------|
| Unity Environment | 3D simulation, agent embodiment, configuration   | [evaaa_unity/README.md](evaaa_unity/README.md) |
| Python Training   | RL algorithms, logging, evaluation, curriculum   | [evaaa_train/README.md](evaaa_train/README.md) |

- **New to EVAAA?**
  1. Start with the [Unity environment setup](evaaa_unity/README.md) to explore or customize the simulation.
  2. Then follow the [Python training guide](evaaa_train/README.md) to train and evaluate RL agents.

---

## 📚 Citing EVAAA


---

## 📄 License
This project is licensed under the [CC BY-SA 4.0 License](https://creativecommons.org/licenses/by-sa/4.0/).