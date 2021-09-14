using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    //private const int COIN_SCORE_AMOUNT = 5;
    public static GameManager Instance { set; get; }

    public bool isGameStarted = false; // bandera para saber si el nivel comenzo
    public bool isDead = false;
    public bool isGamePaused = false;
    private PlayerController controller;

    // FX
    public AudioSource clickFX;


    // MUsic
    public AudioSource music;

    // HUD
    public Animator gameCanvas, coinAnim;         
    private int coinScore;
    public TextMeshProUGUI sectionCountText;
    public GameObject HUDMenu;

    // Main Menu
    public Animator mainMenuAnim;
    public GameObject mainMenu;

    // Game Over
    public Animator gameOverMenuAnim;
    public TextMeshProUGUI gameOverCoinText, coinText;
    public GameObject gameOverMenu;

    // Pause
    public Animator pauseMenuAnim;
    public GameObject pauseMenu;
    public GameObject pauseButton;

    // Level Completed
    public TextMeshProUGUI levelCompletedCoinText;
    public Animator levelCompletedAnim;
    public GameObject levelCompletedMenu;
    public AudioSource victoryFX;

    // Level Finished
    public GameObject levelFinished;
    public Animator levelFinishedAnim;

    // credits  
    public GameObject credits;
    public Animator creditsAnim;

    // setings
    public GameObject settings;
    public Animator settingsAnim;


    // Sky
    public GameObject sky;
    public float dayNightCycleSpeed = 0.1f;

    private void Awake()
    {
        Instance = this;

        //UpdateScores();

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

    // Update is called once per frame
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
    }
 

    public void GetCoin()
    {
        coinAnim.SetTrigger("Collect");
        coinScore ++;
        coinText.text = coinScore.ToString("0");

        //scoreText.text = score.ToString("0");
    }

    public void OnPlayButton()
    {
        clickFX.Play();
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
        gameOverCoinText.text = coinScore.ToString("0");
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

        isGamePaused = true;
        pauseButton.SetActive(false);
        clickFX.Play();
        pauseMenu.SetActive(true);
        pauseMenuAnim.SetTrigger("Pause");
        Time.timeScale = 0;   
    }



    public void OnResumeGame()
    {
        isGamePaused = false;
        clickFX.Play();
        pauseMenuAnim.SetTrigger("Resume");
        Time.timeScale = 1 ;
        pauseButton.SetActive(true);

    }



    /*    
     ===============
     Level Finished
    ===============
    */

    public void OnLevelCompleted()
    {

        music.volume = 0.1f;

        victoryFX.Play();
        controller.StopRunning();
        isDead = true;
     
        levelFinished.SetActive(true);
        levelFinishedAnim.SetTrigger("Show");

        levelCompletedMenu.SetActive(true);         
        levelCompletedCoinText.text = coinScore.ToString("0");
        levelCompletedAnim.SetTrigger("LevelCompleted");
    }



    /*    
    ===============
      Main menu
    ===============
    */

    public void CloseApp()
    {
        clickFX.Play();
        Debug.Log("closing app");
        Application.Quit();
    }

    public void OnStartGame()
    {
        isGameStarted = true;
        controller.StartRunning();
        FindObjectOfType<CameraController>().isMoving = true;
        mainMenuAnim.SetTrigger("Hide");
        HUDMenu.SetActive(true);        
    }

    public void ShowCredits()
    {
        clickFX.Play();
        credits.SetActive(true);      
        mainMenuAnim.SetTrigger("Hide");
        creditsAnim.SetTrigger("showCredits");        
    }

    public void HideCredits()
    {
        clickFX.Play();
        creditsAnim.SetTrigger("hideCredits");
        ShowMainMenu();
    }



    public void ShowSettings()
    {
        clickFX.Play();
        settings.SetActive(true);     
        mainMenuAnim.SetTrigger("Hide");
        settingsAnim.SetTrigger("showSettings");
    }

    public void HideSettings()
    {
        clickFX.Play();
        settingsAnim.SetTrigger("hideSettings");
        ShowMainMenu();
    }


    public void ShowMainMenu()
    {   
        mainMenu.SetActive(true);
        mainMenuAnim.SetTrigger("Show");
    }
}
