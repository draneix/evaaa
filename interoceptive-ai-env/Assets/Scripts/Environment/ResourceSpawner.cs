using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Utility;

[System.Serializable]
public class ResourceGroup
{
    public string prefabName;
    public string prefabLabel;
    public int count;
    public PositionRange position;
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

    private ConfigLoader configLoader; // Reference to ConfigLoader

    public void InitializeResourceSpawner(ConfigLoader loader)
    {
        configLoader = loader;
        if (configLoader == null)
        {
            Debug.LogError("ConfigLoader is not set. Ensure ConfigLoader is initialized.");
            return;
        }

        LoadConfig();
    }

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
        if (configLoader == null)
        {
            Debug.LogError("ConfigLoader is not set. Ensure ConfigLoader is initialized.");
            return;
        }

        resourceConfig = configLoader.LoadConfig<ResourceConfig>(configFileName);

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

        if (string.IsNullOrEmpty(group.prefabLabel))
        {
            group.prefabLabel = group.prefabName;
            // Debug.LogWarning($"Prefab label not found for {group.prefabName}. Using prefab name as label.");
        }
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found: {group.prefabName}");
            return;
        }

        for (int i = 0; i < group.count; i++)
        {
            Vector3 position = RandomPosition(group.position);
            Quaternion rotation = RandomRotation(group.rotationRange);
            Vector3 scale = RandomScale(group.scaleRange);

            GameObject resource = Instantiate(prefab, position, rotation);
            // Change name of resource
            resource.name = $"{group.prefabLabel}(Clone)";

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

    private Vector3 RandomPosition(PositionRange position) =>
        new Vector3(
            Random.Range(position.xMin, position.xMax),
            Random.Range(position.yMin, position.yMax),
            Random.Range(position.zMin, position.zMax)
        );

    private Quaternion RandomRotation(RotationRange rotationRange) =>
        Quaternion.Euler(
            rotationRange.x,
            Random.Range(rotationRange.y, rotationRange.y),
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
                
        if (resource.CompareTag("pond"))
        {
            // Debug.Log($"Resource {resourceName} will not be relocated.");
            return;
        }

        ResourceGroup group = resourceConfig.groups.Find(g => g.prefabLabel == resourceName);
        if (group == null)
        {
            Debug.LogError($"Resource {resourceName} configuration not found.");
            return;
        }

        Vector3 newPosition = RandomPosition(group.position);
        resource.transform.position = newPosition;

        ResourceProperty resourceProperty = resource.GetComponent<ResourceProperty>();
        if (resourceProperty != null)
        {
            resourceProperty.InitializeProperties();
        }
    }
}
