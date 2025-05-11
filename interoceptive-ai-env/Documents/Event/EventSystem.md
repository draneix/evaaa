# Event System Overview

The Event System in EVAAA provides a flexible mechanism for managing and triggering in-game events, enabling dynamic interactions between agents and the environment. It supports the creation of event-driven scenarios, resource changes, and message-based feedback, all configurable via external JSON files.

Below are the main components of the Event System. Each component is documented in detail in its own markdown file:

- [EventManager](./EventManager.md): Manages the configuration, instantiation, and handling of in-game events and triggers. Dynamically generates trigger zones and registers event handlers based on configuration files.
- [GameEventSystem](./GameEventSystem.md): Provides a static, centralized event management system for registering, triggering, and managing in-game events. Includes the `TriggerZone` component for collider-based event activation.

For details on each component, see the corresponding markdown file linked above.

## Main Components

### EventManager.cs
- **Purpose:** Manages the configuration, instantiation, and handling of in-game events and triggers.
- **Key Features:**
  - Loads event configurations from JSON files for flexible scenario design.
  - Dynamically generates trigger zones in the environment based on configuration.
  - Registers event handlers for different event types (e.g., resource changes, messages).
  - Handles event logic such as updating agent resources or displaying messages.
  - Supports resetting and reinitializing events during runtime.

### GameEventSystem.cs
- **Purpose:** Provides a static, centralized system for registering, triggering, and managing events and their handlers.
- **Key Features:**
  - Allows registration of event handlers for specific trigger tags, with optional limits on event counts.
  - Triggers all registered actions when a trigger event occurs.
  - Includes a `TriggerZone` component for detecting agent entry and invoking events.
  - Supports resetting event counts and clearing all handlers for experiment reproducibility.

---

The Event System enables modular, data-driven event management, supporting complex experimental designs and adaptive agent-environment interactions in EVAAA. 