using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;

    [SerializeField] float turnSpeed = 4f;
    
    //float horizontalSpeed = 2f;
    //float verticalSpeed = 2f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(player.transform.position.x, player.transform.position.y + 2f, player.transform.position.z - 4f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;

        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform.position);
    }
}
