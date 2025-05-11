# DataRecorder.cs

## Purpose
The `DataRecorder` class provides comprehensive experiment data recording and export functionality for EVAAA. It tracks agent state, actions, rewards, collisions, events, and more at both step and episode levels, supporting reproducibility and analysis of experiments.

## Key Features
- Records detailed step-level and episode-level metrics for each experiment run.
- Tracks agent state variables, actions, rewards, collisions, resource consumption, and events.
- Exports data to CSV files for analysis and reproducibility.
- Supports flexible configuration and output directory management via main config.
- Integrates with agent, environment, and event systems for complete experiment tracking.
- Provides methods for initializing, recording, exporting, and resetting data.

## Configuration Options
- `experimentType`: Type of experiment (set in Inspector).
- `isActive`: Whether recording is enabled.
- `targetAgent`: Reference to the agent being tracked.
- Output directory, file name prefix, and recording enable flag are set via `MainConfig` and `ExperimentData`.

## Main Methods
- `Initialize(InteroceptiveAgent agent)`: Sets up the recorder and output directories.
- `InitializeEpisode()`: Prepares data structures for a new episode.
- `RecordStep()`, `RecordStep(string action)`: Records a step of agent-environment interaction.
- `RecordFinalStep()`, `RecordFinalStep(string action)`: Records the final step of an episode.
- `RecordCollision()`, `RecordFoodConsumed()`, `RecordWaterConsumed()`, `RecordEvent(string eventType)`: Specialized event and state recorders.
- `ExportEpisodeSummary()`: Exports episode-level summary data to CSV.
- `CalculateFinalMetrics()`: Computes summary statistics for an episode.
- `SetResourceOffered(string resource)`, `RecordResourceChoice(string resourceChosen)`: Tracks resource choice events.
- `OnEpisodeBegin()`, `OnEpisodeEnd()`: Episode lifecycle hooks.
- `SetEpisodeEndType(string endType)`: Records the reason for episode termination.

## Integration Notes
- Attach the `DataRecorder` script to a GameObject in the Unity scene.
- Assign the agent reference and configure experiment settings in the Inspector or config files.
- Works in conjunction with agent, environment, and event systems for complete experiment tracking.
- Output directory and file naming are managed via the main configuration file.

## Example Usage
- Attach `DataRecorder` to a GameObject in the scene.
- Assign the agent reference and configure experiment settings.
- Call `Initialize()` at runtime to set up recording.
- Use the provided methods to record steps, events, and export data during simulation.

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/Utility/DataRecorder.cs`. 