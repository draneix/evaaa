using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

// Attach this script to the Sun GameObject (Directional Light)
public class DayAndNight : MonoBehaviour
{
    // Reference to ML-Agents Environment Parameters
    private EnvironmentParameters m_ResetParams;

    [Header("Day/Night Settings")]
    public bool isNight = false;

    [Header("Sun Rotation")]
    public float eulerAnglesSun;
    public float daySpeed = 10f; // Default speed, can be overridden via EnvironmentParameters
    public float nightSpeed = 30f;

    [Header("Fog Settings")]
    public float fogChangeSpeed = 0.1f;
    public float dayFogDensity = 0f;
    public float nightFogDensity = 0.1f;
    private float currentFogDensity = 0.01f;

    [Header("Fog Color Settings")]
    [Tooltip("Color of the fog during the day.")]
    public Color dayFogColor = new Color(0.0f, 0.0f, 0.0f, 1f);
    [Tooltip("Color of the fog during the night.")]
    public Color nightFogColor = new Color(0.0f, 0.0f, 0.0f, 1f);
    private Color targetFogColor;

    [Header("ThermoGrid Settings")]
    public ThermoGridSpawner thermoGridSpawner; // Reference to the ThermoGridSpawner
    public HeatMap heatMap; // Reference to the HeatMap
    public float dayTemperatureChange = 5f;
    public float nightTemperatureChange = -5f;

    [Header("Camera Settings")]
    public Camera mainCamera;
    public float dayFarClip = 1000f;
    public float nightFarClip = 500f;
    public float farClipTransitionSpeed = 1f;
    private float targetFarClip;

    private bool previousIsNight = false;

