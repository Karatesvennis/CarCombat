using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{

    private Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;
    private Rigidbody rb;
    private bool avoiding = false;
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
    public float maxSpeed = 100f;
    public float turnSpeed = 10f;

    [Header("Sensors")]
    public Transform frontCenterSensor;
    public Transform frontLeftSensor;
    public Transform frontRightSensor;
    public Transform leftAngledSensor;
    public Transform rightAngledSensor;
    public float angleSensorLength = 5f;
    public float frontSensorLength = 10f;
    public float frontSensorAngle = 10f;
    public float sideSensorAngle = 45f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        path = FindObjectOfType<Path>().transform;
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Sensors();
        Drive();
        Braking();
        ApplySteer();
        CheckWaypointDistance();
        LerpToSteerAngle();
    }

    private void Sensors()
    {
        RaycastHit hit;
        float avoidMultiplier = 0;
        avoiding = false;

        if (Physics.Raycast(frontRightSensor.position, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, frontSensorLength))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(frontRightSensor.position, hit.point, Color.white);
                avoiding = true;
                avoidMultiplier -= 1f;
                if (currentSpeed > 5)
                {
                    isBraking = true;
                }
                else
                {
                    isBraking = false;
                }
            }
        }
        else if (Physics.Raycast(rightAngledSensor.position, Quaternion.AngleAxis(sideSensorAngle, transform.up) * transform.forward, out hit, angleSensorLength))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(rightAngledSensor.position, hit.point, Color.white);
                avoiding = true;
                avoidMultiplier -= 0.5f;
                if (currentSpeed > 5)
                {
                    isBraking = true;
                }
                else
                {
                    isBraking = false;
                }
            }
        }

        if (Physics.Raycast(frontLeftSensor.position, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, frontSensorLength))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(frontLeftSensor.position, hit.point, Color.white);
                avoiding = true;
                avoidMultiplier += 1f;
                if (currentSpeed > 5)
                {
                    isBraking = true;
                }
                else
                {
                    isBraking = false;
                }
            }
        }
        else if (Physics.Raycast(leftAngledSensor.position, Quaternion.AngleAxis(-sideSensorAngle, transform.up) * transform.forward, out hit, angleSensorLength))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(leftAngledSensor.position, hit.point, Color.white);
                avoiding = true;
                avoidMultiplier += 0.5f;
                if (currentSpeed > 5)
                {
                    isBraking = true;
                }
                else
                {
                    isBraking = false;
                }
            }
        }

        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(frontCenterSensor.position, transform.forward, out hit, frontSensorLength))
            {
                if (!hit.collider.CompareTag("Ground"))
                {
                    Debug.DrawLine(frontCenterSensor.position, hit.point, Color.white);
                    avoiding = true;
                    if (hit.normal.x < 0)
                    {
                        avoidMultiplier = -1;
                    }
                    else
                    {
                        avoidMultiplier = 1;
                    }

                    if (currentSpeed > 5)
                    {
                        isBraking = true;
                    }
                    else
                    {
                        isBraking = false;
                    }
                }
            }
        }

        if (avoiding)
        {
            targetSteerAngle = maxSteerAngle * avoidMultiplier;
        }

    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed && !isBraking)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
    }

    private void Braking()
    {
        if (isBraking)
        {
            wheelBL.brakeTorque = maxBrakeTorque;
            wheelBR.brakeTorque = maxBrakeTorque;
        }
        else
        {
            wheelBL.brakeTorque = 0;
            wheelBR.brakeTorque = 0;
        }
    }

    private void ApplySteer()
    {
        if (avoiding) return;
        isBraking = false;
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        targetSteerAngle = newSteer;
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 0.5)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.fixedDeltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.fixedDeltaTime * turnSpeed);
    }
}
