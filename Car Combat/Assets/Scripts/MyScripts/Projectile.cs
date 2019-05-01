﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] float damage = 50f;
    [SerializeField] GameObject deathVFX;

    public bool playerShot;

    TimeFlowManager timeManager;


    private void Start()
    {
        timeManager = FindObjectOfType<TimeFlowManager>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (playerShot && other.tag != "Player" && other.GetType() != typeof(SphereCollider))
        {
            ExplosionDamageOnImpact(transform.position, 30);

            if (other.GetComponent<Health>())
            {
                other.GetComponent<Health>().DealDamage(damage);
            }
            DestroyWithVFX();
            timeManager.DoSlowMotion();
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

    void ExplosionDamageOnImpact(Vector3 center, float radius)
    {

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Rigidbody hitRb = hitColliders[i].gameObject.GetComponent<Rigidbody>();
            if (hitRb != null)
            {
                hitRb.AddExplosionForce(5f, center, 8f, 3f, ForceMode.Impulse);
            }
        }
    }

    void DestroyWithVFX()
    {

        GameObject myDeathVFX = Instantiate(deathVFX, this.transform.position + Vector3.up, Quaternion.identity);
        Destroy(myDeathVFX, 1f);
        Destroy(gameObject);
    }
}
