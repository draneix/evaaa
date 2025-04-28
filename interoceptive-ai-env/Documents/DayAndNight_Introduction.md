# DayAndNight

## Description

The `DayAndNight` component implements temporal dynamics in the environment, providing cyclical changes to lighting, temperature, atmospheric conditions, and camera properties. This system is designed to be attached to a Sun (Directional Light) GameObject and enables research on agent adaptation to day-night cycles, as described in our paper "EVAAA: Embodied Virtual Agents with Artificial Autonomic Systems" (NeurIPS 2025).

The system divides the day into five distinct phases (`Day`, `Sunset`, `Night`, `DeepNight`, `Dawn`), each with its own environmental characteristics, creating a rich temporal landscape for agent learning.

## Research Context

Temporal dynamics are crucial for studying agent adaptation and planning in changing environments. The day-night cycle serves multiple research purposes:

- Introducing non-stationarity into the environment state
- Creating cyclical changes in optimal policies (day vs. night strategies)
- Enabling research on anticipatory behaviors and temporal planning
- Modeling realistic environmental periodicities that biological agents navigate
- Testing generalization across different environmental conditions

Our implementation enables precise control over cycle parameters, allowing researchers to study how temporal dynamics affect learning and behavioral adaptation.

## Implementation Details

The day-night system provides several key features relevant to reinforcement learning research:

- **Sun Rotation**: Continuous sun movement across the sky drives the cycle
- **Phase-Based States**: Five distinct phases (`Day`, `Sunset`, `Night`, `DeepNight`, `Dawn`) with unique properties
- **Temperature Modulation**: Time-dependent environmental temperature variations with 24-hour cycles
- **Visual Changes**: Dynamic lighting, skybox transitions, and fog density changes
- **Non-Linear Transitions**: Customizable exponent-based transitions between phases for realistic dusk/dawn
- **Camera Far Clip Adjustment**: Dynamic modification of camera view distance based on time of day
- **Step-Based Progression**: Configurable step sizes for fine control of cycle speed

## Research Implications

Day-night configuration directly impacts several aspects of reinforcement learning experiments:

1. **Policy Complexity**: Agents must learn condition-dependent policies based on time of day
2. **Temporal Planning**: Anticipating temperature changes requires forward planning
3. **Visual Adaptation**: Changing visual conditions test robustness of vision-based systems
4. **Thermodynamic Challenges**: Temperature variations create time-dependent thermal regulation demands

## Configuration Parameters

The day-night system is configured via a JSON file with the following key parameters:

| Parameter | Description | Research Implications |
|-----------|-------------|----------------------|
| `fogChangeSpeed` | Speed of fog density changes | Controls abruptness of visibility transitions |
| `dayFogDensity` | Fog density during day | Affects vision-based observation quality during day |
| `nightFogDensity` | Fog density during night | Affects vision-based observation quality during night |
| `dayTemperatureChange` | Temperature adjustment during day | Sets amplitude of thermal regulation challenge during day |
| `nightTemperatureChange` | Temperature adjustment during night | Sets amplitude of thermal regulation challenge during night |
| `dayFarClip` | Camera far clipping plane distance during day | Controls maximum visible distance during day |
| `nightFarClip` | Camera far clipping plane distance during night | Controls maximum visible distance during night |
| `farClipTransitionSpeed` | Speed of far clip plane adjustments | Controls abruptness of visibility distance changes |
| `randomSunAngle` | Whether to randomize initial sun position | Tests agent adaptability to different starting conditions |
| `rotationIntervalMultiplier` | Multiplier for rotation intervals (1=360, 2=720, etc.) | Controls granularity of sun movement |
| `rotationSpeedSteps` | Steps advanced per update | Controls speed of day-night cycle progression |
| `temperatureUpdateSteps` | Number of steps in temperature cycle (typical: 24) | Maps to hours of day for temperature changes |
| `fogExponent` | Exponent for non-linear fog transitions during Night phase | Controls curve of fog density transitions (higher = more sudden) |
| `sunsetExponent` | Exponent for Sunset phase transitions | Controls curve of transitions during Sunset (higher = more sudden) |
| `dawnExponent` | Exponent for Dawn phase transitions | Controls curve of transitions during Dawn (higher = more sudden) |

## Integration with Benchmark Tasks

The `DayAndNight` component integrates with other environmental systems to create research scenarios:

1. **Temporal Thermal Regulation**: Agents must adapt thermal regulation strategies to time of day
2. **Day-Night Resource Management**: Different resource gathering strategies for day vs. night
3. **Predator Avoidance with Temporal Dynamics**: Predator behaviors that vary by time of day
4. **Visual Adaptation**: Navigation and object recognition under varying light conditions

## Example Configuration

