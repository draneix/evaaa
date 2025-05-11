# PredatorSpawner

## Description

The `PredatorSpawner` component implements adversarial agents within the environment, creating dynamic challenges for learning agents. It enables the creation of predator entities with configurable behavioral parameters, sensory capabilities, and attack properties, as described in our paper "EVAAA: Embodied Virtual Agents with Artificial Autonomic Systems" (NeurIPS 2025).

## Research Context

Predator-prey dynamics are fundamental to studying complex agent behaviors in reinforcement learning. The inclusion of adversarial entities serves multiple research purposes:

- Creating risk-reward tradeoffs that influence agent decision-making
- Enabling the study of avoidance behaviors and threat detection
- Introducing temporal pressure and urgency into agent planning
- Providing a mechanism for implementing dynamic difficulty adjustment
- Investigating multi-agent dynamics in competitive scenarios

Our implementation supports research across these domains by providing precise control over predator behavior and capabilities.

## Implementation Details

The predator system provides several key capabilities relevant to reinforcement learning research:

- **Configurable Sensory Systems**: Adjustable view angles and distances model limited perception
- **State-Based Behavior**: Transitions between resting, searching, and attacking states
- **Parameterized Movement**: Configurable movement speeds and rest durations
- **Attack Mechanics**: Damage amounts, intervals, and maximum damage values
- **Spatial Distribution**: Control over initial placement and density of predators

## Research Implications

Predator configuration directly impacts several aspects of reinforcement learning experiments:

1. **Risk Levels**: Predator count, damage, and sensory capabilities determine environmental risk
2. **Learning Complexity**: Sophisticated predator behaviors increase the complexity of optimal policies
3. **Exploration Disincentives**: Predators create exploration challenges and risk-aversion
4. **Emergent Behaviors**: Predator-prey interactions can lead to emergent avoidance strategies

## Configuration Parameters

The predator system is configured via a JSON file with groups of predators, each with parameters:

| Parameter | Description | Research Implications |
|-----------|-------------|----------------------|
| `prefabName` | Type of predator to spawn | Visual appearance and agent recognition challenges |
| `count` | Number of predators in group | Controls overall risk level in environment |
| `position` | Placement bounds in arena | Defines initial spatial distribution of threats |
| `rotationRange` | Range of possible rotations | Affects initial detection by learning agents |
| `scaleRange` | Range of possible sizes | Affects visibility and physical threat radius |
| `walkSpeed` | Movement speed | Determines difficulty of evasion strategies |
| `restDuration` | Time spent in resting state | Creates temporal patterns in risk levels |
| `viewAngle` | Field of view (degrees) | Models predator perception limitations |
| `viewDistance` | Maximum detection range | Balances predator advantage and safe distances |
| `damageAmount` | Damage per attack | Sets consequence severity for failed avoidance |
| `maxDamage` | Maximum damage capacity | Limits predator threat without intervention |
| `attackInterval` | Time between attacks | Creates rhythmic patterns of risk |
| `maxRestingSteps` | Max duration in rest state | Controls predictability of state transitions |
| `maxSearchingSteps` | Max duration in search state | Controls hunting persistence |
| `padding` | Minimum distance between predators | Prevents clustering and ensures distribution |

## Integration with Benchmark Tasks

The `PredatorSpawner` is a key component in the following benchmark scenarios:

1. **Predator Avoidance**: Simple survival task with predator evasion
2. **Resource Collection with Predators**: Balancing resource gathering with predator avoidance
3. **Thermal Regulation with Predators**: Managing homeostasis while avoiding threats
4. **Full Challenge**: Complete environment with all elements, maximizing task complexity

## Example Configuration

```json
{
  "groups": [
    {
      "prefabName": "WolfPredator",
      "count": 3,
      "position": {"xMin": -14.0, "xMax": 14.0, "yMin": 0.0, "yMax": 0.0, "zMin": -14.0, "zMax": 14.0},
      "rotationRange": {"x": 0, "y": 360, "z": 0},
      "scaleRange": {"xMin": 1.0, "xMax": 1.0, "yMin": 1.0, "yMax": 1.0, "zMin": 1.0, "zMax": 1.0},
      "walkSpeed": 3.5,
      "restDuration": 2.0,
      "viewAngle": 120.0,
      "viewDistance": 12.0,
      "damageAmount": 10.0,
      "maxDamage": 100.0,
      "attackInterval": 1.5,
      "maxRestingSteps": 8,
      "maxSearchingSteps": 15,
      "padding": 5.0
    },
    {
      "prefabName": "SnakePredator",
      "count": 4,
      "position": {"xMin": -12.0, "xMax": 12.0, "yMin": 0.0, "yMax": 0.0, "zMin": -12.0, "zMax": 12.0},
      "rotationRange": {"x": 0, "y": 360, "z": 0},
      "scaleRange": {"xMin": 0.8, "xMax": 1.2, "yMin": 0.8, "yMax": 1.2, "zMin": 0.8, "zMax": 1.2},
      "walkSpeed": 2.0,
      "restDuration": 4.0,
      "viewAngle": 90.0,
      "viewDistance": 8.0,
      "damageAmount": 15.0,
      "maxDamage": 75.0,
      "attackInterval": 3.0,
      "maxRestingSteps": 12,
      "maxSearchingSteps": 10,
      "padding": 3.0
    }
  ]
}
```

## Parameter Tuning for Research

For controlled reinforcement learning experiments, we recommend:

- **Easy Environment**: Low predator count (1-2), low speed (<2.0), short view distance (<8.0)
- **Medium Environment**: Moderate predator count (3-4), medium speed (2.0-3.0), medium view distance (8.0-12.0)
- **Hard Environment**: High predator count (5+), high speed (>3.0), large view distance (>12.0)

For curriculum learning, gradually increase predator count, speed, and sensory capabilities as agent performance improves. 