    private void Start()
    {
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        RenderSettings.fog = true;
        RenderSettings.fogDensity = currentFogDensity;
        RenderSettings.fogColor = dayFogColor;

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
        targetFogColor = isNight ? nightFogColor : dayFogColor;
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


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.MLAgents;

// // Attach this script to the Sun GameObject (Directional Light)
// public class DayAndNight : MonoBehaviour
// {
//     // Reference to ML-Agents Environment Parameters
//     private EnvironmentParameters m_ResetParams;
//     private bool previousIsNight = false;

//     [Header("Day/Night Settings")]
//     public bool isNight = false;

//     [Header("Sun Rotation")]
//     public float eulerAnglesSun;
//     public float daySpeed = 10f; // Default speed, can be overridden via EnvironmentParameters
//     public float nightSpeed = 30f;

//     [Header("Fog Settings")]
//     public float fogChangeSpeed = 0.1f;
//     public float dayFogDensity = 0f;
//     public float nightFogDensity = 0.1f;
//     private float currentFogDensity = 0.01f;

//     [Header("Fog Color Settings")]
//     [Tooltip("Color of the fog during the day.")]
//     public Color dayFogColor = new Color(0.0f, 0.0f, 0.0f, 1f);
//     [Tooltip("Color of the fog during the night.")]
//     public Color nightFogColor = new Color(0.0f, 0.0f, 0.0f, 1f);
//     private Color targetFogColor;

//     [Header("Camera View Settings")]
//     [Tooltip("Reference to the main camera.")]
//     public Camera mainCamera;

//     [Tooltip("Minimum far clip plane distance during night.")]
//     public float nightFarClip = 50f;

//     [Tooltip("Maximum far clip plane distance during day.")]
//     public float dayFarClip = 1000f;

//     [Tooltip("Speed at which the far clip plane transitions.")]
//     public float farClipTransitionSpeed = 5f;

//     // Initial Camera Settings
//     private float initialFarClip;
//     private float targetFarClip;

//     [Header("ThermoGrid Settings")]
//     public ThermoGridSpawner thermoGridSpawner; // Reference to the ThermoGridSpawner
//     public float dayTemperatureChange = 5f;
//     public float nightTemperatureChange = -5f;

//     void Awake()
//     {
//         Academy.Instance.OnEnvironmentReset += SetParameters;
//         eulerAnglesSun = transform.eulerAngles.x;
//     }

//     // Use this for initialization
//     void Start()
//     {
//         // Ensure fog is enabled
//         RenderSettings.fog = true;

//         // Initialize Fog Settings
//         RenderSettings.fogDensity = dayFogDensity;
//         RenderSettings.fogColor = dayFogColor;
//         currentFogDensity = dayFogDensity;
//         targetFogColor = dayFogColor;

//         // Initialize Camera Settings
//         if (mainCamera == null)
//         {
//             mainCamera = Camera.main;
//             if (mainCamera == null)
//             {
//                 Debug.LogError("Main Camera is not assigned and no Camera with tag 'MainCamera' found.");
//                 return;
//             }
//         }

//         initialFarClip = mainCamera.farClipPlane;
//         targetFarClip = initialFarClip;
//     }

//     void SetParameters()
//     {
//         // Setting parameters from ML-Agents' Python
//         m_ResetParams = Academy.Instance.EnvironmentParameters;
//         daySpeed = m_ResetParams.GetWithDefault("daySpeed", daySpeed);
//         nightSpeed = m_ResetParams.GetWithDefault("nightSpeed", nightSpeed);
//         fogChangeSpeed = m_ResetParams.GetWithDefault("fogChangeSpeed", fogChangeSpeed);

//         // Optional: Set initial Fog Densities if provided via EnvironmentParameters
//         // dayFogDensity = m_ResetParams.GetWithDefault("dayFogDensity", dayFogDensity);
//         // nightFogDensity = m_ResetParams.GetWithDefault("nightFogDensity", nightFogDensity);
//     }

//     void FixedUpdate()
//     {
//         eulerAnglesSun = transform.eulerAngles.x;

//         eulerAnglesSun = transform.eulerAngles.x;

//         // Determine if it's night based on Sun's angle
//         bool currentIsNight = eulerAnglesSun >= 170 || eulerAnglesSun <= -10;

//         if (currentIsNight != previousIsNight)
//         {
//             isNight = currentIsNight;
//             UpdateTemperatureAndHeatMap();
//             previousIsNight = currentIsNight;
//         }

//         // Rotate Sun
//         transform.Rotate(Vector3.right, (isNight ? nightSpeed : daySpeed) * Time.deltaTime);

//         // Adjust Fog Density and Color
//         AdjustFog();
//     }

//     /// <summary>
//     /// Adjusts the fog density and color based on day/night state.
//     /// </summary>
//     void AdjustFog()
//     {
//         targetFogColor = isNight ? nightFogColor : dayFogColor;
//         currentFogDensity = Mathf.Lerp(currentFogDensity, isNight ? nightFogDensity : dayFogDensity, fogChangeSpeed * Time.deltaTime);
//         RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetFogColor, fogChangeSpeed * Time.deltaTime);
//         RenderSettings.fogDensity = currentFogDensity;
//     }

//     /// <summary>
//     /// Adjusts the camera's far clip plane based on day/night state.
//     /// </summary>
//     void AdjustCameraView()
//     {
//         if (mainCamera == null)
//             return;

//         if (isNight)
//         {
//             // Set target far clip for night
//             targetFarClip = nightFarClip;
//         }
//         else
//         {
//             // Set target far clip for day
//             targetFarClip = dayFarClip;
//         }

//         // Smoothly interpolate the far clip plane
//         mainCamera.farClipPlane = Mathf.Lerp(mainCamera.farClipPlane, targetFarClip, Time.deltaTime * farClipTransitionSpeed);
//     }

//     /// <summary>
//     /// Updates the temperature based on day/night state.
//     /// </summary>
//     void UpdateTemperatureAndHeatMap()
//     {
//         if (thermoGridSpawner != null)
//         {
//             float temperatureChange = isNight ? nightTemperatureChange : dayTemperatureChange;
//             thermoGridSpawner.AdjustTemperature(temperatureChange);
//         }

//         if (heatMap != null)
//         {
//             heatMap.SetDayNightTemperature(isNight);
//         }

//     }

//     /// <summary>
//     /// Returns whether it is currently night.
//     /// </summary>
//     public bool GetIsNight()
//     {
//         return isNight;
//     }
// }