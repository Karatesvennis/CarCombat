using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    [SerializeField] float damage = 50f;

    public bool playerShot;


    private void OnTriggerEnter(Collider other)
    {

        if (playerShot && other.tag != "Player")
        {
            if (other.GetComponent<Health>())
            {
                other.GetComponent<Health>().DealDamage(damage);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (!playerShot && other.tag != "Enemy")
        {
            if (other.GetComponent<Health>())
            {
                other.GetComponent<Health>().DealDamage(damage);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
