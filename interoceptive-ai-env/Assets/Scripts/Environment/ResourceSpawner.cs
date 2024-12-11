// using SpawnerUtilities;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Utility;

[System.Serializable]
public class ResourceGroup
{
    public string prefabName;
    public int count;
    public AreaRange area;
    public RotationRange rotationRange;
    public ScaleRange scaleRange;
}

[System.Serializable]
public class ResourceConfig
{
    public List<ResourceGroup> groups;
}

public class ResourceSpawner : MonoBehaviour
{
    [Header("Configuration")]
    public string configFileName = "resourceConfig.json";
    public string prefabFolder = "Resources";

    private ResourceConfig resourceConfig = new ResourceConfig(); // Parsed resource configuration
    private List<GameObject> spawnedResources = new List<GameObject>(); // Tracks generated resources
    private Transform courtTransform = null; // Reference to the court object for parenting resources

    public void ReloadConfig()
    {
        LoadConfig();
    }

    public void InitializeResources(Transform court)
    {
        if (resourceConfig == null)
        {
            Debug.LogError("Resource configuration is not loaded. Call ReloadConfig() before InitializeResources().");
            return;
        }

        courtTransform = court;
        GenerateResources();
    }

    private void LoadConfig()
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

        string jsonContent = File.ReadAllText(configFilePath);
        resourceConfig = JsonUtility.FromJson<ResourceConfig>(jsonContent);

        if (resourceConfig == null || resourceConfig.groups == null)
        {
            Debug.LogError("Invalid or empty resource configuration.");
        }
    }

    public void ResetResources()
    {
        ClearResources();
        GenerateResources();
    }

    private void GenerateResources()
    {
        if (resourceConfig == null || resourceConfig.groups == null)
        {
            Debug.LogError("Resource configuration is not loaded.");
            return;
        }

        foreach (var group in resourceConfig.groups)
        {
            SpawnResourceGroup(group);
        }
    }

    private void ClearResources()
    {
        foreach (var resource in spawnedResources)
        {
            if (resource != null) Destroy(resource);
        }
        spawnedResources.Clear();
    }

    private void SpawnResourceGroup(ResourceGroup group)
    {
        GameObject prefab = Resources.Load<GameObject>($"{prefabFolder}/{group.prefabName}");
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found: {group.prefabName}");
            return;
        }

        for (int i = 0; i < group.count; i++)
        {
            Vector3 position = RandomPosition(group.area);
            Quaternion rotation = RandomRotation(group.rotationRange);
            Vector3 scale = RandomScale(group.scaleRange);

            GameObject resource = Instantiate(prefab, position, rotation);

            if (courtTransform != null)
            {
                resource.transform.SetParent(courtTransform);
            }

            resource.transform.localScale = scale;

            ResourceProperty resourceProperty = resource.GetComponentInChildren<ResourceProperty>();
            if (resourceProperty != null)
            {
                resourceProperty.InitializeProperties();
            }

            spawnedResources.Add(resource);
        }
    }

    private Vector3 RandomPosition(AreaRange area) =>
        new Vector3(
            Random.Range(area.xMin, area.xMax),
            Random.Range(area.yMin, area.yMax),
            Random.Range(area.zMin, area.zMax)
        );

    private Quaternion RandomRotation(RotationRange rotationRange) =>
        Quaternion.Euler(
            rotationRange.x,
            Random.Range(0, rotationRange.y),
            rotationRange.z
        );

    private Vector3 RandomScale(ScaleRange scaleRange) =>
        new Vector3(
            Random.Range(scaleRange.xMin, scaleRange.xMax),
            Random.Range(scaleRange.yMin, scaleRange.yMax),
            Random.Range(scaleRange.zMin, scaleRange.zMax)
        );

    public void RelocateResource(Collider resource)
    {
        string resourceName = resource.name.Replace("(Clone)", "").Trim();
        ResourceGroup group = resourceConfig.groups.Find(g => g.prefabName == resourceName);
        if (group == null)
        {
            Debug.LogError($"Resource {resourceName} configuration not found.");
            return;
        }

        Vector3 newPosition = RandomPosition(group.area);
        resource.transform.position = newPosition;

        ResourceProperty resourceProperty = resource.GetComponent<ResourceProperty>();
        if (resourceProperty != null)
        {
            resourceProperty.InitializeProperties();
        }
    }
}
