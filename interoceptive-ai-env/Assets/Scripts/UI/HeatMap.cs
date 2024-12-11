using UnityEngine;
using UnityEngine.UI;

public class HeatMap : MonoBehaviour
{
    public Image heatMap;
    public GameObject agentTrack;
    public InteroceptiveAgent agent;

    public Gradient gradient;

    private Texture2D mapTexture;
    private ThermoGridSpawner thermoGridSpawner;

    private int numberOfCubeX;
    private int numberOfCubeZ;

    private bool isInitialized = false;

    // Call this explicitly to initialize the heatmap
    public void InitializeHeatMap()
    {
        if (isInitialized) return;

        // Find ThermoGridSpawner in the scene
        thermoGridSpawner = FindObjectOfType<ThermoGridSpawner>();
        if (thermoGridSpawner == null || !thermoGridSpawner.isThermalGridReady)
        {
            Debug.LogError("Thermal grid is not ready. Initialize the ThermoGridSpawner first.");
            return;
        }

        // Get grid dimensions from ThermoGridSpawner
        numberOfCubeX = thermoGridSpawner.NumberOfGridCubeX;
        numberOfCubeZ = thermoGridSpawner.NumberOfGridCubeZ;

        // Initialize the heatmap texture if thermal observations are enabled
        if (agent.useThermalObs)
        {
            mapTexture = new Texture2D(numberOfCubeX, numberOfCubeZ);
        }
        else
        {
            // Disable the heatmap and agent track if thermal observations are not used
            heatMap.enabled = false;
            agentTrack.GetComponent<Image>().enabled = false;
        }

        // Debug.Log("HeatMap initialized.");
        isInitialized = true;
    }

    public void EpisodeHeatMap()
    {
        if (!isInitialized)
        {
            Debug.LogError("HeatMap is not initialized. Call InitializeHeatMap() first.");
            return;
        }

        ModifyPixels();
    }

    private void ModifyPixels()
    {
        if (thermoGridSpawner == null || mapTexture == null)
        {
            Debug.LogError("ThermoGridSpawner or mapTexture is not initialized. Cannot modify pixels.");
            return;
        }

        // Update the texture pixels based on the thermal grid temperatures
        for (int z = 0; z < mapTexture.height; z++)
        {
            for (int x = 0; x < mapTexture.width; x++)
            {
                // Get normalized temperature from ThermoGridSpawner
                float normalizedTemp = thermoGridSpawner.GetNormalizedAreaTemp(x, z);
                Color color = gradient.Evaluate(1 - normalizedTemp);
                mapTexture.SetPixel(x, z, color);
            }
        }

        // Apply changes to the texture
        mapTexture.Apply();

        // Assign the updated texture to the heatmap material
        heatMap.material.mainTexture = mapTexture;
    }

    public void SetDayNightTemperature(bool isNight)
    {
        // Update the heatmap based on the day/night state
        ModifyPixels();
        Debug.Log("Modified HeatMap for " + (isNight ? "Night" : "Day"));
    }

    public bool IsThermalGridReady()
    {
        var thermoGridSpawner = FindObjectOfType<ThermoGridSpawner>();
        return thermoGridSpawner != null && thermoGridSpawner.isThermalGridReady;
    }
}