using UnityEngine;

public class AgentTrackBlackDot : MonoBehaviour
{
    public GameObject agent; // The agent whose position we are tracking
    public CourtSpawner courtSpawner; // Reference to the CourtSpawner
    public RectTransform heatMapRect; // The RectTransform of the HeatMap UI element
    public RectTransform blackDotRect; // The RectTransform of the black dot representing the agent

    private void Awake()
    {
        if (agent == null)
        {
            Debug.LogError("Agent is not assigned.");
        }

        if (courtSpawner == null)
        {
            Debug.LogError("CourtSpawner is not assigned.");
        }

        if (heatMapRect == null)
        {
            Debug.LogError("HeatMap RectTransform is not assigned.");
        }

        if (blackDotRect == null)
        {
            Debug.LogError("BlackDot RectTransform is not assigned.");
        }
    }

    private void FixedUpdate()
    {
        if (agent == null || courtSpawner == null || heatMapRect == null || blackDotRect == null)
        {
            return;
        }

        // Get the agent's relative position within the court
        Vector3 relativePosition = GetAgentRelativePosition();
        // Debug.Log($"Agent's relative position: {relativePosition}");

        // Move the black dot based on the agent's relative position
        MoveBlackDot(relativePosition);
    }

    private Vector3 GetAgentRelativePosition()
    {
        // Get the court floor's position and size
        Transform courtFloorTransform = courtSpawner.CourtFloorTransform;
        if (courtFloorTransform == null)
        {
            Debug.LogError("Court floor transform is not found.");
            return Vector3.zero;
        }

        Vector3 courtPosition = courtFloorTransform.position;
        Vector3 courtSize = new Vector3(courtSpawner.courtConfig.floorSize.x, 1, courtSpawner.courtConfig.floorSize.z);

        // Calculate the agent's relative position within the court
        Vector3 agentPosition = agent.transform.position;
        Vector3 relativePosition = new Vector3(
            (agentPosition.x - courtPosition.x) / courtSize.x,
            (agentPosition.y - courtPosition.y) / courtSize.y,
            (agentPosition.z - courtPosition.z) / courtSize.z
        );

        return relativePosition;
    }

    private void MoveBlackDot(Vector3 relativePosition)
    {

        Vector2 localPosition = new Vector2(heatMapRect.anchoredPosition.x, heatMapRect.anchoredPosition.y);
        Vector2 relativePosition2D = new Vector2(relativePosition.x * heatMapRect.rect.width, relativePosition.z* heatMapRect.rect.height);

        // Debug.Log($"heatMapRect's anchoredPosition position: {localPosition}");
        // Debug.Log($"agent's relativePosition2D position: {relativePosition2D}");

        // Update the position of the black dot on the HeatMap
        blackDotRect.anchoredPosition = localPosition + relativePosition2D;
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // Canvas - AgentTrack 오브젝트 (검정색 점을 의미함)에 부착하는 스크립트
// public class AgentTrack : MonoBehaviour
// {
//     public Camera cam;
//     public GameObject agent;
//     public GameObject canvas;
//     RectTransform canvasRect;
//     Vector2 viewportPosition;
//     Vector2 screenPosition;

//     // 3차원의 scene에서 우측 하단에 2차원의 HeatMap (그래픽 인터페이스)을 넣고
//     // 상호작용 (agent가 움직이면 미니맵 상의 검정색 점이 움직임)을 하기 위해 HeatMap 오브젝트의 RectTransform 컴포넌트를 가져옴
//     // https://blog.naver.com/pxkey/221558646854
//     private void Awake()
//     {
//         canvasRect = canvas.GetComponent<RectTransform>();
//     }

//     // void Start()
//     // {
//     //     viewportPosition = cam.WorldToViewportPoint(agent.transform.position);
//     //     screenPosition = new Vector2(((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f) - 30), ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) + 30));
//     //     GetComponent<RectTransform>().anchoredPosition = screenPosition;
//     // }

//     // void Update()
//     void FixedUpdate()
//     {
//         // agent의 scene 상에서의 좌표 (transform)를 cam (TopViewCamera) 상에서의 좌표로 변환
//         // https://fiftiesstudy.tistory.com/254
//         viewportPosition = cam.WorldToViewportPoint(agent.transform.position);

//         // cam 상에서의 좌표를 screen 상에서의 좌표로 변환
//         // 변환 과정을 구체적으로 이해하고 구현한 것이 아니라
//         // 조금씩 수정해가면서 하드코딩식으로 구현해서 추후 screen의 해상도가 바뀌면 버그가 생길 수 있음
//         // 단순히 TopViewCamera가 보여주는 화면을 그대로 출력하는 것이 아니라 HeatMap 상에서
//         // agent (검정색 점)가 움직이는 것을 구현해야 해서 이렇게 구현하였음
//         // https://answers.unity.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
//         screenPosition = new Vector2(((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f) - 30), ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) + 90));

//         // 위에서 구한 screen 상에서의 좌표를 AgentTrack 스크립트가 부착할 게임 오브젝터 (검정색 점)의 좌표에 넣어줌
//         GetComponent<RectTransform>().anchoredPosition = screenPosition;
//     }
// }
