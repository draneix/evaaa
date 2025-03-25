using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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
                    // prefabName = obj.name.Replace("(Clone)", "").Trim(),
                    prefabName = prefabName,
                    count = 1,
                    temperature = 0.0f,
                    position = new ObstaclePositionRange
                    {
                        xMin = obj.transform.localPosition.x,
                        xMax = obj.transform.localPosition.x,
                        yMin = obj.transform.localPosition.y,
                        yMax = obj.transform.localPosition.y,
                        zMin = obj.transform.localPosition.z,
                        zMax = obj.transform.localPosition.z
                    },
                    rotationRange = new ObstacleRotationRange
                    {
                        x = obj.transform.localEulerAngles.x,
                        y = obj.transform.localEulerAngles.y,
                        z = obj.transform.localEulerAngles.z
                    },
                    scaleRange = new ObstacleScaleRange
                    {
                        xMin = obj.transform.localScale.x,
                        xMax = obj.transform.localScale.x,
                        yMin = obj.transform.localScale.y,
                        yMax = obj.transform.localScale.y,
                        zMin = obj.transform.localScale.z,
                        zMax = obj.transform.localScale.z
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