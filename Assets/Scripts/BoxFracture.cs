using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFracture : MonoBehaviour
{
   
    public GameObject fracturedCrate;    
    public AudioSource boxDestroyFX; 

    private void OnTriggerEnter(Collider other)
    {

        // Chequeamos colicion con el jugador... 
        if (other.gameObject.name == "Player")
        {
            //si esta en la animacion "spinning", entonces destruimos la caja
            if (other.GetComponent<PlayerController>().isSpinning)
            {                
                explode();                
            }
            // de lo contrario, vamos a la pantalla de Game Over
            else
            {
                other.GetComponent<PlayerController>().Crash();
            }            
        }
    }


    public void explode()
    {   
        GameManager.Instance.GetBoxScore();                                         // aumentamos la puntuacion
        boxDestroyFX.Play();                                                        // sonido
        Instantiate(fracturedCrate, transform.position, transform.rotation);        // instanciamos el objeto que contiene la animacion de destruccion
        Destroy(this.gameObject);                                                   // destruimos este objeto
    }  

}
