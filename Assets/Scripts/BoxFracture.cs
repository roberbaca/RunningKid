using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFracture : MonoBehaviour
{
   
    public GameObject fracturedCrate; 
   
    private void OnTriggerEnter(Collider other)
    {

        // Buscamos al jugador, si esta en la animacion "spinning", entonces destruimos la caja
        if (other.gameObject.name == "Player")
        {      
            if (other.GetComponent<PlayerController>().isSpinning)
            {
                explode();                
            }
            else
            {
                other.GetComponent<PlayerController>().Crash();
            }            
        }
    }

    public void explode()
    {
        //make object disappear      
        Instantiate(fracturedCrate, transform.position, transform.rotation);        
        Destroy(this.gameObject);

    }
}
