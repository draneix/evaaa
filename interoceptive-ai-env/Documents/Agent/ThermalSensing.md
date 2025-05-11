# ThermalSensing.cs

## Purpose
The `ThermalSensing` class enables agents in EVAAA to sense and respond to local environmental temperature. It provides agents with thermal perception by reading from a spatial temperature grid, supporting adaptive behavior in dynamic environments.

## Key Features
- Reads temperature values from a thermal grid based on the agent's position.
- Provides thermal data as part of the agent's observation vector.
- Supports dynamic adaptation to spatial and temporal temperature gradients.
- Integrates with the agent's internal state and reward system.
- Configurable sensing parameters for flexible experiments.

## Configuration Options
- `thermalGrid`: Reference to the environment's thermal grid object.
- `sensingRadius`: Radius around the agent for sampling temperature.
- `samplingFrequency`: How often to update thermal readings.
- `useThermalSensing`: Enable or disable thermal sensing for the agent.

## Main Methods
- `InitializeThermalSensing()`: Sets up references and parameters for thermal sensing.
- `GetLocalTemperature()`: Reads and returns the current temperature at the agent's position.
- `CollectThermalObservations()`: Adds thermal data to the agent's observation vector.

## Integration Notes
- Designed to be attached to agent GameObjects and referenced by the main agent script.
- Works in conjunction with the environment's thermal grid system.
- Thermal data can be used for reward shaping and adaptive behavior.

## Example Usage
- Attach the `ThermalSensing` script to an agent GameObject.
- Assign the thermal grid reference and configure sensing parameters via the Unity Inspector or config files.
- The script will automatically provide thermal data during simulation.

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/Agent/ThermalSensing.cs`. 