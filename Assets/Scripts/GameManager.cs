using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    private const int COIN_SCORE_AMOUNT = 5;
    public static GameManager Instance { set; get; }

    public bool isGameStarted = false; // bandera para saber si el nivel comenzo
    public bool isDead = false;
    private PlayerController controller;

    // HUD
    public Animator gameCanvas, coinAnim;         
    private int score, coinScore;
    public GameObject HUDMenu;

    // Main Menu
    public Animator mainMenuAnim;

    // Game Over Menu
    public Animator gameOverMenuAnim;
    public TextMeshProUGUI gameOverCoinText, coinText;
    public GameObject gameOverMenu;

    // Pause Menu
    public Animator pauseMenuAnim;
    public GameObject pauseMenu;


    private void Awake()
    {
        Instance = this;

        //UpdateScores();

        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
            isGameStarted = true;
            controller.StartRunning();
            FindObjectOfType<CameraController>().isMoving = true;
            //gameCanvas.SetTrigger("Show");
            mainMenuAnim.SetTrigger("Hide");
            HUDMenu.SetActive(true);
        }

        if(Input.anyKey && !isGameStarted)
        {
            isGameStarted = true;
            controller.StartRunning();
            FindObjectOfType<CameraController>().isMoving = true;
            //gameCanvas.SetTrigger("Show");
            mainMenuAnim.SetTrigger("Hide");
            HUDMenu.SetActive(true);
        }

        if (isGameStarted)
        {
            // Bump Score Up            
            score += (int)(Time.deltaTime); // DESACTIVAR !!!
            //scoreText.text = score.ToString("0");
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            OnPauseGame();
        }

        

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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnGameOver()
    {

        isDead = true;
        //gameOverScoreText.text = score.ToString("0");
        gameOverMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverCoinText.text = coinScore.ToString("0");
        gameOverMenuAnim.SetTrigger("GameOver");
    }

    public void OnPauseGame()
    {
        if (isDead)
        {
            return;
        }

        // pausamos el juego
        pauseMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenuAnim.SetTrigger("Pause");
        Time.timeScale = 0;                    
        
    }

    public void OnResumeGame()
    {
        pauseMenuAnim.SetTrigger("Resume");
        Time.timeScale = 1 ;
        
        //pauseMenu.SetActive(false);
        
        
    }
}
