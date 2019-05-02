using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableMissile : MonoBehaviour
{

    [SerializeField] Transform[] missilePickupPoint;

    Collider collider;
    Renderer renderer;

    int newSpawnPos;
    
    float nextSpawn = 0f;
    float spawnRate = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponentInChildren<Renderer>();
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);
    }

    private void Respawn()
    {
        newSpawnPos = Random.Range(0, missilePickupPoint.Length);
        transform.position = missilePickupPoint[newSpawnPos].position;
        collider.enabled = true;
        renderer.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            collider.enabled = false;
            renderer.enabled = false;
            FindObjectOfType<Fire>().amountOfAmmo++;
            Invoke("Respawn", 10f);
        }
    }
}
