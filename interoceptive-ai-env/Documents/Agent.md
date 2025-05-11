# Agent Scripts Overview

The Agent scripts in EVAAA define the core logic for agent behavior, perception, and interaction with the environment. These scripts enable both the main learning agent and non-player agents (such as predators) to sense, act, and adapt within the simulation. Below is a summary of the main components:

## Main Components

### InteroceptiveAgent.cs
- **Purpose:** Implements the primary reinforcement learning agent, integrating internal physiological variables (essential variables: food, water, thermal, health) and multimodal perception (vision, olfaction, thermal, collision, touch).
- **Key Features:**
  - Loads agent configuration from JSON files for flexible experiments.
  - Handles agent initialization, observation collection, action execution, and reward calculation.
  - Manages internal state dynamics and updates essential variables based on agent actions and environment feedback.
  - Supports both AI-controlled and manual control modes.
  - Interfaces with data recording and environment configuration systems.

### Predator.cs
- **Purpose:** Defines the logic for predator agents that act as dynamic threats in the environment.
- **Key Features:**
  - Implements state-based behavior (resting, searching, chasing, attacking).
  - Uses Unity's NavMesh for navigation and pathfinding.
  - Interacts with the day/night cycle and landmark areas.
  - Applies damage to agents upon successful attack.

### ObjectRaycast.cs
- **Purpose:** Provides collision and obstacle detection for the agent using raycasting.
- **Key Features:**
  - Casts multiple rays in a radial pattern to detect obstacles and gather collision observations.
  - Calculates and applies damage to the agent based on collision impulse.

### ThermalSensing.cs
- **Purpose:** Enables the agent to sense local environmental temperature using thermal grid data.
- **Key Features:**
  - Reads temperature values from the environment based on the agent's position.
  - Supports dynamic adaptation to spatial temperature gradients.

### ResourceEating.cs
- **Purpose:** Handles the agent's interaction with consumable resources (food, water, pond).
- **Key Features:**
  - Detects when the agent is in range to consume resources.
  - Updates internal state and triggers resource respawn upon consumption.

### ConfigurableCameraSensor.cs
- **Purpose:** Provides a flexible camera sensor for visual observations, compatible with Unity ML-Agents.
- **Key Features:**
  - Configurable resolution, grayscale mode, and compression.
  - Automatically attaches or creates a camera for agent vision.

---

These scripts collectively enable rich, embodied agent behavior and perception, supporting the study of autonomy, adaptivity, and internal state regulation in complex environments. 