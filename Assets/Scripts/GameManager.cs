using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { set; get; }

    private bool isGameStarted = false; // bandera para saber si el nivel comenzo

    private PlayerController controller;

    // HUD
    public Text scoreText, coinText, modifierText;

    public float score, coinScore, modifierScore;

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
    }

    public void UpdateScores()
    {
        scoreText.text = score.ToString();
        coinText.text = coinScore.ToString();
        modifierText.text = modifierScore.ToString();
    }
}
