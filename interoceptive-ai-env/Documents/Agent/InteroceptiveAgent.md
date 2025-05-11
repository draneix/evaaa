# InteroceptiveAgent.cs

## Purpose
The `InteroceptiveAgent` class implements the primary reinforcement learning agent in EVAAA. It models an embodied agent with internal physiological variables (essential variables: food, water, thermal, health) and supports multimodal perception (vision, olfaction, thermal, collision, touch). The agent interacts with the environment, collects observations, takes actions, and receives rewards based on internal state regulation.

## Key Features
- Loads agent configuration from JSON files for flexible experiments.
- Handles agent initialization, observation collection, action execution, and reward calculation.
- Manages internal state dynamics and updates essential variables based on agent actions and environment feedback.
- Supports both AI-controlled and manual control modes.
- Interfaces with data recording and environment configuration systems.
- Integrates with Unity ML-Agents for training and evaluation.

## Configuration Options
- `configFileName`: Name of the agent configuration JSON file (default: `agentConfig.json`).
- `isAIControlled`: Whether the agent is controlled by AI or manually.
- Essential variable ranges and coefficients (food, water, thermal, health).
- Observation and action settings (e.g., use of olfaction, thermal sensing, collision detection).
- Movement and rotation parameters.

## Main Methods
- `InitializeAgent(ConfigLoader loader)`: Initializes the agent with configuration and data recorder.
- `OnEpisodeBegin()`: Resets the agent and environment at the start of each episode.
- `CollectObservations(VectorSensor sensor)`: Collects multimodal observations for the agent.
- `OnActionReceived(ActionBuffers actions)`: Executes actions and updates internal state based on agent policy.
- `Heuristic(in ActionBuffers actionsOut)`: Provides manual control for testing.
- `FoodUpdate`, `WaterUpdate`, `ThermoUpdate`, `HealthUpdate`: Update methods for each essential variable.
- `CalculateReward()`: Computes the reward based on homeostatic deviation.

## Integration Notes
- The agent is designed to be highly configurable via JSON files in the `Config/` directory.
- Works in conjunction with other components such as `ResourceEating`, `ObjectRaycast`, and `ThermalSensing` for perception and interaction.
- Data recording and experiment tracking are handled via the `DataRecorder` component.

## Example Usage
- To use the agent, ensure it is attached to a GameObject in the Unity scene and properly configured via the inspector and JSON files.
- The agent will automatically load its configuration and begin interacting with the environment when the simulation starts.

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/Agent/InteroceptiveAgent.cs`. 