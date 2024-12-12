using System.IO;
// using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Assets.Scripts.Utility;

// Attach this script to the Sun GameObject (Directional Light)
public class DayAndNight : MonoBehaviour
{
    [Header("Configuration")]
    public string configFileName = "daynightConfig.json";
    // Reference to ML-Agents Environment Parameters
    private EnvironmentParameters m_ResetParams;

    [Header("Day/Night Settings")]
    public bool isNight = false;
    private bool previousIsNight = false;

    [Header("Sun Rotation")]
    public float eulerAnglesSun;
    public float daySpeed; // Default speed, can be overridden via EnvironmentParameters
    public float nightSpeed;

    [Header("Fog Settings")]
    public float fogChangeSpeed;
    public float dayFogDensity;
    public float nightFogDensity;
    private float currentFogDensity;

    [Header("Fog Color Settings")]
    [Tooltip("Color of the fog during the day.")]
    // public Color dayFogColor = new Color(0.0f, 0.0f, 0.0f, 1f);
    public ColorVector dayFogColor;

    [Tooltip("Color of the fog during the night.")]
    // public Color nightFogColor = new Color(0.0f, 0.0f, 0.0f, 1f);
    public ColorVector nightFogColor;
    private Color targetFogColor;

    [Header("ThermoGrid Settings")]
    public ThermoGridSpawner thermoGridSpawner; // Reference to the ThermoGridSpawner
    public HeatMap heatMap; // Reference to the HeatMap
    public float dayTemperatureChange;
    public float nightTemperatureChange;

    [Header("Camera Settings")]
    public Camera mainCamera;
    public float dayFarClip;
    public float nightFarClip;
    public float farClipTransitionSpeed;
    private float targetFarClip;


    private void Start()
    {
        string configFolderPath = Application.isEditor
            ? Path.Combine(Application.dataPath, "../Config")
            : Path.Combine(Directory.GetCurrentDirectory(), "Config");

        string configFilePath = Path.Combine(configFolderPath, configFileName);

        if (!File.Exists(configFilePath))
        {
            Debug.LogError($"Config file not found: {configFilePath}");
            return;
        }

        string json = File.ReadAllText(configFilePath);
        JsonUtility.FromJsonOverwrite(json, this);

        m_ResetParams = Academy.Instance.EnvironmentParameters;
        RenderSettings.fog = true;
        RenderSettings.fogDensity = currentFogDensity;
        RenderSettings.fogColor = dayFogColor.ToColor();

        // Check if thermoGridSpawner is assigned
        if (thermoGridSpawner == null)
        {
            Debug.LogError("ThermoGridSpawner is not assigned.");
        }

        // Check if heatMap is assigned
        if (heatMap == null)
        {
            Debug.LogError("HeatMap is not assigned.");
        }

        // Check if mainCamera is assigned
        if (mainCamera == null)
        {
            Debug.LogError("MainCamera is not assigned.");
        }
    }

    private void FixedUpdate()
    {
        eulerAnglesSun = transform.eulerAngles.x;

        // Determine if it's night based on Sun's angle
        bool currentIsNight = eulerAnglesSun >= 170 || eulerAnglesSun <= -10;

        if (currentIsNight != previousIsNight)
        {
            isNight = currentIsNight;
            UpdateTemperatureAndHeatMap();
            previousIsNight = currentIsNight;
        }

        // Rotate Sun
        transform.Rotate(Vector3.right, (isNight ? nightSpeed : daySpeed) * Time.deltaTime);

        // Adjust Fog Density and Color
        AdjustFog();

        // Adjust Camera View Distance
        AdjustCameraView();
    }

    private void AdjustFog()
    {
        targetFogColor = isNight ? nightFogColor.ToColor() : dayFogColor.ToColor();
        currentFogDensity = Mathf.Lerp(currentFogDensity, isNight ? nightFogDensity : dayFogDensity, fogChangeSpeed * Time.deltaTime);
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetFogColor, fogChangeSpeed * Time.deltaTime);
        RenderSettings.fogDensity = currentFogDensity;
    }

    private void AdjustCameraView()
    {
        if (mainCamera == null)
            return;

        if (isNight)
        {
            // Set target far clip for night
            targetFarClip = nightFarClip;
        }
        else
        {
            // Set target far clip for day
            targetFarClip = dayFarClip;
        }

        // Smoothly interpolate the far clip plane
        mainCamera.farClipPlane = Mathf.Lerp(mainCamera.farClipPlane, targetFarClip, Time.deltaTime * farClipTransitionSpeed);
    }

    /// <summary>
    /// Updates the temperature based on day/night state.
    /// </summary>
    private void UpdateTemperatureAndHeatMap()
    {
        if (thermoGridSpawner != null)
        {
            float temperatureChange = isNight ? nightTemperatureChange : dayTemperatureChange;
            thermoGridSpawner.AdjustTemperature(temperatureChange);
        }

        if (heatMap != null)
        {
            heatMap.SetDayNightTemperature(isNight);
        }
    }

    /// <summary>
    /// Returns whether it is currently night.
    /// </summary>
    public bool GetIsNight()
    {
        return isNight;
    }
}