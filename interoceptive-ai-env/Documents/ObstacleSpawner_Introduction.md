# ObstacleSpawner

## Description

The `ObstacleSpawner` component implements dynamic obstacle placement for reinforcement learning environments with embodied agents. It enables the creation of complex navigational challenges with thermally-active obstacles that impact both agent movement and environmental temperature, as described in our paper "EVAAA: Embodied Virtual Agents with Artificial Autonomic Systems" (NeurIPS 2025).

## Research Context

Environmental obstacles play a critical role in embodied AI research by:

- Creating navigation and path-planning challenges
- Forming decision points that require trade-offs (e.g., shorter path vs. thermal comfort)
- Providing objects with thermal properties that influence environment dynamics
- Enabling the study of transfer learning across environments with varying complexity

Thermally-active obstacles are particularly relevant for studying interoceptive AI systems, as they create spatially-anchored thermal anomalies that agents must factor into their decision-making.

## Implementation Details

The obstacle system provides several key features relevant to reinforcement learning research:

- **Parameterized Obstacle Groups**: Multiple types and configurations of obstacles
- **Thermal Properties**: Each obstacle can have its own temperature, affecting local environmental conditions
- **Spatial Distribution Control**: Configurable position ranges, rotations, and scales
- **Collision Avoidance**: Intelligent placement with overlap prevention
- **Runtime Regeneration**: Environment can be restructured during experiments

## Research Implications

Obstacle configuration directly impacts several aspects of reinforcement learning experiments:

1. **Navigation Complexity**: Higher obstacle counts increase path-planning challenges
2. **Temperature Regulation**: Thermally-active obstacles create localized temperature gradients
3. **Observation Complexity**: Obstacles may occlude visual information or resources
4. **Exploration Strategies**: Obstacle distribution affects optimal exploration patterns

## Configuration Parameters

The obstacle system is configured via a JSON file with groups of obstacles, each with parameters:

| Parameter | Description | Research Implications |
|-----------|-------------|----------------------|
| `prefabName` | Type of obstacle to spawn | Visual/semantic properties of environment |
| `count` | Number of obstacles in group | Controls navigation complexity |
| `temperature` | Thermal property of obstacles | Influences thermal gradients and agent thermoregulation |
| `position` | Placement bounds in arena | Controls spatial distribution and navigation paths |
| `rotationRange` | Range of possible rotations | Adds variability to environment |
| `scaleRange` | Range of possible sizes | Controls difficulty of navigation/avoidance |
| `padding` | Minimum distance between obstacles | Affects path widths and navigation difficulty |

## Integration with Benchmark Tasks

The `ObstacleSpawner` is a critical component in the following benchmark scenarios:

1. **Navigation with Obstacles**: Testing pathfinding abilities
2. **Thermal Regulation with Obstacles**: Navigating thermal landscape with physical constraints
3. **Resource Collection with Obstacles**: Optimizing paths to resources with obstacles
4. **Combined Scenario**: All elements together for complete environmental challenge

## Example Configuration

```json
{
  "groups": [
    {
      "prefabName": "ThermalRock",
      "count": 8,
      "temperature": 35.0,
      "position": {"xMin": -12.0, "xMax": 12.0, "yMin": 0.5, "yMax": 0.5, "zMin": -12.0, "zMax": 12.0},
      "rotationRange": {"x": 0, "y": 360, "z": 0},
      "scaleRange": {"xMin": 1.0, "xMax": 3.0, "yMin": 1.0, "yMax": 3.0, "zMin": 1.0, "zMax": 3.0},
      "padding": 2.5
    },
    {
      "prefabName": "ColdCrystal",
      "count": 5,
      "temperature": 5.0,
      "position": {"xMin": -14.0, "xMax": 14.0, "yMin": 0.5, "yMax": 0.5, "zMin": -14.0, "zMax": 14.0},
      "rotationRange": {"x": 0, "y": 360, "z": 0},
      "scaleRange": {"xMin": 0.8, "xMax": 1.5, "yMin": 0.8, "yMax": 2.0, "zMin": 0.8, "zMax": 1.5},
      "padding": 1.8
    }
  ]
}
```

## Parameter Tuning for Research

For controlled reinforcement learning experiments, we recommend:

- **Easy Environment**: Low obstacle count (<5), large padding (>3.0), uniform temperatures
- **Medium Environment**: Medium obstacle count (5-10), medium padding (2.0-3.0), varied temperatures
- **Hard Environment**: High obstacle count (>10), low padding (<2.0), extreme temperatures

For curriculum learning, progressively decrease padding and increase count as agent performance improves. 