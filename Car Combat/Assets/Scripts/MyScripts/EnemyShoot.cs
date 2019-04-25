using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject gun;
    [SerializeField] float speed;
    [SerializeField] float timeBetweenShots = 1;

    GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }


    public IEnumerator Firing()
    {
        while (true)
        {
            FireOnPlayer();

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    public void FireOnPlayer()
    {
        if (player != null)
        {
            Vector3 moveDirection = (player.transform.position - gun.transform.position).normalized;
            GameObject newProjectile = Instantiate(projectile, gun.transform.position, Quaternion.identity);
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            rb.velocity = moveDirection * speed;
        }
    }
}
