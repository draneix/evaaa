# ResourceEating.cs

## Purpose
The `ResourceEating` class manages the agent's interaction with consumable resources in EVAAA, such as food, water, and pond objects. It detects when the agent is in range to consume resources, updates internal state variables, and handles resource respawn logic.

## Key Features
- Detects proximity to consumable resources (food, water, pond).
- Updates the agent's internal state (food, water, health) upon consumption.
- Triggers resource respawn and updates resource availability.
- Supports flexible resource types and consumption rules.
- Integrates with the agent's reward and data recording systems.

## Configuration Options
- `resourceTypes`: List of resource types the agent can consume.
- `consumptionRadius`: Distance threshold for consuming a resource.
- `consumptionAmount`: Amount of resource gained per consumption event.
- `respawnDelay`: Time before a consumed resource respawns.
- `enableResourceRespawn`: Toggle for resource respawn behavior.

## Main Methods
- `InitializeResourceEating()`: Sets up resource detection and consumption parameters.
- `OnTriggerEnter(Collider other)`: Detects when the agent enters a resource's trigger zone.
- `ConsumeResource(GameObject resource)`: Handles the logic for consuming a resource and updating state.
- `RespawnResource(GameObject resource)`: Manages resource respawn timing and placement.

## Integration Notes
- Designed to be attached to agent GameObjects and referenced by the main agent script.
- Works in conjunction with environment resource spawners and the agent's internal state system.
- Resource consumption events can be logged by the data recorder for analysis.

## Example Usage
- Attach the `ResourceEating` script to an agent GameObject.
- Configure resource types and parameters via the Unity Inspector or config files.
- The script will automatically handle resource detection and consumption during simulation.

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/Agent/ResourceEating.cs`. 