using System;
using Unity.Mathematics;
using UnityEngine;
namespace Unity.MLAgents.Areas
[DefaultExecutionOrder(-5)]
public class TrainingAreaReplicator : MonoBehaviour
{
    public GameObject baseArea;
    public int numAreas = 1;
    public float separation = 10f;
    public bool buildOnly = true;
    int3 m_GridSize = new(1, 1, 1);
    int m_AreaCount = 0;
    string m_TrainingAreaName;
    public int3 GridSize => m_GridSize;
    public string TrainingAreaName => m_TrainingAreaName;
    public void Awake()
    {
            ComputeGridSize();
            m_TrainingAreaName = baseArea.name;
        }
        public void OnEnable()
        {
            if (buildOnly)
            {
#if UNITY_STANDALONE && !UNITY_EDITOR
                AddEnvironments();
#endif
                return;
            }
            AddEnvironments();
        }
        void ComputeGridSize()
        {
            if (Academy.Instance.Communicator != null)
                numAreas = Academy.Instance.NumAreas;
            var rootNumAreas = Mathf.Pow(numAreas, 1.0f / 3.0f);
            m_GridSize.x = Mathf.CeilToInt(rootNumAreas);
            m_GridSize.y = Mathf.CeilToInt(rootNumAreas);
            m_GridSize.z = Mathf.CeilToInt((float)numAreas / (m_GridSize.x * m_GridSize.y));
        }
        void AddEnvironments()
        {
            if (numAreas > m_GridSize.x * m_GridSize.y * m_GridSize.z)
            {
                throw new UnityAgentsException("The number of training areas that you have specified exceeds the size of the grid.");
            }
            for (int z = 0; z < m_GridSize.z; z++)
            {
                for (int y = 0; y < m_GridSize.y; y++)
                {
                    for (int x = 0; x < m_GridSize.x; x++)
                    {
                        if (m_AreaCount < numAreas)
                        {
                            if (m_AreaCount == 0)
                            {
                                m_AreaCount++;
                            else
                            {
                                m_AreaCount++;
                                var area = Instantiate(baseArea, new Vector3(x * separation, y * separation, z * separation), Quaternion.identity);
                                area.name = $"{m_TrainingAreaName}_{m_AreaCount}";
                            }
                        }
                    }
                }
            }
        }
    }
