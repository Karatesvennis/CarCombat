using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 10f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float bumpForce = 10f;
    [SerializeField] float bumpExplosionRaduis = 2f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float maxRotation = 50f;

    float bumpDamage = 200f;
    float newSpeedForce;

    private Vector3 eulerAngleVelocity;
    private Rigidbody rb;

    bool dead = false;
    
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


        /*float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        float moveVertical = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        Vector3 movement = new Vector3(turn, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
        rb.rotation += rotation;*/
    }

    private void Move()
    {
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveVertical;
        rb.AddForce(movement * speed);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        var xRotation = rb.rotation.x;
        var yRotation = rb.rotation.y;
        var zRotation = rb.rotation.z;
        
        eulerAngleVelocity = new Vector3(0, Input.GetAxis("Horizontal"), 0);
        Quaternion deltaRotation = Quaternion.Euler((eulerAngleVelocity * rotationSpeed) * Time.deltaTime);
        rb.rotation *= deltaRotation;

        //yRotation = Mathf.Clamp(yRotation, -90, 90);
        //xRotation = Mathf.Clamp(xRotation, 0, 0);
        //zRotation = Mathf.Clamp(zRotation, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.GetType() != typeof(SphereCollider))
        {

            Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
            

            if (SpeedCollisionCheck(rb, enemyRb))
            {
                Debug.Log("Player was faster");
                Vector3 otherAngle = transform.position - other.transform.position;
                otherAngle = -otherAngle.normalized;
                enemyRb.AddExplosionForce(bumpForce, transform.position, bumpExplosionRaduis);
                //other.GetComponent<Health>().DealDamage(bumpDamage);
                //Debug.Log(newSpeedForce);
            }
            else if (!SpeedCollisionCheck(rb, enemyRb))
            {
                Debug.Log("Enemy was faster");
                Vector3 angle = other.transform.position - transform.position;
                angle = -angle.normalized;
                rb.AddForce((angle * bumpForce) /* * newSpeedForce*/);
                //Debug.Log(newSpeedForce);
            }
        }
    }

    bool SpeedCollisionCheck(Rigidbody player, Rigidbody enemy)
    {
        var speedDifference = player.velocity.sqrMagnitude - enemy.velocity.sqrMagnitude;
        var nextSpeedDifference = enemy.velocity.sqrMagnitude - player.velocity.sqrMagnitude;

        //Debug.Log(speedDifference + " " + nextSpeedDifference);

        if (speedDifference > nextSpeedDifference)
        {
            newSpeedForce = speedDifference;
            return true;
        }
        else
        {
            newSpeedForce = nextSpeedDifference;
            return false;
        }
    }
}
