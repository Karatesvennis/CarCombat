using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    [SerializeField] GameObject projectile;
    [SerializeField] GameObject gun;
    [SerializeField] Camera mainCam;
    [SerializeField] float speed;
    [SerializeField] float fireRate;
    float nextFire;

    Vector3 hitPoint;


    public void FireProjectile()
    {
        Ray ray;
        RaycastHit hit;

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Vector3 mouseResult = Input.mousePosition;
            mouseResult = new Vector3(Screen.width / 2, mouseResult.y);

            ray = mainCam.ScreenPointToRay(mouseResult);

            if (Physics.Raycast(ray, out hit))
            {
                hitPoint = hit.point;
                Vector3 moveDirection = (hitPoint - gun.transform.position).normalized;
                GameObject newProjectile = Instantiate(projectile, gun.transform.position, Quaternion.identity);
                newProjectile.GetComponent<Projectile>().playerShot = true;
                Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
                rb.velocity = moveDirection * speed;
            }
        }
    }
}
