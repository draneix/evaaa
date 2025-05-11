# ObjectRaycast.cs

## Purpose
The `ObjectRaycast` class provides collision and obstacle detection for agents in EVAAA using a radial raycasting system. It enables agents to perceive their immediate surroundings, detect obstacles, and gather collision-based observations for learning and navigation.

## Key Features
- Casts multiple rays in a radial pattern around the agent to detect obstacles and objects.
- Collects detailed collision and proximity data for use in agent observations.
- Calculates and applies damage to the agent based on collision impulse.
- Supports customizable ray count, length, and detection layers.
- Integrates with the agent's internal state and reward system.

## Configuration Options
- `rayCount`: Number of rays to cast around the agent.
- `rayLength`: Maximum distance for each ray.
- `detectionLayerMask`: Layer mask for objects to detect.
- `collisionDamageThreshold`: Minimum impulse required to apply damage.
- `damageCoefficient`: Scales the amount of damage applied on collision.

## Main Methods
- `InitializeRaycast()`: Sets up raycasting parameters and references.
- `CollectRaycastObservations()`: Casts rays and collects data for agent observations.
- `OnCollisionEnter(Collision collision)`: Handles collision events and applies damage if necessary.
- `DrawRays()`: (Optional) Visualizes rays in the Unity Editor for debugging.

## Integration Notes
- Designed to be attached to agent GameObjects and referenced by the main agent script.
- Works in conjunction with the agent's health and reward systems.
- Raycast data can be included in the agent's observation vector for learning.

## Example Usage
- Attach the `ObjectRaycast` script to an agent GameObject.
- Configure raycasting parameters via the Unity Inspector or agent config files.
- The script will automatically collect and provide collision data during simulation.

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/Agent/ObjectRaycast.cs`. 