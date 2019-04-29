using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 10f;
    [SerializeField] float sideSpeed = 5f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] GameObject deathVFX = null;
    //[SerializeField] float torque = 1f;

    float distanceToGround;

    public Vector3 com;
    
    public bool isGrounded = false;

    private Vector3 eulerAngleVelocity;
    private Rigidbody rb;
    new Collider collider;
    //private Vector3 baseRotation;

    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        rb.centerOfMass = com;
        distanceToGround = collider.bounds.extents.y;
    }

    private void OnDestroy()
    {
        GameObject myDeathVFX = Instantiate(deathVFX, this.transform.position, Quaternion.identity);
        Destroy(myDeathVFX, 2f);
    }

    private void Update()
    {
        //Move();
        GetComponent<Fire>().FireProjectile();
    }

    private void FixedUpdate()
    {
        TestMove();
    }

    private void TestMove()
    {
        if (IsGrounded())
        {
            float moveVertical = Input.GetAxis("Vertical");
            float turn = Input.GetAxis("Horizontal");
            float drift = 5f;
            float forwardSpeedAdjustment = speed - transform.InverseTransformDirection(rb.velocity).z;
            float sideSpeedAdjustment = -transform.InverseTransformDirection(rb.velocity).x / drift;

            Vector3 localVelocity = rb.velocity;

            Vector3 verticalMovement = transform.forward * moveVertical;
            Vector3 horizontalMovement = transform.right * turn;

            rb.AddForce(verticalMovement * speed);
            rb.AddRelativeForce(sideSpeedAdjustment, 0f, 0f, ForceMode.VelocityChange);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        

        //Testar
        eulerAngleVelocity = new Vector3(0, Input.GetAxis("Horizontal"), 0);
        Quaternion deltaRotation = Quaternion.Euler((eulerAngleVelocity * rotationSpeed) * Time.fixedDeltaTime);
        rb.rotation *= deltaRotation;


        //rb.AddRelativeTorque(transform.up * torque * turn);
    }

    private void Move()
    {
        //Drives the car

        if (isGrounded)
        {
            float moveVertical = Input.GetAxis("Vertical");
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector3 verticalMovement = transform.forward * moveVertical;
            Vector3 horizontalMovement = transform.right * moveHorizontal;
            rb.AddForce(verticalMovement * speed);
            rb.AddForce(horizontalMovement * sideSpeed);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }

        //Rotates the car
        
        eulerAngleVelocity = new Vector3(0, Input.GetAxis("Horizontal"), 0);
        Quaternion deltaRotation = Quaternion.Euler((eulerAngleVelocity * rotationSpeed) * Time.fixedDeltaTime);
        rb.rotation *= deltaRotation;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }
}
