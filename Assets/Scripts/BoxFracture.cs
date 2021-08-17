using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFracture : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject fracturedCrate;

    //public static BoxFracture Instance { set; get; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
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
        //gameObject.SetActive(false);
        Instantiate(fracturedCrate, transform.position, transform.rotation);        
        Destroy(this.gameObject);

    }
}
