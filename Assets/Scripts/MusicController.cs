using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{ 
    private static MusicController music;    

    private void Awake()
    {

        // Para mantener la musica incluso cuando cambiamos de escena
        if (music == null)
        {
            music = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }        
    }
    
}
