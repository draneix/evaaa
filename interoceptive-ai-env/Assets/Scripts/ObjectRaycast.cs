using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class ObjectRaycast : MonoBehaviour
{
    public GameObject agent;
    public LayerMask layermask;
    public int raysPerDirection = 100;
    public float maxDistance = 2;
    public float radialRange = 360f;

    public float impulseToDamageConversion = 0.1f;
    public float damageConstant = 1f;
    public int hitCount = 0; // count the number ray of hit
    public RaycastHit hit;


    void Update()
    {
        DetectObstacle();
    }

    void DetectObstacle()
    {    
        int agentLayer = LayerMask.NameToLayer("Player");
        int layerMask = ~(1 << agentLayer); // player 레이어만 제외하는 마스크 생성

        for (int i = 0; i < raysPerDirection; i++)
        {
            float angle = i * radialRange / (raysPerDirection - 1); 
            Quaternion rotation = Quaternion.Euler(0f, angle - radialRange / 2, 0f); 
            Vector3 direction = rotation * transform.forward;

            
            if (Physics.Raycast(transform.position, direction, out hit, maxDistance, layerMask))
            {
                
                Debug.DrawRay(transform.position, direction * maxDistance, Color.red);
                // agent.GetComponent<InteroceptiveAgent>().isObjectDetected = true;
                // Debug.Log("Detected obstacle: " + hit.collider.name + " at distance: " + hit.distance);

                hitCount++; // Increment the count for hit ray
            }
            else
            {
                Debug.DrawRay(transform.position, direction * maxDistance, Color.green);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
        {
            if (layermask == (layermask | (1 << collision.gameObject.layer)))
            {
                // float damage = collision.impulse.magnitude * impulseToDamageConversion;
                float damage = hitCount * damageConstant;    // total damage = num of ray hit x damage constant

                // collision.impulse : OnCollisionEnter 함수에 전달되는 Collision 객체의 속성으로, 충돌 중에 오브젝트에 작용한 총 충격량을 벡터로 나타냄.
                // impulse는 힘과 그 힘이 작용하는 시간의 곱으로 정의되며, 운동량의 변화량을 나타냄. 
                // collision.impulse에서 .magnitude를 호출하면, 벡터의 크기(길이)를 계산하여 반환함. 이 크기는 임펄스 벡터의 모든 방향 성분을 고려한 결과로, 즉, 충돌에 의한 순수한 충격량의 크기를 의미함

                agent.GetComponent<InteroceptiveAgent>().resourceLevels[3] -= damage;
                Debug.Log("damage" + damage);
                Debug.Log("Agent Health: " + agent.GetComponent<InteroceptiveAgent>().resourceLevels[3]);

                if (agent.GetComponent<InteroceptiveAgent>().resourceLevels[3] <= 0)
                {
                    Debug.Log("Agent has been destroyed!");
                    gameObject.SetActive(false);
                }
            }
        }   
}