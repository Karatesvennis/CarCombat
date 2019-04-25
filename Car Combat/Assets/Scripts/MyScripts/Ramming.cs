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
                Vector3 otherAngle = transform.position - other.transform.position;
                otherAngle = -otherAngle.normalized;
                otherRb.AddForce(new Vector3(0, 10000, 0));
                //otherRb.AddExplosionForce(bumpForce, transform.position, bumpExplosionRaduis);
                other.GetComponentInParent<Health>().DealDamage(bumpDamage);
                //Debug.Log(newSpeedForce);
            }
            /*else if (!SpeedCollisionCheck(rb, otherRb))
            {
                Debug.Log("Enemy was faster");
                Vector3 angle = other.transform.position - transform.position;
                angle = -angle.normalized;
                rb.AddForce((angle * bumpForce)  * newSpeedForce);
                Debug.Log(newSpeedForce);
            }*/
        }
    }

    bool SpeedCollisionCheck(Rigidbody player, Rigidbody enemy)
    {
        
        var speedDifference = player.velocity.sqrMagnitude - enemy.velocity.sqrMagnitude;
        var nextSpeedDifference = enemy.velocity.sqrMagnitude - player.velocity.sqrMagnitude;

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
