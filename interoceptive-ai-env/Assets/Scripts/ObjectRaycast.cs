using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class ObjectRaycast : MonoBehaviour
{
    public GameObject agent;
    public RaycastHit hit;
    public float damage;
    private float impulseMagnitude;

    public float[] collisionObservation;

    void Start()
    {
        collisionObservation = new float[10];
    }
    void FixedUpdate()
    {
        DetectObstacle();
    }

    void DetectObstacle()
    {    
        int agentLayer = LayerMask.NameToLayer("Player");
        int layerMask = ~(1 << agentLayer);

        for (int j = 0; j < collisionObservation.Length; j++)
        {
            collisionObservation[j] = 0;
        }

        int raysPerGroup = 100 / 10; // 각 그룹당 레이의 개수
        
        for (int i = 0; i < 100; i++)
        {
            int groupIndex = i / raysPerGroup; // 현재 레이가 속한 그룹 인덱스
            float angle = i * 360f / 100; // 360도를 100개의 벡터로 나눔
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 direction = rotation * transform.forward;

            if (Physics.Raycast(transform.position, direction, out hit, agent.GetComponent<InteroceptiveAgent>().maxDistance, layerMask))
            {
                Debug.DrawRay(transform.position, direction * agent.GetComponent<InteroceptiveAgent>().maxDistance, Color.red);
                collisionObservation[groupIndex] = 1 + impulseMagnitude; // 해당 그룹에서 충돌 감지시 1로 설정
            }
            else
            {
                Debug.DrawRay(transform.position, direction * agent.GetComponent<InteroceptiveAgent>().maxDistance, Color.green);
            }
        }
        // string collisionObservationString = string.Join(", ", collisionObservation);
        // Debug.Log("collisionObservation : " + collisionObservationString);
        // Debug.Log("Observation : " + agent.GetComponent<InteroceptiveAgent>().collisionObservation);
    }

    void OnCollisionStay(Collision collision)
    {   
        int agentLayer = LayerMask.NameToLayer("Court");
        if (collision.gameObject.layer == agentLayer)
        {
            return;
        }
        impulseMagnitude = collision.impulse.magnitude;
        // agent.GetComponent<InteroceptiveAgent>().isCollisionDetected = true;
        damage = Mathf.Exp(0.07f * (impulseMagnitude-60)) * agent.GetComponent<InteroceptiveAgent>().damageConstant;
        // agent.GetComponent<InteroceptiveAgent>().ApplyDamage(damage);
    }

    void OnCollisionExit(Collision collision)
    {
        damage = 0;
    }
}
