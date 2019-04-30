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

    private float normalMaxSpeed = 0f;

    float distanceToGround;

    public Vector3 com;
    
    public bool isGrounded = false;

    private Vector3 eulerAngleVelocity;
    private Rigidbody rb;
    new Collider collider;

    public Vector3 extraForce;
    public Vector3 extraRotation;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        rb.centerOfMass = com;
        distanceToGround = collider.bounds.extents.y;
        normalMaxSpeed = maxSpeed;
        
    }

    private void OnDestroy()
    {
        GameObject myDeathVFX = Instantiate(deathVFX, this.transform.position, Quaternion.identity);
        Destroy(myDeathVFX, 2f);
    }

    private void Update()
    {
        GetComponent<Fire>().FireProjectile();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (IsGrounded())
        {
            if (Input.GetKey(KeyCode.Space))
            {
                maxSpeed = maxSpeed + 10;
            }

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
            maxSpeed = normalMaxSpeed;
        }
        

        //Testar
        eulerAngleVelocity = new Vector3(0, Input.GetAxis("Horizontal"), 0);
        Quaternion deltaRotation = Quaternion.Euler((eulerAngleVelocity * rotationSpeed) * Time.fixedDeltaTime);
        rb.rotation *= deltaRotation;

        //For ramming
        extraForce = Vector3.Lerp(extraForce, Vector3.zero, Time.fixedDeltaTime * 5);
        extraRotation = Vector3.Lerp(extraRotation, Vector3.zero, Time.fixedDeltaTime * 5);
        rb.velocity += extraForce; // external
        rb.rotation *= Quaternion.Euler(extraRotation);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }
}
