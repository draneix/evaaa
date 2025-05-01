using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Utility;

public class LandmarkSpawner : MonoBehaviour
{
    [Header("Landmark Settings")]
    public GameObject landmarkPrefab; // Optional: assign a prefab, or leave null for empty GameObject
    public Vector3 regionMin = new Vector3(-15, 0, -15);
    public Vector3 regionMax = new Vector3(15, 0, 15);
    public float gridSpacing = 10f;
    public float landmarkRadius = 1.5f; // For overlap checking
    public float overlapPadding = 2.0f;
    public LayerMask staticObjectLayerMask; // Assign in inspector to include static objects/resources

    private List<GameObject> spawnedLandmarks = new List<GameObject>();

    public void GenerateLandmarks()
    {
        GameObject overlapCheckPrefab = landmarkPrefab;
        // If no prefab, use a temporary primitive for overlap checking
        if (overlapCheckPrefab == null)
        {
            overlapCheckPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            overlapCheckPrefab.transform.localScale = Vector3.one * landmarkRadius * 2f;
            overlapCheckPrefab.GetComponent<Collider>().isTrigger = true;
            overlapCheckPrefab.SetActive(false); // Hide from scene
        }

        for (float x = regionMin.x; x <= regionMax.x; x += gridSpacing)
        {
            for (float z = regionMin.z; z <= regionMax.z; z += gridSpacing)
            {
                Vector3 pos = new Vector3(x, regionMin.y, z);
                // Overlap check ONLY for static objects/resources
                Collider[] staticOverlaps = Physics.OverlapSphere(pos, landmarkRadius + overlapPadding, staticObjectLayerMask);
                if (staticOverlaps.Length > 0)
                {
                    // Skip this landmark, as a static object/resource is here
                    continue;
                }
                if (!OverlapUtility.IsOverlapping(
                        pos,
                        overlapCheckPrefab,
                        overlapCheckPrefab.transform.localScale,
                        1.0f,
                        overlapPadding))
                {
                    GameObject landmark = landmarkPrefab ?
                        Instantiate(landmarkPrefab, pos, Quaternion.identity, transform) :
                        new GameObject($"Landmark_{x}_{z}");
                    landmark.transform.position = pos;
                    landmark.transform.parent = transform;
                    // Add a temporary collider for overlap checking (optional, for obstacle phase)
                    var col = landmark.AddComponent<SphereCollider>();
                    col.radius = landmarkRadius;
                    col.isTrigger = true;
                    spawnedLandmarks.Add(landmark);
                }
            }
        }

        // Clean up the temporary primitive if used
        if (landmarkPrefab == null && overlapCheckPrefab != null)
        {
            DestroyImmediate(overlapCheckPrefab);
        }
    }

    public void RemoveLandmarkColliders()
    {
        foreach (var landmark in spawnedLandmarks)
        {
            var col = landmark.GetComponent<Collider>();
            if (col) Destroy(col);
        }
    }

    public List<GameObject> GetLandmarks() => spawnedLandmarks;
} 