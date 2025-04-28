# InteroceptiveAI Environment System

## Overview

This document introduces the InteroceptiveAI Environment System, an extensible framework for creating configurable reinforcement learning environments with interoceptive dynamics. The system is designed to support research in embodied artificial intelligence, homeostatic regulation, and physiological simulation as described in our 2025 NeurIPS submission, "EVAAA: Embodied Virtual Agents with Artificial Autonomic Systems."

The modular architecture allows researchers to construct custom environments with varying levels of complexity, from simple temperature-based navigation tasks to complex multi-agent scenarios with predator-prey dynamics and resource constraints.

## Research Applications

The environment system is particularly suited for research in:

- Homeostatic reinforcement learning
- Physiologically-motivated reward functions
- Embodied intelligence with internal state regulation
- Multi-agent cooperation under resource constraints
- Interoceptive awareness and decision-making

## Core Components

The environment is composed of several modular systems, each with its own configuration parameters:

- [ThermoGridSpawner](ThermoGridSpawner_Introduction.md) - Manages temperature dynamics in the environment
- [CourtSpawner](CourtSpawner_Introduction.md) - Generates the physical arena for experiments
- [ObstacleSpawner](ObstacleSpawner_Introduction.md) - Places obstacles with thermal properties
- [ResourceSpawner](ResourceSpawner_Introduction.md) - Manages consumable resources
- [PredatorSpawner](PredatorSpawner_Introduction.md) - Implements adversarial entities

Each component is configurable through JSON files, allowing researchers to perform systematic parameter sweeps and reproducible experiments.

## Research Benchmarks

The InteroceptiveAI environment provides several benchmark scenarios described in our paper:

1. **Temperature Regulation** - Agents must maintain homeostatic temperature through environment navigation
2. **Resource Gathering** - Agents must balance resource collection with temperature regulation
3. **Predator Avoidance** - Agents must avoid predators while maintaining homeostasis
4. **Multi-Agent Cooperation** - Multiple agents must share limited resources while maintaining collective welfare

## Reinforcement Learning Integration

The environment provides standardized observation and action spaces compatible with common RL frameworks:

### Observation Space
- Agent internal state (temperature, energy, etc.)
- Local environmental perception (temperature, obstacles, resources)
- Optional: Global environment state for centralized training

### Action Space
- Movement (continuous or discrete)
- Resource interaction
- Optional: Communication signals for multi-agent scenarios

### Reward Functions
- Homeostatic rewards based on temperature regulation
- Resource collection rewards
- Survival time
- Custom reward functions can be implemented for specific research questions

## Reproducible Research

To ensure experimental reproducibility, all environment configurations can be saved as JSON files, including:

```
{environment_name}/{experiment_id}/
  ├── thermoGridConfig.json
  ├── courtConfig.json  
  ├── obstacleConfig.json
  ├── resourceConfig.json
  └── predatorConfig.json
```

## Getting Started

For detailed documentation on each component, please refer to the individual component guides linked in the Core Components section.

For benchmark implementation details and baseline agent performance, please refer to our paper "EVAAA: Embodied Virtual Agents with Artificial Autonomic Systems" (NeurIPS 2025). 