using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject gun;
    [SerializeField] float speed;
    [SerializeField] float timeBetweenShots;

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
            Debug.Log("Is firing");
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    public void FireOnPlayer()
    {
        Debug.Log("Has shot");
        Vector3 moveDirection = (player.transform.position - gun.transform.position).normalized;
        GameObject newProjectile = Instantiate(projectile, gun.transform.position, Quaternion.identity);
        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
        rb.velocity = moveDirection * speed;

        /*RaycastHit hit;

        if (Physics.Raycast(gun.transform.position, player.transform.position, out hit))
        {
            Debug.Log("Has shot");
            Vector3 moveDirection = (hit.point - gun.transform.position).normalized;
            GameObject newProjectile = Instantiate(projectile, gun.transform.position, Quaternion.identity);
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            rb.velocity = moveDirection * speed;
        }*/
    }
}
