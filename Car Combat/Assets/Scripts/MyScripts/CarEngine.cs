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

    [Header("Sensors")]
    public Transform frontCenterSensor;
    public Transform frontLeftSensor;
    public Transform frontRightSensor;
    public Transform leftAngledSensor;
    public Transform rightAngledSensor;
    public float sensorLength = 5f;
    public float frontSensorPosition = 0.5f;
    public float sideSensorPosition = 0.2f;
    public float fronSensorAngle = 30f;
    

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
    }

    private void Sensors()
    {
        RaycastHit hit;
        float avoidMultiplier = 0;
        avoiding = false;

        if (Physics.Raycast(frontCenterSensor.position, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(frontCenterSensor.position, hit.point, Color.white);
                avoiding = true;
            }
        }

        if (Physics.Raycast(frontRightSensor.position, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(frontCenterSensor.position, hit.point, Color.white);
                avoiding = true;
                avoidMultiplier -= 1f;
            }
        }

        if (Physics.Raycast(rightAngledSensor.position, Quaternion.AngleAxis(fronSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(frontCenterSensor.position, hit.point, Color.white);
                avoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }

        if (Physics.Raycast(frontLeftSensor.position, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(frontCenterSensor.position, hit.point, Color.white);
                avoiding = true;
                avoidMultiplier += 1f;
            }
        }

        if (Physics.Raycast(leftAngledSensor.position, Quaternion.AngleAxis(-fronSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(frontCenterSensor.position, hit.point, Color.white);
                avoiding = true;
                avoidMultiplier += 0.5f;
            }
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
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 0.1)
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
}
