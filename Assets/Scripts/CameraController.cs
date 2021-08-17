using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform lookAt; // target que va a seguir la camara
    //public Vector3 offset = new Vector3(0, 5.0f, -6.0f);
    //public Vector3 rotation = new Vector3(15,0,0);

    public Vector3 offset;
    public Vector3 rotation;

    public bool isMoving { set; get; }

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = lookAt.position + offset;        
    }

 

    private void LateUpdate()
    {
        
        if (!isMoving)
        {
            return;
        }

        
        //Vector3 desiredPosition = lookAt.position + offset / 2;
        Vector3 desiredPosition = lookAt.position + offset;
        desiredPosition.x = 0;
        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation),0.1f);
    }

}