```json
{
  "fogChangeSpeed": 0.1,
  "dayFogDensity": 0.0,
  "nightFogDensity": 0.1,
  "dayTemperatureChange": 15,
  "nightTemperatureChange": -10,
  "dayFarClip": 1000,
  "nightFarClip": 500,
  "farClipTransitionSpeed": 5,
  "randomSunAngle": true,
  "rotationIntervalMultiplier": 30,
  "rotationSpeedSteps": 15,
  "temperatureUpdateSteps": 24,
  "fogExponent": 2.0
}
```

## Temporal Dynamics Visualization

### Daily Cycle Overview

The day-night cycle spans a full 24-hour period, divided into five distinct phases that affect environmental parameters differently:

```
Hour:  14 15 16 17 18 19 20 21 22 23  0  1  2  3  4  5  6  7  8  9 10 11 12 13
Phase: D  D  D  D  S  S  S  N  N  N  N  N  N  DN Dn Dn Dn D  D  D  D  D  D  D
      |------Day------|---Sunset---|------Night------|DN|--Dawn--|------Day------|

Legend: D = `Day`, S = `Sunset`, N = `Night`, DN = `DeepNight`, Dn = `Dawn`
```

### Temperature Variation (Actual Data)

The following chart shows the actual temperature offset values per hour, as extracted from the Unity Console:

```
Hour:   14   15   16   17   18   19   20   21   22   23    0    1    2    3    4    5    6    7    8    9   10   11   12   13
Temp: 11.88 10.31  8.75  7.19  5.63  4.06  2.50  0.94 -0.63 -2.19 -3.75 -5.31 -6.88 -8.44 -10.0 -6.88 -3.75 -0.63  2.50  5.63  8.75 11.88 15.00 13.44
```

### Fog Density Transitions (Actual Data)

The following chart shows the actual fog density values per hour:

```
Hour:   14   15   16   17   18   19   20   21   22   23    0    1    2    3    4    5    6    7    8    9   10   11   12   13
Fog:  0.000 0.000 0.000 0.000 0.000 0.019 0.054 0.050 0.053 0.060 0.068 0.077 0.088 0.100 0.100 0.054 0.019 0.000 0.000 0.000 0.000 0.000 0.000 0.000
```

### Camera Far Clip Transitions (Actual Data)

The following chart shows the actual far clip values per hour:

```
Hour:   14    15    16    17    18    19    20    21    22    23     0     1     2     3     4     5     6     7     8     9    10    11    12    13
Clip: 1000  1000  1000  1000  1000  903.8 727.8 750.0 733.0 701.9 661.6 613.9 559.8 500.0 500.0 727.8 903.8 1000  1000  1000 1000  1000  1000  1000
```

### Phase Transition Examples

Based on actual system data, here are the observed transitions between phases:

1. **`Day` Phase (Hours 5-17)**:
   - Temperature Offset: Peaks around +11.88°C at Hour 14, gradually decreases
   - Fog Density: Constant at 0.00000
   - Far Clip: Constant at 1000.00 units

2. **`Sunset` Transition (Hours 18-20)**:
   - Temperature Offset: Decreases from +5.63°C at Hour 18 to +4.06°C at Hour 19
   - Fog Density: Begins increasing from 0.00000 at Hour 18 to 0.01925 at Hour 19
   - Far Clip: Begins decreasing from 1000.00 at Hour 18 to 903.78 at Hour 19

3. **`Night` Phase (Hours 21-2)**:
   - Temperature: Continues decreasing toward minimum values
   - Fog Density: Reaches medium-high values
   - Far Clip: Continues decreasing toward night minimum

4. **`DeepNight` Phase (Hour 3)**:
   - Temperature: Reaches minimum value (coldest point)
   - Fog Density: Maximum density
   - Far Clip: Minimum distance (most limited visibility)

5. **`Dawn` Transition (Hours 4-6)**:
   - Temperature: Begins rising from minimum values
   - Fog Density: Decreases from night to day levels
   - Far Clip: Increases from night to day values

The transitions follow non-linear curves controlled by the exponent parameters in the configuration, creating smooth and realistic changes between environmental states.

## Parameter Tuning for Research

For controlled reinforcement learning experiments, we recommend:

- **Learning Focus**: For initial training, use slower cycles (low `rotationSpeedSteps` value) to give agents time to adapt
- **Testing Adaptation**: For testing adaptability, use faster cycles (high `rotationSpeedSteps` value) to stress response times
- **Temperature Studies**: Vary `dayTemperatureChange` and `nightTemperatureChange` to create different thermal regulation challenges:
  - Mild: Small difference between day and night (±5 units)
  - Moderate: Medium difference between day and night (±10-15 units)
  - Extreme: Large difference between day and night (±20+ units)
- **Visual Learning**: Modify fog densities and far clip planes to create different visual observation challenges

For curriculum learning, start with mild temperature variations and static conditions, then gradually introduce stronger day-night differences. 