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
    [SerializeField] AudioSource clickSFX;
    [SerializeField] AudioSource coinSFX;
    [SerializeField] AudioSource multipleCoinsSFX;

    // MUsic
    [SerializeField] AudioSource music;

    // HUD
    [SerializeField] Animator gameCanvas, coinAnim, boxAnim;         
    private int coinScore, boxScore;
    [SerializeField] GameObject HUDMenu;

    // Main Menu
    [SerializeField] Animator mainMenuAnim;
    [SerializeField] GameObject mainMenu;

    // Game Over
    [SerializeField] Animator gameOverMenuAnim;
    [SerializeField] TextMeshProUGUI gameOverCoinText, coinText;
    [SerializeField] TextMeshProUGUI gameOverBoxText, boxText;
    [SerializeField] GameObject gameOverMenu;

    // Pause
    [SerializeField] Animator pauseMenuAnim;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseButton;

    // Level Completed
    [SerializeField] TextMeshProUGUI levelCompletedCoinText;
    [SerializeField] TextMeshProUGUI levelCompletedBoxText;
    [SerializeField] Animator levelCompletedAnim;
    [SerializeField] GameObject levelCompletedMenu;
    [SerializeField] AudioSource victoryFX;

    // credits  
    [SerializeField] GameObject credits;
    [SerializeField] Animator creditsAnim;

    // setings
    [SerializeField] GameObject settings;
    [SerializeField] Animator settingsAnim;

    // tutorial
    [SerializeField] GameObject tutorial;
    [SerializeField] Animator tutorialAnim;

    // particles
    [SerializeField] GameObject confetiParticles;


    private void Awake()
    {
        Instance = this;       

        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    
        pauseMenu.SetActive(false);
        levelCompletedMenu.SetActive(false);
        credits.SetActive(false);
        settings.SetActive(false);
    }

    void Start()
    {       
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);                
    }

    
    void Update()
    {      
        if(Input.GetKeyDown(KeyCode.P))
        {
            OnPauseGame();
        }  
    }


    public void UpdateScores()
    {        
        coinText.text = coinScore.ToString();
        boxText.text = boxScore.ToString();
    }
 

    public void GetCoin(int quantityOfCoins)
    {
        // puntaje cuando agarramos una moneda
        if(quantityOfCoins == 1)
        {
            coinAnim.SetTrigger("Collect");
            coinSFX.Play();
            coinScore += quantityOfCoins;
            coinText.text = coinScore.ToString("0");
        }                       
    }

   

    public void GetBoxScore()
    {
        // puntaje cuando rompemos una caja de madera
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
        // despausamos el juego
        isGamePaused = false;
        clickSFX.Play();
        pauseMenuAnim.SetTrigger("Resume");
        Time.timeScale = 1 ;
        pauseButton.SetActive(true);
        HUDMenu.SetActive(true);
    }



    /*    
     =======================
     Level Finished Screen
    ========================
    */

    public void OnLevelCompleted()
    {
        confetiParticles.GetComponentInChildren<ParticleSystem>().Play();
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
        Menu´s
    ===============
    */

    public void CloseApp()
    {
        // para salir del juego
        clickSFX.Play();
        Debug.Log("closing app");
        Application.Quit();
    }

    public void OnStartGame()
    {        
        // comienza la partida
        clickSFX.Play();
        HideTutorial();
        tutorial.SetActive(false);
        isGameStarted = true;
        controller.StartRunning();
        FindObjectOfType<CameraController>().isMoving = true;   
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
