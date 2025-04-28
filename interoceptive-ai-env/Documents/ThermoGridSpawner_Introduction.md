# ThermoGridSpawner

## Description

The `ThermoGridSpawner` implements a spatially-distributed thermal field for embodied reinforcement learning experiments. Inspired by biological thermoregulation mechanisms, this component provides a continuous environmental temperature gradient that agents must navigate to maintain homeostasis, as discussed in our paper "EVAAA: Embodied Virtual Agents with Artificial Autonomic Systems" (NeurIPS 2025).

## Research Context

Temperature regulation is a fundamental homeostatic process in biological systems. By implementing a configurable thermal environment, we enable research on:

- Homeostatic reinforcement learning with physiologically-motivated reward functions
- Sensorimotor mapping between interoceptive signals and spatial navigation
- Evolution of regulatory behaviors under varying environmental conditions
- Transfer learning between different thermal environments

## Implementation Details

The thermal grid is implemented as a matrix of discrete cells, each with a temperature value. The system provides several key features:

- **Temperature Gradients**: Continuous temperature fields with configurable gradients
- **Hot Spots**: Both random and object-linked thermal sources
- **Gaussian Smoothing**: Realistic heat diffusion using configurable Gaussian kernels
- **Dynamic Updates**: Temperature values that respond to time (day/night) and agent actions

## Observation Space

When used in reinforcement learning environments, the thermal grid provides the following observation components:

- Local temperature at agent position (scalar)
- Optional: Temperature gradient around agent (vector field)
- Optional: Global temperature map (partial or complete)

These observations can be configured via the observation preprocessor to match research requirements.

## Configuration Parameters

The thermal grid is configured via a JSON file with the following parameters:

| Parameter | Description | Research Implications |
|-----------|-------------|----------------------|
| `numberOfGridCubeX/Z` | Grid dimensions | Controls environment complexity and observation space dimensionality |
| `fieldDefaultTemp` | Baseline temperature | Sets the neutral thermal point for agent homeostasis |
| `hotSpotTemp` | Hot spot temperature | Determines the strength of thermal extremes |
| `hotSpotCount` | Number of hot spots | Controls environment complexity |
| `hotSpotSize` | Size of hot spots | Affects strategy complexity for thermal regulation |
| `smoothingSigma` | Gaussian smoothing parameter | Controls temperature gradient smoothness |
| `useObjectHotSpot` | Enable object-linked hot spots | Links thermal properties to physical objects |
| `useRandomHotSpot` | Enable random hot spots | Creates unpredictable thermal landscapes |
| `gridCubeHeight` | Visual height of grid visualization | Visualization parameter without direct research impact |

## Integration with Benchmark Tasks

The `ThermoGridSpawner` is a core component in the following benchmark scenarios:

1. **Basic Thermal Regulation**: Agents navigate to maintain optimal temperature
2. **Thermal Regulation with Obstacles**: Navigation with physical constraints
3. **Resource-Temperature Trade-off**: Balancing resource gathering with temperature regulation

## Example Configuration

```json
{
  "numberOfGridCubeX": 20,
  "numberOfGridCubeZ": 20,
  "fieldDefaultTemp": 20.0,
  "hotSpotTemp": 40.0,
  "hotSpotCount": 3,
  "hotSpotSize": 2.0,
  "smoothingSigma": 1.5,
  "useObjectHotSpot": true,
  "useRandomHotSpot": true,
  "gridCubeHeight": 0.1
}
```

## Parameter Tuning for Research

For research on temperature regulation behaviors, we recommend:

- Varying `fieldDefaultTemp` to study adaptation to different baseline environments
- Adjusting `smoothingSigma` to investigate the impact of gradient sharpness on learning
- Modifying `hotSpotCount` and `hotSpotSize` to control task difficulty

For reproducible experiments, maintain consistent thermal parameters across training runs. 