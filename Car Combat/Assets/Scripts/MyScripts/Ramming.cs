using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramming : MonoBehaviour
{

    [SerializeField] float bumpForce = 10f;
    [SerializeField] float bumpExplosionRaduis = 2f;
    [SerializeField] float bumpDamage = 200f;

    float newSpeedForce;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() != typeof(SphereCollider) && other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
        {
            Debug.Log("entered collision");
            Rigidbody otherRb = other.gameObject.GetComponentInParent<Rigidbody>();

            if (!otherRb)
            {
                return;
            }

            if (SpeedCollisionCheck(rb, otherRb))
            {
                Debug.Log("doing collision check: " + rb.gameObject.name);

                Vector3 extraForce = rb.gameObject.transform.forward * 30;
                extraForce += Vector3.up * 5;

                Vector3 extraRot = rb.gameObject.transform.right * 10;

                if (otherRb.GetComponent<Enemy>())
                {
                    otherRb.gameObject.GetComponent<Enemy>().extraForce = extraForce;
                    otherRb.gameObject.GetComponent<Enemy>().extraRotation = extraRot;
                }

                //  otherRb.AddForce(rb.gameObject.transform.forward * 500, ForceMode.Impulse);
                //  otherRb.AddForce(Vector3.up * 90, ForceMode.Impulse);
                //
                //  otherRb.AddTorque(rb.gameObject.transform.right * 90, ForceMode.Impulse);


                //   otherRb.AddExplosionForce(bumpForce, transform.position, bumpExplosionRaduis, 2.0f, mode: ForceMode.Impulse);
                other.GetComponentInParent<Health>().DealDamage(bumpDamage);
            }
        }
    }

    bool SpeedCollisionCheck(Rigidbody player, Rigidbody enemy)
    {

        var speedDifference = player.velocity.sqrMagnitude - enemy.velocity.sqrMagnitude;

        return speedDifference >= 0;
    }
}
