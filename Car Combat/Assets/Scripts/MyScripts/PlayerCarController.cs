using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{

    private Rigidbody rb;
    private float targetSteerAngle = 0;

    public bool isBraking = false;
    public Vector3 centerOfMass;


    [Header("Wheel Colliders")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    [Header("Drive values")]
    public float maxSteerAngle = 45f;
    public float maxMotorTorque = 500f;
    public float maxBrakeTorque = 800f;
    public float currentSpeed;
    public float maxSpeed = 50f;
    public float turnSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Drive();
        ApplySteer();
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        float acceleration = Input.GetAxis("Vertical");

        if (currentSpeed < maxSpeed)
        {
            wheelFL.motorTorque = maxMotorTorque * acceleration;
            wheelFR.motorTorque = maxMotorTorque * acceleration;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
    }

    private void ApplySteer()
    {
        float turn = Input.GetAxis("Horizontal");
        float newSteer = maxSteerAngle * turn;
        targetSteerAngle = newSteer;
        LerpToSteerAngle();
    }

    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.fixedDeltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.fixedDeltaTime * turnSpeed);
    }
}
