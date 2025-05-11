# Predator.cs

## Purpose
The `Predator` class implements a dynamic predator agent in EVAAA. Predators act as moving threats in the environment, challenging the learning agent to develop adaptive and evasive behaviors. The predator uses state-based logic and Unity's NavMesh for navigation.

## Key Features
- State machine with four behaviors: Resting, Searching, Chasing, Attacking.
- Uses Unity NavMesh for pathfinding and movement.
- Field of view and detection logic for identifying agents.
- Applies damage to agents upon successful attack.
- Interacts with the day/night cycle and environmental landmarks.
- Configurable movement, attack, and timing parameters.

## Configuration Options
- Movement: `walkSpeed`, `turnSpeed`
- Field of View: `viewAngle`, `viewDistance`, `targetMask`
- Damage: `damageAmount`, `maxDamage`, `attackInterval`
- State Timing: `maxRestingSteps`, `maxSearchingSteps`, `searchingActionInterval`
- Integration with `DayAndNight` and `LandmarkSpawner` components

## Main Methods
- `InitializePredator()`: Initializes the predator and its state machine.
- `InitializeNavMesh()`: Sets up NavMesh navigation and activates movement.
- `SetDayAndNight(DayAndNight dayAndNightSystem)`: Integrates with the day/night cycle.
- `TakeAction()`: Main update loop for state transitions and behavior.
- `ChangeState(PredatorState newState)`: Switches between predator states.
- `ApplyDamage()`: Applies damage to the agent when attacking.
- `IsInsideLandmarkArea()`: Checks if the predator is within a defined area.

## Integration Notes
- Predators are spawned and managed by the environment system (see `PredatorSpawner`).
- The predator's behavior is influenced by environmental cues (e.g., time of day, landmarks).
- Works in conjunction with the agent's collision and health systems.

## Example Usage
- Attach the `Predator` script to a GameObject with a NavMeshAgent component.
- Configure parameters via the Unity Inspector or environment config files.
- The predator will automatically initialize and begin acting when the simulation starts.

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/Agent/Predator.cs`. 