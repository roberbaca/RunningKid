using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform lookAt; // target que va a seguir la camara
    public Vector3 offset;

    private void LateUpdate()
    {
        Vector3 desiredPosition = lookAt.position + offset/2;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
    }


    // Start is called before the first frame update
    void Start()
    {
        transform.position = lookAt.position + offset;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
