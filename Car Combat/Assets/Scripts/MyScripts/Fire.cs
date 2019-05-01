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
    int amountOfAmmo = 2;

    GameObject crosshair;

    Vector3 hitPoint;

    private void Start()
    {
        crosshair = FindObjectOfType<Crosshair>().gameObject;
    }


    public void FireProjectile()
    {
        if (amountOfAmmo <= 0)
        {
            crosshair.SetActive(false);
        }
        else
        {
            crosshair.SetActive(true);

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
                    amountOfAmmo--;
                }
            }
        }
    }
}
