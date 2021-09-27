using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { set; get; }

    public bool isGameStarted = false;          // bandera para saber si el nivel comenzo
    public bool isDead = false;                 // bandera para saber si el personaje colisiono con un obstaculo
    public bool isGamePaused = false;           // bandera para saber si el juego esta en pausa
    public TextMeshProUGUI sectionCountText;    // para debug (conteo de las secciones generadas)
    private PlayerController controller;

    // SFX
    public AudioSource clickSFX;
    public AudioSource coinSFX;
    public AudioSource multipleCoinsSFX;

    // MUsic
    public AudioSource music;

    // HUD
    public Animator gameCanvas, coinAnim, boxAnim;         
    private int coinScore, boxScore;    
    public GameObject HUDMenu;

    // Main Menu
    public Animator mainMenuAnim;
    public GameObject mainMenu;

    // Game Over
    public Animator gameOverMenuAnim;
    public TextMeshProUGUI gameOverCoinText, coinText;
    public TextMeshProUGUI gameOverBoxText, boxText;
    public GameObject gameOverMenu;

    // Pause
    public Animator pauseMenuAnim;
    public GameObject pauseMenu;
    public GameObject pauseButton;

    // Level Completed
    public TextMeshProUGUI levelCompletedCoinText;
    public TextMeshProUGUI levelCompletedBoxText;
    public Animator levelCompletedAnim;
    public GameObject levelCompletedMenu;
    public AudioSource victoryFX;  
    //public GameObject levelFinished;
    //public Animator levelFinishedAnim;

    // credits  
    public GameObject credits;
    public Animator creditsAnim;

    // setings
    public GameObject settings;
    public Animator settingsAnim;

    // tutorial
    public GameObject tutorial;
    public Animator tutorialAnim;

    // Sky
    public GameObject sky;
    public float dayNightCycleSpeed = 0.1f;

    // particles
    public GameObject confetiParticles;


    private void Awake()
    {
        Instance = this;       

        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    
        pauseMenu.SetActive(false);
        levelCompletedMenu.SetActive(false);
        credits.SetActive(false);
        settings.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {       
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);                
    }

    
    void Update()
    {

        // controles tactiles
        if(MobileInput.Instance.Tap && !isGameStarted)
        {
            //isGameStarted = true;
            //controller.StartRunning();
            //FindObjectOfType<CameraController>().isMoving = true;         
            //mainMenuAnim.SetTrigger("Hide");
            //HUDMenu.SetActive(true);            
        }

        if(Input.anyKey && !isGameStarted)
        {
            //isGameStarted = true;
            //controller.StartRunning();
            //FindObjectOfType<CameraController>().isMoving = true;          
            //mainMenuAnim.SetTrigger("Hide");
            //HUDMenu.SetActive(true);
        }      

        if(Input.GetKeyDown(KeyCode.P))
        {
            OnPauseGame();
        }

        // Ciclo dia/noche

        //sky.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(dayNightCycleSpeed * Time.time, 0);
        //sky.GetComponent<Transform>().Rotate(Vector3.up * Time.deltaTime * 5);

    }


    public void UpdateScores()
    {        
        coinText.text = coinScore.ToString();
        boxText.text = boxScore.ToString();
    }
 

    public void GetCoin(int quantityOfCoins)
    {
        if(quantityOfCoins == 1)
        {
            coinAnim.SetTrigger("Collect");
            coinSFX.Play();
            coinScore += quantityOfCoins;
            coinText.text = coinScore.ToString("0");
        }
        else
        {
            //coinAnim.SetTrigger("Multiple");
            //multipleCoinsSFX.Play();
            //StartCoroutine(MultipleCoinScore());
        }                   
    }

    /*
    private IEnumerator MultipleCoinScore()
    {     
        // para obtener multiples monedas...
        for (int i = 0; i < 3; i++)
        {
            coinScore++;
            coinText.text = coinScore.ToString("0");
            yield return new WaitForSeconds(0.3f);
        }

    }
    */

    public void GetBoxScore()
    {        
        boxAnim.SetTrigger("BreakBox");
        boxScore ++;
        boxText.text = boxScore.ToString("0");
    }


    public void OnPlayButton()
    {
        // cargamos la escena
        music.Stop();
        clickSFX.Play();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        
    }


    /*    
    ===============
    Game Over menu
    ===============
    */

    public void OnGameOver()
    {        
        isDead = true;  
        gameOverMenu.SetActive(true);
        pauseButton.SetActive(false);
        HUDMenu.SetActive(false);
        gameOverCoinText.text = coinScore.ToString("0");
        gameOverBoxText.text = boxScore.ToString("0");
        gameOverMenuAnim.SetTrigger("GameOver");
    }


    /*    
     ===============
       pause menu
     ===============
    */


    public void OnPauseGame()
    {
        // pausamos el juego

        if (isDead)
        {
            return;
        }

        HUDMenu.SetActive(false);
        isGamePaused = true;
        pauseButton.SetActive(false);
        clickSFX.Play();
        pauseMenu.SetActive(true);
        pauseMenuAnim.SetTrigger("Pause");
        Time.timeScale = 0;   
    }



    public void OnResumeGame()
    {
       
        isGamePaused = false;
        clickSFX.Play();
        pauseMenuAnim.SetTrigger("Resume");
        Time.timeScale = 1 ;
        pauseButton.SetActive(true);
        HUDMenu.SetActive(true);
    }



    /*    
     ===============
     Level Finished
    ===============
    */

    public void OnLevelCompleted()
    {

        confetiParticles.GetComponentInChildren<ParticleSystem>().Play(); // VER SI LO SACO
        HUDMenu.SetActive(false);
        music.Stop();
        victoryFX.Play();      

        controller.StopRunning();
        isDead = true;     

        levelCompletedMenu.SetActive(true);         
        levelCompletedCoinText.text = coinScore.ToString("0");
        levelCompletedBoxText.text = boxScore.ToString("0");
        levelCompletedAnim.SetTrigger("LevelCompleted");
    }



    /*    
    ===============
      Main menu
    ===============
    */

    public void CloseApp()
    {
        clickSFX.Play();
        Debug.Log("closing app");
        Application.Quit();
    }

    public void OnStartGame()
    {        
        clickSFX.Play();
        HideTutorial();
        tutorial.SetActive(false);
        isGameStarted = true;
        controller.StartRunning();
        FindObjectOfType<CameraController>().isMoving = true;
        //mainMenuAnim.SetTrigger("Hide");
        HUDMenu.SetActive(true);        
    }

    public void ShowCredits()
    {
        clickSFX.Play();
        credits.SetActive(true);      
        mainMenuAnim.SetTrigger("Hide");
        creditsAnim.SetTrigger("showCredits");        
    }

    public void HideCredits()
    {
        clickSFX.Play();
        creditsAnim.SetTrigger("hideCredits");
        ShowMainMenu();
    }


    public void ShowSettings()
    {
        clickSFX.Play();
        settings.SetActive(true);     
        mainMenuAnim.SetTrigger("Hide");
        settingsAnim.SetTrigger("showSettings");
    }

    public void HideSettings()
    {
        clickSFX.Play();
        settingsAnim.SetTrigger("hideSettings");
        ShowMainMenu();
    }


    public void ShowMainMenu()
    {   
        mainMenu.SetActive(true);
        mainMenuAnim.SetTrigger("Show");
    }

    public void ShowTutorial()
    {
        clickSFX.Play();
        tutorial.SetActive(true);
        mainMenuAnim.SetTrigger("Hide");
        tutorialAnim.SetTrigger("TutorialOn");
    }

    public void HideTutorial()
    {
        clickSFX.Play();
        tutorialAnim.SetTrigger("TutorialOff");       
    }
}
