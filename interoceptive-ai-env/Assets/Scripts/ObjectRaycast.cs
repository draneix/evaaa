using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class ObjectRaycast : MonoBehaviour
{
    public GameObject agent;
    // public int hitCount = 0; // count the number ray of hit
    public RaycastHit hit;
    public float damage;

    public float[] collisionObservation;

    void Start()
    {
        collisionObservation = new float[10];
    }
    void Update()
    {
        // hitCount = 0;
        DetectObstacle();
        // agent.GetComponent<InteroceptiveAgent>().isCollisionDetected = false;

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
                collisionObservation[groupIndex] = 1; // 해당 그룹에서 충돌 감지시 1로 설정
            }
            else
            {
                Debug.DrawRay(transform.position, direction * agent.GetComponent<InteroceptiveAgent>().maxDistance, Color.green);
            }
            // Debug.Log("isCollisionDetected : " + agent.GetComponent<InteroceptiveAgent>().isCollisionDetected);
            // Debug.Log("damage : " + damage);
            // Debug.Log(agent.GetComponent<InteroceptiveAgent>().resourceLevels[3]);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Get the mass of the colliding object
        float mass = collision.rigidbody.mass;

        // Calculate damage using an adjusted exponential function of the mass
        agent.GetComponent<InteroceptiveAgent>().isCollisionDetected = true;
        damage = collision.impulse.magnitude * Mathf.Exp(0.01f * mass) * agent.GetComponent<InteroceptiveAgent>().damageConstant;
        // Debug.Log("damage : " + damage);
        // Debug.Log("mass : " + mass);
        // Debug.Log("collision.impulse.magnitude : " + collision.impulse.magnitude);
    }
    // void OnCollisionExit(Collision collision)
    // {
    //     // Reset the isCollisionDetected flag and damage
    //     agent.GetComponent<InteroceptiveAgent>().isCollisionDetected = false;
    //     // Debug.Log("Collision ended. isCollided : " + agent.GetComponent<InteroceptiveAgent>().isCollisionDetected);
    // }
}
