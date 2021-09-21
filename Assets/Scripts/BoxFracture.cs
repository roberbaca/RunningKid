using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFracture : MonoBehaviour
{
   
    public GameObject fracturedCrate;    
    public AudioSource boxDestroyFX; 

    private void OnTriggerEnter(Collider other)
    {

        // Chequeamos colicion con el jugador, si esta en la animacion "spinning", entonces destruimos la caja
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
        GameManager.Instance.GetCoin(3);                                            // obtenemos 3 monedas por destruir la caja
        boxDestroyFX.Play();                                                        
        Instantiate(fracturedCrate, transform.position, transform.rotation);        // instanciamos el objeto que contiene la animacion de destruccion
        Destroy(this.gameObject);
    }  

}
