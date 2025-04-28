# ResourceSpawner

## Description

The `ResourceSpawner` component implements adaptive resource distribution for reinforcement learning environments with embodied agents. It provides configurable mechanisms for deploying consumable, collectable, or interactive resources throughout the environment with various spatiotemporal patterns, as described in our paper "EVAAA: Embodied Virtual Agents with Artificial Autonomic Systems" (NeurIPS 2025).

## Research Context

Resource management is central to embodied reinforcement learning research, particularly for studying:

- Foraging behaviors and optimal resource gathering strategies
- Trade-offs between homeostatic regulation and resource acquisition
- Multi-agent competition and cooperation for limited resources
- Temporal planning and adaptation to changing resource availability

Our implementation supports research across these domains by providing precise control over the type, quantity, and distribution patterns of environmental resources.

## Implementation Details

The resource system provides three distinct distribution strategies with different research applications:

1. **Static Resources** (`Static`): Fixed-position resources that remain constant throughout an episode. Useful for studying spatial memory, landmark navigation, and predictable resource environments.

2. **Random Resources** (`Random`): Stochastically distributed resources with configurable spatial constraints. Valuable for investigating exploration strategies and adaptability to uncertainty.

3. **Grouped Random Resources** (`GroupedRandom`): Clustered resource distributions that simulate natural resource patterns like food patches or water sources. Enables research on foraging strategies and resource area exploitation.

## Research Implications

Resource configuration directly impacts several aspects of reinforcement learning experiments:

1. **Exploration-Exploitation Balance**: Resource distribution patterns influence the optimal balance between exploration and exploitation
2. **Long-term vs. Short-term Planning**: Resource density affects the importance of long-term planning
3. **Multi-agent Dynamics**: Resource scarcity can induce competition or cooperation between agents
4. **Generalization**: Varying resource distributions tests agent generalization capabilities

## Configuration Parameters

The resource system is configured via a JSON file with groups of resources, each with parameters:

| Parameter | Description | Research Implications |
|-----------|-------------|----------------------|
| `prefabName` | Type of resource to spawn | Determines resource properties and visual recognition challenges |
| `prefabLabel` | Semantic label for the resource | Enables categorical learning and semantic understanding |
| `count` | Number of resources in group | Controls resource density and scarcity |
| `position` | Placement bounds in arena | Defines spatial distribution constraints |
| `rotationRange` | Range of possible rotations | Adds variability to environment |
| `scaleRange` | Range of possible sizes | Affects visibility and recognition complexity |
| `resourceType` | Distribution strategy | Determines spatiotemporal patterns of resources |

## Integration with Benchmark Tasks

The `ResourceSpawner` is a core component in the following benchmark scenarios:

1. **Resource Collection**: Simple gathering of resources in various environments
2. **Resource-Temperature Trade-off**: Balancing homeostatic temperature regulation with resource gathering
3. **Resource Competition**: Multiple agents competing for limited resources
4. **Resource Cooperation**: Agents sharing information or coordinating to optimize collection

## Example Configuration

```json
{
  "groups": [
    {
      "prefabName": "FoodResource",
      "prefabLabel": "Food",
      "count": 15,
      "position": {"xMin": -13.0, "xMax": 13.0, "yMin": 0.5, "yMax": 0.5, "zMin": -13.0, "zMax": 13.0},
      "rotationRange": {"x": 0, "y": 360, "z": 0},
      "scaleRange": {"xMin": 1.0, "xMax": 1.0, "yMin": 1.0, "yMax": 1.0, "zMin": 1.0, "zMax": 1.0},
      "resourceType": "GroupedRandom"
    },
    {
      "prefabName": "WaterResource",
      "prefabLabel": "Water",
      "count": 8,
      "position": {"xMin": -14.0, "xMax": 14.0, "yMin": 0.1, "yMax": 0.1, "zMin": -14.0, "zMax": 14.0},
      "rotationRange": {"x": 0, "y": 0, "z": 0},
      "scaleRange": {"xMin": 1.2, "xMax": 2.5, "yMin": 0.3, "yMax": 0.3, "zMin": 1.2, "zMax": 2.5},
      "resourceType": "Static"
    }
  ]
}
```

## Parameter Tuning for Research

For controlled reinforcement learning experiments, we recommend:

- **Abundance Studies**: Use high resource counts (>20) with wide spatial distribution
- **Scarcity Studies**: Use low resource counts (<10) with constrained spatial distribution
- **Foraging Research**: Use `GroupedRandom` with 3-5 clusters of resources
- **Exploration Research**: Use `Random` distribution with varying counts to study adaptation

For curriculum learning, begin with `Static` resources in obvious locations, then transition to `Random` and finally to `GroupedRandom` as agents develop more sophisticated strategies. 