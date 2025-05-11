# Agent System Overview

The Agent system in EVAAA defines the core logic for agent behavior, perception, and interaction with the environment. This modular system enables both the main learning agent and non-player agents (such as predators) to sense, act, and adapt within the simulation.

Below are the main components of the Agent system. Each component is documented in detail in its own markdown file:

- [InteroceptiveAgent](./InteroceptiveAgent.md): The primary reinforcement learning agent, integrating internal physiological variables and multimodal perception.
- [Predator](./Predator.md): Implements dynamic predator agents that act as threats in the environment.
- [ObjectRaycast](./ObjectRaycast.md): Provides collision and obstacle detection for the agent using raycasting.
- [ThermalSensing](./ThermalSensing.md): Enables the agent to sense local environmental temperature using thermal grid data.
- [ResourceEating](./ResourceEating.md): Handles the agent's interaction with consumable resources (food, water, pond).
- [ConfigurableCameraSensor](./ConfigurableCameraSensor.md): Provides a flexible camera sensor for visual observations, compatible with Unity ML-Agents.

For details on each component, see the corresponding markdown file linked above. 