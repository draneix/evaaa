# ObstacleCollector.cs

## Purpose
The `ObstacleCollector` class automates the collection and export of obstacle configuration data from the Unity scene in EVAAA. It scans the scene for obstacles, extracts their properties, and generates a structured JSON configuration file for reproducible environment setup.

## Key Features
- Scans the scene for obstacle GameObjects and collects their position, rotation, scale, and other properties.
- Supports filtering obstacles by name and customizing padding and decimal precision.
- Exports obstacle configuration as a formatted JSON file for use in procedural generation or experiment reproducibility.
- Collapses position, rotation, and scale objects to single lines for compact JSON output.
- Integrates with Unity Inspector for easy configuration and execution.

## Configuration Options
- `outputFileName`: Name of the generated JSON file (default: `generatedObstacleConfig.json`).
- `prefabFolder`: Folder containing obstacle prefabs.
- `obstacleNameFilter`: Filter string for selecting obstacles by name.
- `defaultPadding`: Default padding value for obstacles.
- `decimalPlaces`: Number of decimal places for exported values.

## Main Methods
- `CollectObstacles()`: Scans the scene, collects obstacle data, and writes the configuration to a JSON file.
- `FormatFloat(float value)`: Formats float values to the specified decimal places.

## Integration Notes
- Attach the `ObstacleCollector` script to a GameObject in the Unity scene.
- Configure options in the Inspector as needed.
- Call `CollectObstacles()` to generate the obstacle configuration file.
- The generated JSON can be used for reproducible environment setup in future experiments.

## Example Usage
- Attach `ObstacleCollector` to a GameObject in the scene.
- Configure output file name, filter, and other options in the Inspector.
- Call `CollectObstacles()` to export the current obstacle configuration.

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/Utility/ObstacleCollector.cs`. 