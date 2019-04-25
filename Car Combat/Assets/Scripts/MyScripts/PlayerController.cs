using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 10f;
    [SerializeField] float sideSpeed = 5f;
    [SerializeField] float rotationSpeed = 100f;
    //[SerializeField] float bumpForce = 10f;
    //[SerializeField] float bumpExplosionRaduis = 2f;
    [SerializeField] float maxSpeed = 10f;
    //[SerializeField] float bumpDamage = 200f;
    float yRotation = 0f;

    
    //float newSpeedForce;

    private Vector3 eulerAngleVelocity;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        GetComponent<Fire>().FireProjectile();
    }

    private void Move()
    {
        //Drives the car
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 verticalMovement = transform.forward * moveVertical;
        Vector3 horizontalMovement = transform.right * moveHorizontal;
        rb.AddForce(verticalMovement * speed);
        rb.AddForce(horizontalMovement * sideSpeed);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        //Rotates the car
        eulerAngleVelocity = new Vector3(0, Input.GetAxis("Horizontal"), 0);
        Quaternion deltaRotation = Quaternion.Euler((eulerAngleVelocity * rotationSpeed) * Time.deltaTime);
        rb.rotation *= deltaRotation;
    }
}
