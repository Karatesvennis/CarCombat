using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    [SerializeField] GameObject cameraViewPoint = null;

    private Vector3 offset;

    private Vector3 posOffset;

    private Quaternion rotationOffset;

    [SerializeField] float turnSpeed = 4f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        offset = player.transform.forward * -4;
        offset += Vector3.up * 3;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;

        //float horizontal = Input.GetAxis("Mouse X") * turnSpeed;
        //transform.Rotate(0, horizontal, 0);

        //rotationOffset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up);

        if (player != null)
        {
            transform.position = player.transform.position + offset;
            transform.LookAt(cameraViewPoint.transform.position);
        }
    }
}
