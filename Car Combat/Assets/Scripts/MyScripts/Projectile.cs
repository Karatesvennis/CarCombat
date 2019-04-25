using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] float damage = 50f;
    [SerializeField] GameObject deathVFX;

    public bool playerShot;


    private void OnTriggerEnter(Collider other)
    {

        if (playerShot && other.tag != "Player" && other.GetType() != typeof(SphereCollider))
        {
            if (other.GetComponent<Health>())
            {
                other.GetComponent<Health>().DealDamage(damage);
                DestroyWithVFX();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (!playerShot && other.tag != "Enemy" && other.GetType() != typeof(SphereCollider))
        {
            if (other.GetComponent<Health>())
            {
                other.GetComponent<Health>().DealDamage(damage);
                DestroyWithVFX();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void DestroyWithVFX()
    {
        GameObject myDeathVFX = Instantiate(deathVFX, this.transform.position, Quaternion.identity);
        Destroy(myDeathVFX, 0.2f);
        Destroy(gameObject);
    }
}
