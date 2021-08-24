using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
  
    // Destruimos el objeto transcurrido unos segundos

    void Start()
    {        
        StartCoroutine("OnDestroyObject");      
    }

    IEnumerator OnDestroyObject()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }


}