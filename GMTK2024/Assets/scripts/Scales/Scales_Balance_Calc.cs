using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scales_Balance_Calc : MonoBehaviour
{
    public float currentWeightLeft;
    public float currentWeightRight;
    

    public float springStrength = 10f;
    public float damping = 1f;

    private Rigidbody2D rb;
    private float initialRotation;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialRotation = transform.rotation.eulerAngles.z;
    }

    void FixedUpdate()
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        float angleDifference = Mathf.DeltaAngle(currentRotation, initialRotation);
        float torque = -angleDifference * springStrength - rb.angularVelocity * damping;
        rb.AddTorque(torque);
    }

    public void AddWeight(float weight, SpawnTestWeights.SpawnPlace spawnPlace )
    {
        if (spawnPlace == SpawnTestWeights.SpawnPlace.Left)
            currentWeightLeft += weight;
        else if (spawnPlace == SpawnTestWeights.SpawnPlace.Right)
            currentWeightRight += weight;
    }
}
