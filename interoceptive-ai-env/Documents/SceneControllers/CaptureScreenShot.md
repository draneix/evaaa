# CaptureScreenShot.cs

## Purpose
The `CaptureScreenShot` class handles automated screenshot capture and recording during simulation runs in EVAAA. It enables visual documentation and analysis of agent behavior and environment states, based on experiment configuration settings.

## Key Features
- Captures and saves screenshots at specified intervals or events during simulation.
- Configures output directories and recording options from the main configuration file.
- Supports toggling recording on/off via configuration.
- Integrates with Unity ML-Agents and experiment reset events.
- Automatically creates output directories for recordings.

## Configuration Options
- `recordEnable`: Whether screenshot recording is enabled (set via main config).
- `recordingFolderName`: Name of the output folder for screenshots (set via main config).
- `mediaOutputFolder`: Full path to the output directory (set automatically).

## Main Methods
- `Initialize()`: Loads configuration, sets up output directories, and enables recording if configured.
- `CreateRecordDirectory()`: Creates the output directory for screenshots if it does not exist.
- `CaptureImage()`: Captures and saves a screenshot to the output directory.
- `Awake()`: Subscribes to environment reset events for parameter updates.

## Integration Notes
- Designed to be attached to a GameObject in the Unity scene.
- Used by `MasterInitializer` and other systems for automated visual recording.
- Output directory and recording options are set via the main configuration file (`mainConfig.json`).
- Works on both Windows and macOS builds.

## Example Usage
- Attach the `CaptureScreenShot` script to a GameObject in the Unity scene.
- Ensure the main configuration file specifies recording options.
- Call `Initialize()` at runtime to set up recording.
- Call `CaptureImage()` to save a screenshot during simulation (e.g., from agent actions or events).

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/SceneController/CaptureScreenShot.cs`. 