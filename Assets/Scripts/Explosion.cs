using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    
    private MeshCollider meshCol;
    private Rigidbody rb;

    void Start()
    {
        meshCol = GetComponent<MeshCollider>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine("DisableColliders");
        StartCoroutine("OnDestroyObject");
      
    }

    IEnumerator DisableColliders()
    {
        yield return new WaitForSeconds(0.1f);
        meshCol.enabled = false;
        rb.detectCollisions = false;            
    }

    IEnumerator OnDestroyObject()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }


}