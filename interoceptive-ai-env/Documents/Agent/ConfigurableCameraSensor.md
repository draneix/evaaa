# ConfigurableCameraSensor.cs

## Purpose
The `ConfigurableCameraSensor` class provides a flexible, attachable camera sensor for agents in EVAAA. It enables visual observation input for reinforcement learning, supporting a range of camera configurations and integration with Unity ML-Agents.

## Key Features
- Attachable to any agent GameObject for visual perception.
- Configurable resolution, grayscale mode, and image compression.
- Supports both RGB and grayscale image output.
- Automatically attaches or creates a camera for agent vision.
- Integrates with Unity ML-Agents for visual observation spaces.
- Adjustable field of view and camera orientation.

## Configuration Options
- `width`, `height`: Output image resolution.
- `grayscale`: Toggle for grayscale or RGB output.
- `compressionType`: Image compression method (e.g., PNG, JPEG).
- `fieldOfView`: Camera field of view angle.
- `cameraOffset`: Position and rotation offset relative to the agent.
- `sensorName`: Name for the ML-Agents sensor.

## Main Methods
- `InitializeCameraSensor()`: Sets up the camera and sensor parameters.
- `GetObservation()`: Captures and returns the current camera image.
- `UpdateCameraSettings()`: Dynamically updates camera parameters during runtime.
- `OnDrawGizmos()`: Visualizes camera frustum in the Unity Editor for debugging.

## Integration Notes
- Designed to be attached to agent GameObjects and referenced by the main agent script.
- Works seamlessly with Unity ML-Agents for visual observation spaces.
- Camera settings can be configured via the Unity Inspector or agent config files.

## Example Usage
- Attach the `ConfigurableCameraSensor` script to an agent GameObject.
- Configure camera parameters via the Unity Inspector or config files.
- The script will automatically provide visual observations during simulation and training.

---

For more details on configuration fields and advanced usage, see the code comments in `Assets/Scripts/Agent/ConfigurableCameraSensor.cs`. 