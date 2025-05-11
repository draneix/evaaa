# GameEventSystem.cs

## Purpose
The `GameEventSystem` class provides a static, centralized event management system for EVAAA. It enables registration, triggering, and management of in-game events, supporting flexible event-driven interactions between agents, triggers, and the environment.

## Key Features
- Static event registration and triggering by tag.
- Supports multiple event handlers per tag.
- Optional maximum trigger count for each event type.
- Integrated `TriggerZone` component for collider-based event activation.
- Runtime reset and clearing of all event handlers and counts.
- Simple API for registering, triggering, and managing events.

## Configuration Options
- **Event tags:** String identifiers for each event type (e.g., `"resource"`, `"message"`).
- **Max count:** Optional limit on the number of times an event can be triggered.

## Main Methods
- `Register(string triggerTag, Action<GameObject> action, int maxCount = -1)`: Registers an event handler for a tag.
- `Trigger(string triggerTag, GameObject invoker)`: Triggers all handlers for a tag.
- `ResetCount(string triggerTag)`: Resets the trigger count for a tag.
- `GetCount(string triggerTag)`: Returns the current trigger count for a tag.
- `ClearAllEventHandlers()`: Removes all registered event handlers and resets counts.

### TriggerZone Component
- `SetTriggerTag(string tag)`: Sets the trigger tag for the zone.
- `SetTargetTag(string tag)`: Sets the target tag for objects that can activate the zone.
- `OnTriggerEnter(Collider other)`: Triggers the event when a target-tagged object enters the zone.

## Integration Notes
- Used by `EventManager` to manage and trigger in-game events.
- `TriggerZone` is attached to dynamically generated GameObjects for event activation.
- Designed for extensibility and integration with agent and environment systems.

## Example Usage
- Register an event handler:
  ```csharp
  GameEventSystem.Register("resource", HandleResourceEvent, 5);
  ```
- Trigger an event (usually via `TriggerZone`):
  ```csharp
  GameEventSystem.Trigger("resource", agentGameObject);
  ```
- Attach `TriggerZone` to a GameObject and set tags for event activation.

---

For more details on usage and advanced integration, see the code comments in `Assets/Scripts/Event/GameEventSystem.cs`. 