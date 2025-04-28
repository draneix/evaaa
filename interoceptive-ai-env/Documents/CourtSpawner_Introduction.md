# CourtSpawner

## Description

The `CourtSpawner` component implements the physical arena for embodied reinforcement learning experiments. It provides a configurable environment with bounded spatial constraints, enabling controlled research on navigation, exploration, and spatial reasoning as described in our paper "EVAAA: Embodied Virtual Agents with Artificial Autonomic Systems" (NeurIPS 2025).

## Research Context

The physical structure of an environment significantly impacts agent behavior and learning. By providing a configurable arena, we enable research on:

- Spatial navigation under different environmental constraints
- Impact of bounded vs. unbounded environments on exploration strategies
- Transfer learning between environments of varying complexity
- Multi-agent interactions in shared physical spaces

## Implementation Details

The court implementation provides several key features relevant to reinforcement learning research:

- **Configurable Dimensions**: Fully parameterized floor size and wall height
- **Boundary Control**: Optional walls to create bounded or partially bounded environments
- **Material Properties**: Customizable surface materials to support visual feature learning
- **Coordinate System**: Consistent spatial reference frame for agent observations

## Research Implications

The court configuration directly impacts several aspects of reinforcement learning experiments:

1. **State Space Complexity**: Larger courts increase the spatial complexity of the environment
2. **Exploration Challenges**: Wall configurations affect exploration efficiency
3. **Visual Learning**: Material settings impact the visual features available to vision-based agents
4. **Multi-Agent Dynamics**: Court size influences agent density and interaction frequency

## Configuration Parameters

The court environment is configured via a JSON file with the following parameters:

| Parameter | Description | Research Implications |
|-----------|-------------|----------------------|
| `floorSize` | Dimensions of the court floor | Controls navigable area and environment scale |
| `wallHeight` | Height of boundary walls | Affects observation space (visual occlusion) |
| `position` | World-space position of court center | Establishes coordinate reference frame |
| `floorMaterialName` | Material for floor surfaces | Visual feature learning, semantic understanding |
| `wallMaterialName` | Material for wall surfaces | Visual feature learning, semantic understanding |
| `createWall` | Boolean flag to enable/disable walls | Bounded vs. unbounded environment studies |

## Integration with Benchmark Tasks

The `CourtSpawner` provides the physical foundation for all benchmark scenarios, including:

1. **Basic Navigation**: Agents learning to navigate within bounded spaces
2. **Thermal Regulation**: Providing spatial constraints for temperature navigation
3. **Resource Collection**: Defining the arena where resources are distributed
4. **Predator Avoidance**: Creating the shared space for predator-prey dynamics

## Example Configuration

```json
{
  "floorSize": {"x": 30.0, "y": 1.0, "z": 30.0},
  "wallHeight": 3.0,
  "position": {"x": 0.0, "y": 0.0, "z": 0.0},
  "floorMaterialName": "GreyMatte",
  "wallMaterialName": "BlueMatte",
  "createWall": true
}
```

## Parameter Tuning for Research

For controlled reinforcement learning experiments, we recommend:

- Standardizing `floorSize` across comparative studies to ensure fair comparison
- Using `createWall: true` for novice agent training to constrain exploration
- Varying court dimensions systematically to study scaling effects on learning
- Maintaining consistent `position` values for reproducible coordinate systems 