using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

    }
    private void OnTriggerEnter(Collider other)
    {
        // chequeamos colision con el jugador...
        if (other.tag == "Player")
        {          
            GameManager.Instance.GetCoin(1); // aumentamos en 1 la puntuacion del juego
            anim.SetTrigger("Collected");   
            Destroy(gameObject, 1.2f);        
        }        
    }
}
