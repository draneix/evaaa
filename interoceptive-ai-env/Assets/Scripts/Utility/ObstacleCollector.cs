using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;

[System.Serializable]
public class ObstaclePositionRange
{
    public float xMin, xMax;
    public float yMin, yMax;
    public float zMin, zMax;
}

[System.Serializable]
public class ObstacleRotationRange
{
    public float x, y, z;
}

[System.Serializable]
public class ObstacleScaleRange
{
    public float xMin, xMax;
    public float yMin, yMax;
    public float zMin, zMax;
}

[System.Serializable]
public class CollectedObstacleGroup
{
    public string prefabName;
    public int count;
    public float temperature;
    public float padding;
    public ObstaclePositionRange position;
    public ObstacleRotationRange rotationRange;
    public ObstacleScaleRange scaleRange;
}

[System.Serializable]
public class CollectedObstacleConfig
{
    public List<CollectedObstacleGroup> groups = new List<CollectedObstacleGroup>();
}

public class ObstacleCollector : MonoBehaviour
{
    public string outputFileName = "generatedObstacleConfig.json";
    public string prefabFolder = "Obstacles";
    public string obstacleNameFilter = ""; // New field for obstacle name filter
    public float defaultPadding = 2.0f; // Default padding value
    [Range(0, 6)]
    public int decimalPlaces = 2; // Number of decimal places to maintain

    private float RoundToDecimalPlaces(float value)
    {
        return (float)Math.Round(value, decimalPlaces);
    }

    public void CollectObstacles()
    {
        CollectedObstacleConfig config = new CollectedObstacleConfig();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (string.IsNullOrEmpty(obstacleNameFilter) || obj.name.Contains(obstacleNameFilter))
            {
                string prefabName = Regex.Replace(obj.name, @"\(Clone\).*", "").Trim();

                CollectedObstacleGroup group = new CollectedObstacleGroup
                {
                    prefabName = prefabName,
                    count = 1,
                    temperature = RoundToDecimalPlaces(0.0f),
                    padding = RoundToDecimalPlaces(defaultPadding),
                    position = new ObstaclePositionRange
                    {
                        xMin = RoundToDecimalPlaces(obj.transform.localPosition.x),
                        xMax = RoundToDecimalPlaces(obj.transform.localPosition.x),
                        yMin = RoundToDecimalPlaces(obj.transform.localPosition.y),
                        yMax = RoundToDecimalPlaces(obj.transform.localPosition.y),
                        zMin = RoundToDecimalPlaces(obj.transform.localPosition.z),
                        zMax = RoundToDecimalPlaces(obj.transform.localPosition.z)
                    },
                    rotationRange = new ObstacleRotationRange
                    {
                        x = RoundToDecimalPlaces(obj.transform.localEulerAngles.x),
                        y = RoundToDecimalPlaces(obj.transform.localEulerAngles.y),
                        z = RoundToDecimalPlaces(obj.transform.localEulerAngles.z)
                    },
                    scaleRange = new ObstacleScaleRange
                    {
                        xMin = RoundToDecimalPlaces(obj.transform.localScale.x),
                        xMax = RoundToDecimalPlaces(obj.transform.localScale.x),
                        yMin = RoundToDecimalPlaces(obj.transform.localScale.y),
                        yMax = RoundToDecimalPlaces(obj.transform.localScale.y),
                        zMin = RoundToDecimalPlaces(obj.transform.localScale.z),
                        zMax = RoundToDecimalPlaces(obj.transform.localScale.z)
                    }
                };
                config.groups.Add(group);
            }
        }

        string json = JsonUtility.ToJson(config, true);
        string outputPath = Path.Combine(Application.dataPath, outputFileName);
        File.WriteAllText(outputPath, json);
        Debug.Log($"Obstacle configuration saved to {outputPath}");
    }
}