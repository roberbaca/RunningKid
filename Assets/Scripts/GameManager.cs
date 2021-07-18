using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private const int COIN_SCORE_AMOUNT = 5;
    public static GameManager Instance { set; get; }

    private bool isGameStarted = false; // bandera para saber si el nivel comenzo

    private PlayerController controller;

    // HUD
    public Text scoreText, coinText, modifierText;

    public float modifierScore;

    private int score, coinScore;

    private void Awake()
    {
        Instance = this;

        //UpdateScores();

        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // controles tactiles
        if(MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            controller.StartRunning();
        }

        if(Input.anyKey && !isGameStarted)
        {
            isGameStarted = true;
            controller.StartRunning();
        }

        if (isGameStarted)
        {
            // Bump Score Up            
            score += (int)(Time.deltaTime);
            scoreText.text = score.ToString("0");
        }

    }


    public void UpdateScores()
    {
        
        coinText.text = coinScore.ToString();
        modifierText.text = modifierScore.ToString();
    }


    /*MODIFIER SPEED*/
    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1f + modifierAmount;
        UpdateScores();
    }

    public void GetCoin()
    {
        coinScore ++;
        coinText.text = coinScore.ToString("0");
        scoreText.text = score.ToString("0");
    }

}
