using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    public GameObject unit = null;

    private void Start()
    {
        unit = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            if (unit.GetComponent<PlayerController>())
            {
                unit.GetComponent<PlayerController>().isGrounded = true;
            }

            if (unit.GetComponent<Enemy>())
            {
                unit.GetComponent<Enemy>().isGrounded = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            if (unit.GetComponent<PlayerController>())
            {
                unit.GetComponent<PlayerController>().isGrounded = false;
            }

            if (unit.GetComponent<Enemy>())
            {
                unit.GetComponent<Enemy>().isGrounded = false;
            }
        }
    }
}
