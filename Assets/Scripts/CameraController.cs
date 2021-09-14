using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt; // target que va a seguir la camara

    public Vector3 offset;
    public Vector3 rotation;

    public bool isMoving { set; get; }

 
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

        if (!GameManager.Instance.isDead)
        {
            Vector3 desiredPosition = lookAt.position + offset;
            desiredPosition.x = 0;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.1f);
        }     
                
    }

}
