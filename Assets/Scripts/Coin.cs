using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;
    public AudioSource coinFX;

    private void Start()
    {
        anim = GetComponent<Animator>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            coinFX.Play();
            GameManager.Instance.GetCoin();
            anim.SetTrigger("Collected");
            Destroy(gameObject, 1.2f);        
        }
        
    }
}
