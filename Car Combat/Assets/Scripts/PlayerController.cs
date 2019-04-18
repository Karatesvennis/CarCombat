using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed = 10f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float bumpForce = 10f;

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
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        GetComponent<Fire>().FireProjectile();
    }

    /*private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Bumped enemy");

            //Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();

            Vector3 angle = other.contacts[0].point - transform.position;
            angle = -angle.normalized;
            rb.AddForce(angle * bumpForce);
            
        }
    }*/
}
