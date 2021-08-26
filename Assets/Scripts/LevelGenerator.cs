 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public GameObject[] section;         // array para guardar cada seccion del nivel
    public int zPos = 50;                // distancia entre seccion y seccion en el eje Z
    public bool creatingSection = false; // bandera para ir eliminando secciones
    public int secNum;                   // numero de seccion

    //private List<GameObject> activeSections = new List<GameObject>();

      

    // Start is called before the first frame update
    void Start()
    {
        
    } 

    // Update is called once per frame
    void Update()
    {
        if (!creatingSection && GameManager.Instance.isGameStarted && !GameManager.Instance.isDead)
        {
            creatingSection = true;
            StartCoroutine(GenerateSection());

            if (zPos >= 250)
            {
                //StartCoroutine(DeleteSection());
            }
        }
      
    }


    IEnumerator GenerateSection()
    {
        secNum = Random.Range(0, section.Length);                                    // elegimos una seccion al azar (min inclusivo, max exclusivo)
        GameObject tile = Instantiate(section[secNum], new Vector3(0,0,zPos),Quaternion.identity);     // instanciamos la seccion
        //activeSections.Add(tile);
        zPos += 100;
        yield return new WaitForSeconds(1.5f);
        creatingSection = false;

    }

    IEnumerator DeleteSection()
    {
        yield return new WaitForSeconds(1.5f);
        //Destroy(activeSections[0]);
        //activeSections.RemoveAt(0);
    }

}
