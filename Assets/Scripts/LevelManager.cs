using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject[] sectionPrefabs;                                // array con las secciones
    public float zSpawn = 100;                                           // posicion de origen de la 1er seccion
    public float sectionLenght = 100;                                  // largo de cada seccion
    public int numberOfVisibleSections = 5;                                   // cantidad de secciones que se veran en pantalla a la vez
    public List<GameObject> activeSections = new List<GameObject>();   // creamos una lista con las secciones activas
    public Transform playerTransform;                                  // para obtener la posicion del personaje
    public float safeZone = 35;                                        // zona de seguridad (sirve para no destruir la seccion en la que se encuentra el peronaje)

    void Start()
    {
        for(int i = 0; i < numberOfVisibleSections; i++)
        {

            SpawnSection(Random.Range(0, sectionPrefabs.Length));       
            
        }
    }

    
    void Update()
    {
        if ((playerTransform.position.z - safeZone) > (zSpawn - numberOfVisibleSections * sectionLenght))
        {
            SpawnSection(Random.Range(0, sectionPrefabs.Length));
            DeleteSection();
        }
    }

    public void SpawnSection(int index)
    {
        GameObject newSection = Instantiate(sectionPrefabs[index], transform.forward * zSpawn, transform.rotation);
        activeSections.Add(newSection); // agregamos un nuevo item al array de secciones
        zSpawn += sectionLenght;
         
    }

    private void DeleteSection()
    {
        Destroy(activeSections[0]); // destruimos el primer elemento de la lista
        activeSections.RemoveAt(0); // sacamos el primer elemento de la lista
    }
}
