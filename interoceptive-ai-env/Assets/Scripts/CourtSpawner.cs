using UnityEngine;
using System.IO;

[System.Serializable]
public class CourtConfig
{
    public Vector3 floorSize;    // Size of the floor (X, Y, Z)
    public float wallHeight;     // Height of the walls
    public Vector3 position;     // Position of the Court
}

public class CourtSpawner : MonoBehaviour
{
    public string configFileName = "courtConfig.json";
    public Material floorMaterial;
    public Material wallMaterial;

    private CourtConfig courtConfig; // Declare courtConfig here
    private GameObject courtObject; // Store the generated court

    public Transform CourtTransform => courtObject != null ? courtObject.transform : null;

    void Start()
    {
        LoadConfig();
        GenerateCourt();
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
        courtConfig = JsonUtility.FromJson<CourtConfig>(jsonContent);

        if (courtConfig == null)
        {
            Debug.LogError("Invalid court configuration.");
        }
    }

    public void ReloadConfig()
    {
        LoadConfig();
        GenerateCourt();
        Debug.Log("Court configuration reloaded.");
    }

    private void GenerateCourt()
    {
        if (courtObject != null) Destroy(courtObject);

        courtObject = new GameObject("Court");
        courtObject.transform.position = courtConfig.position;

        CreateFloor(courtObject.transform);
        CreateWalls(courtObject.transform);
    }

    private void CreateFloor(Transform parent)
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Court_Floor";
        floor.transform.parent = parent;
        floor.transform.localScale = new Vector3(courtConfig.floorSize.x, 1, courtConfig.floorSize.z);

        if (floorMaterial != null)
        {
            floor.GetComponent<Renderer>().material = floorMaterial;
        }
    }

    private void CreateWalls(Transform parent)
    {
        float halfX = courtConfig.floorSize.x / 2;
        float halfZ = courtConfig.floorSize.z / 2;
        float wallThickness = 1;

        Vector3[] wallPositions =
        {
            new Vector3(0, courtConfig.wallHeight / 2, halfZ + wallThickness / 2),
            new Vector3(0, courtConfig.wallHeight / 2, -(halfZ + wallThickness / 2)),
            new Vector3(halfX + wallThickness / 2, courtConfig.wallHeight / 2, 0),
            new Vector3(-(halfX + wallThickness / 2), courtConfig.wallHeight / 2, 0)
        };

        Vector3[] wallSizes =
        {
            new Vector3(courtConfig.floorSize.x, courtConfig.wallHeight, wallThickness),
            new Vector3(courtConfig.floorSize.x, courtConfig.wallHeight, wallThickness),
            new Vector3(wallThickness, courtConfig.wallHeight, courtConfig.floorSize.z),
            new Vector3(wallThickness, courtConfig.wallHeight, courtConfig.floorSize.z)
        };

        string[] wallNames = { "Wall_North", "Wall_South", "Wall_East", "Wall_West" };

        for (int i = 0; i < 4; i++)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = $"Court_{wallNames[i]}";
            wall.transform.parent = parent;
            wall.transform.localPosition = wallPositions[i];
            wall.transform.localScale = wallSizes[i];

            if (wallMaterial != null)
            {
                wall.GetComponent<Renderer>().material = wallMaterial;
            }
        }
    }
}
