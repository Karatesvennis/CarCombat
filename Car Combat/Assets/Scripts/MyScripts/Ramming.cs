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
                otherRb.AddExplosionForce(bumpForce, transform.position, bumpExplosionRaduis, 1.0f, mode: ForceMode.Impulse);
                other.GetComponentInParent<Health>().DealDamage(bumpDamage);
            }
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
