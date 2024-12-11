// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// // Detect the area of spotlight on the ground, then set the area as the heatObject
// public class SpotlightHotzone : MonoBehaviour
// {
//     public GameObject heatObject;
//     // For object detection
//     public RaycastHit hit;
//     public LayerMask groundLayer;
//     // For spotlight
//     public Light spotlight;
//     // For temperature implementation
//     private float spotlightTemp;
//     [SerializeField]
//     private float spotlightMaxTemp;
//     [SerializeField]
//     private float spotlightMinTemp;

//     private FieldThermoGrid fieldThermoGrid;

//     void Start()
//     {
//         spotlight = GetComponent<Light>();
//         fieldThermoGrid = FindObjectOfType<FieldThermoGrid>();

//         if (fieldThermoGrid != null)
//         {
//             // Access the heatGridCubes through the getter method
//             List<FieldThermoGrid.HeatGridCube> heatGridCubes = fieldThermoGrid.GetHeatGridCubes();
//             //Debug.Log("Number of HeatGridCubes: " + heatGridCubes.Count);
//         }

//     }

//     // MAIN: Handle entire spotlight hotzone process
//     public void ApplySpotlightHotzone()
//     {
//         Vector3 spotlightPosition = spotlight.transform.position;
//         Vector3 spotlightDirection = spotlight.transform.forward;
        
//         // Perform raycast once to get hit point and distance
//         if (Physics.Raycast(spotlightPosition, spotlightDirection, out hit, Mathf.Infinity, groundLayer))
//         {
//             Vector3 coneCenter = hit.point;
//             float distanceToHit = Vector3.Distance(spotlightPosition, coneCenter);

//             // Calculate the spotlight cone radius at the hit point
//             float coneRadius = Mathf.Tan(Mathf.Deg2Rad * spotlight.spotAngle / 2f) * distanceToHit;

//             // Match heatObject's position and scale to spotlight's projection
//             SetHeatObjectTrnasform(coneCenter, coneRadius);

//             // Update the temperature of the heat grid cubes based on the spotlight's projection
//             UpdateSpotTemperature(coneCenter, coneRadius, spotlightMaxTemp, spotlightMinTemp);
//         }
//         else
//         {
//             Debug.Log("No ground detected within the spotlight.");
//         }
//     }

//     // Set heatObject's position and scale to spotlight's projection
//     private void SetHeatObjectTrnasform(Vector3 coneCenter, float coneRadius)
//     {
//         // Set heatObject's position based on spotlight
//         heatObject.transform.position = coneCenter;
//         Debug.Log($"Spotlight {heatObject} Position: {heatObject.transform.position}");

//         // Update the heatObject's scale to match the spotlight's projection
//         float diameter = 2f * coneRadius;  // Diameter of the cone's circular projection
//         heatObject.transform.localScale = new Vector3(diameter, 1f, diameter); // Scale in X and Z based on diameter, Y can be a fixed value
//         Debug.Log($"Spotlight {heatObject} Scale: {heatObject.transform.localScale}");
//     }

//     // Set heatObject's temperature based on the distance from the center of the circle
//     private void UpdateSpotTemperature(Vector3 coneCenter, float coneRadius, float maxTemp, float minTemp)
//     {
//         if (fieldThermoGrid == null) return;

//         List<FieldThermoGrid.HeatGridCube> heatGridCubes = fieldThermoGrid.GetHeatGridCubes();

//         // Loop through all the grid cubes in the FieldThermGrid
//         foreach (var heatGridCube in heatGridCubes)
//         {
//             Vector3 gridCubePosition = heatGridCube.gridCube.transform.position;

//             // Calculate the distance from the grid cube to the spotlight's cone center
//             float distanceFromCenter = Vector3.Distance(coneCenter, gridCubePosition);

//             // Check if the grid cube is within the spotlight's projection (inside the cone radius)
//             if (distanceFromCenter <= coneRadius)
//             {
//                 // Normalize the distance (0 = center, 1 = edge of the spotlight projection)
//                 float normalizedDistance = distanceFromCenter / coneRadius;

//                 // Interpolate the temperature from maxTemp (center) to minTemp (edge)
//                 float newTemp = Mathf.Lerp(maxTemp, minTemp, normalizedDistance);

//                 // Update the temperature for this grid cube in the FieldThermoGrid
//                 fieldThermoGrid.SetGridTemp((int)heatGridCube.arrayIndex.x, (int)heatGridCube.arrayIndex.y, newTemp);
//                 //Debug.Log($"{heatObject} GridCube ({heatGridCube.arrayIndex.x}, {heatGridCube.arrayIndex.y}) - Position: {gridCubePosition} - Distance from Center: {distanceFromCenter} - Temp: {newTemp}");
//             }
//         }
//     }
// }