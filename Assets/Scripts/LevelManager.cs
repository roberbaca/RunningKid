using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] sectionPrefabs;                                // array con las secciones
    [SerializeField] float zSpawn = 100;                                         // posicion de origen de la 1er seccion
    [SerializeField] float sectionLenght = 100;                                  // largo de cada seccion
    [SerializeField] int numberOfVisibleSections = 5;                            // cantidad de secciones que se veran en pantalla a la vez
    [SerializeField] List<GameObject> activeSections = new List<GameObject>();   // creamos una lista con las secciones activas
    [SerializeField] Transform playerTransform;                                  // para obtener la posicion del personaje
    [SerializeField] float safeZone = 35;                                        // zona de seguridad (sirve para no destruir la seccion en la que se encuentra el peronaje) 
    [SerializeField] int maxNumberSections;                                      // es una medida de la longitud del nivel
    [SerializeField] Slider slider;                                              // progress bar
    [SerializeField] GameObject endSection;                                      // ultima seccion del nivel

    [SerializeField] CurveController curved;
    private bool diceRolled = false;                                             // bandera
    private int dice = 0;                                                        // numero aleatorio
    private int sectionCount = 0;                                                // conteo de la secciones generadas

    void Start()
    {
        // al iniciar el juego, creamos las primeras secciones visibles
        for(int i = 0; i < numberOfVisibleSections; i++)
        {
            SpawnSection(Random.Range(0, sectionPrefabs.Length));
            sectionCount = 0;
        }
       
        // el slider en la parte inferior de la pantalla sirve para indicar cuanto falta para llegar al final del nivel
       slider.maxValue = (maxNumberSections + 1) * sectionLenght;                
    }

    
    void Update()
    {
        // creamos secciones nuevas de acuerdo a la posicion del jugador
        if ((playerTransform.position.z - safeZone) > (zSpawn - numberOfVisibleSections * sectionLenght))
        {
            if (sectionCount < (maxNumberSections - numberOfVisibleSections))
            {
                SpawnSection(Random.Range(0, sectionPrefabs.Length));
            }
            else
            {
                // si llegamos al final del nivel, instanciamos una seccion especial
                SpawnEndSection();
            }
            
            DeleteSection();
        }

        slider.value = playerTransform.position.z; // movemos el slider a medida que el personaje avanza por el nivel


        // Completamos el nivel cuando alcanzamos la ultima seccion
        if (sectionCount == maxNumberSections)
        {
            GameManager.Instance.OnLevelCompleted();
            sectionCount = 0;
        }


        // doblamos el mundo a la izquierda o derecha en forma aletatoria
        if (GameManager.Instance.isGameStarted && !GameManager.Instance.isDead && !GameManager.Instance.isGamePaused)
        {
            switch (dice)
            {
                case 0:
                    curved.curvedStraight();
                    break;
                case -1:
                    curved.curvedLeft();
                    break;
                case 1:
                    curved.curvedRight();
                    break;              
            }

            // cada 4 secciones doblamos el mundo
            if (sectionCount % 4 == 0 && sectionCount > 3)
            {
                rollDice();
                
            }

            if (sectionCount % 4 != 0)
            {
                diceRolled = false;
            }
        }
    }

    public void SpawnSection(int index)
    {
        GameObject newSection = Instantiate(sectionPrefabs[index], transform.forward * zSpawn, transform.rotation);
        activeSections.Add(newSection); // agregamos un nuevo item al array de secciones
        zSpawn += sectionLenght;        // posicionamos la nueva seccion a continuacion de la anterior
        sectionCount++;        
        GameManager.Instance.sectionCountText.text = sectionCount.ToString("0");    // para debug
        Debug.Log(sectionCount);
    }

    private void DeleteSection()
    {
        Destroy(activeSections[0]); // destruimos el primer elemento de la lista
        activeSections.RemoveAt(0); // sacamos el primer elemento de la lista        
    }

    public void SpawnEndSection()
    {
        // creamos la seccion que tiene la linea de llegada
        GameObject newSection = Instantiate(endSection, transform.forward * zSpawn, transform.rotation);
        activeSections.Add(newSection); 
        zSpawn += sectionLenght;
        sectionCount++;     
    }

    public void rollDice()
    {
        // obtenemos un numero random entero entre -1 y 1        
        if (!diceRolled)
        {
            dice = Random.Range(-1, 2);

            // para debug...
            if (dice == 0)
            {
                Debug.Log("Dice = " + dice);
            }
            
            diceRolled = true;
        }              
    }
}
