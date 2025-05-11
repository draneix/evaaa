# EventManager.cs

## Purpose
The `EventManager` class manages the configuration, instantiation, and handling of in-game events and triggers in EVAAA. It enables dynamic, event-driven scenarios by generating trigger zones and registering event handlers based on external JSON configuration files.

## Key Features
- Loads event configuration from JSON files for flexible scenario design.
- Dynamically generates trigger zones in the environment based on configuration.
- Registers event handlers for different event types (e.g., resource change, message feedback).
- Supports both resource-based and message-based events.
- Integrates with agent state, data recording, and environment systems.
- Provides runtime reset and reinitialization of events.

## Configuration Options
- `configFileName`: Name of the event configuration JSON file (default: `eventConfig.json`).
- Event groups: Each group specifies trigger tag, target tag, resource values, message, position, rotation, scale, and max count.

## Main Methods
- `InitializeEventManager(ConfigLoader loader)`: Loads configuration and sets up triggers and handlers.
- `GenerateTriggers()`: Instantiates trigger zones in the environment.
- `RegisterEventHandlers()`: Registers event handlers for each event group.
- `HandleMessageEvent(GameObject obj)`: Handles message-based events.
- `HandleResourceEvent(GameObject obj)`: Handles resource-based events (e.g., food, water changes).
- `ResetEventManager()`: Resets and reinitializes all events and triggers.
- `ClearTriggers()`: Removes all dynamically generated triggers.
- `FixedUpdate()`: Allows runtime reset of event counts via keyboard input.

## Integration Notes
- Designed to work with the `GameEventSystem` for event registration and triggering.
- Trigger zones are implemented using the `GameEventSystem.TriggerZone` component.
- Integrates with agent scripts (e.g., `InteroceptiveAgent`) and data recording for event logging.
- Configuration is managed via JSON files in the `Config/` directory.

## Example Usage
- Attach the `EventManager` script to a GameObject in the Unity scene.
- Provide a valid event configuration JSON file in the `Config/` directory.
- Call `InitializeEventManager` with a reference to the `ConfigLoader` at runtime.
- The system will automatically generate triggers and register event handlers.

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/Event/EventManager.cs`. 