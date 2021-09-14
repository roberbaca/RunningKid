using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDesactivate : MonoBehaviour
{
  
    // Este script desactiva los menu una vez que finalizo la animacion de salida (hide)

    public void MenuOff()
    {
        this.gameObject.SetActive(false);
    }
}
