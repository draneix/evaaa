# ConfigLoader.cs

## Purpose
The `ConfigLoader` class loads and manages experiment configuration files in EVAAA. It enables flexible switching between training levels, experimental testbeds, and custom scenarios by reading structured JSON files from the `Config/` directory.

## Key Features
- Loads the main configuration file (`mainConfig.json`) and sets the active experiment folder.
- Supports platform-specific paths for cross-platform compatibility.
- Loads additional configuration files for agents, environment, events, etc., from the selected experiment folder.
- Provides methods to retrieve configuration data and file paths for other systems.
- Integrates with Unity Inspector for easy assignment and debugging.

## Configuration Options
- `mainConfigFileName`: Name of the main configuration JSON file (default: `mainConfig.json`).
- `configFolderPath`: Path to the active experiment configuration folder (set automatically).
- `mainConfig`: Deserialized object containing global experiment settings (AI control, recording, data output, etc.).

## Main Methods
- `InitializeConfigLoader()`: Loads the main configuration and sets the active experiment folder.
- `LoadMainConfig()`: Reads and parses the main configuration JSON file.
- `SetConfigFolder(string folderName)`: Sets the path to the experiment folder.
- `LoadConfig<T>(string configFileName)`: Loads and deserializes a configuration file of type `T` from the active folder.
- `GetFullPath(string configFileName)`: Returns the full path to a configuration file in the active folder.

## Integration Notes
- Designed to be attached to a GameObject in the Unity scene.
- Used by `MasterInitializer` and other systems to load and manage experiment settings.
- All configuration files should be placed in the `Config/` directory at the project root.
- Supports both Windows and macOS builds.

## Example Usage
- Attach the `ConfigLoader` script to a GameObject in the Unity scene.
- Assign the main configuration file name in the Inspector (if different from default).
- Call `InitializeConfigLoader()` at runtime to load experiment settings.
- Use `LoadConfig<T>()` to load additional configuration files as needed.

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/SceneController/ConfigLoader.cs`. 