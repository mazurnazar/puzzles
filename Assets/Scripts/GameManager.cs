using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Initialize init;
    private Timer timer;
    private bool isGameOver;
    [SerializeField] private GameObject[] puzzles;

    private bool isGameRunning;
    public bool IsGameRunning { get => isGameRunning; private set { } }

    private bool pieceSelected;
    public bool PieceSelected { get => pieceSelected; set { value = pieceSelected; } }

   
    private int angleToRotate = 0;
    public int AngleToRotate { get => angleToRotate; private set { } }


    [SerializeField] 
    private GameObject startPanel;

    [SerializeField] 
    private Text gameOverText;

    [SerializeField] 
    private GameObject gameOverPanel;

    [SerializeField] 
    private GameObject pausePanel;

    [SerializeField]
    private GameObject background;

    [SerializeField] 
    private Scrollbar scrollbar;


    private void Start()
    {
        timer = GetComponent<Timer>();
        init = GameObject.Find("InitializeObjects").GetComponent<Initialize>();
        startPanel.gameObject.SetActive(true);
        MenuManager.Instance.LoadInfo();
    }
    void GameOver()
    {
        puzzles = GameObject.FindGameObjectsWithTag("puzzle");
        foreach (var item in puzzles)
        {
            if (item.GetComponent<MovePuzzle>().canMove)
            {
                isGameOver = false;
                return;
            }
        }

        gameOverPanel.gameObject.SetActive(true);
        StopAllCoroutines();
        gameOverText.text = "Win";
        isGameOver = true;
        timer.TimerIsRunning = false;
        if (Level.lvlNum < MenuManager.Instance.Levels.Length-1)
        {
            MenuManager.Instance.Levels[Level.lvlNum + 1] = false;
        }
        CheckBestTime();
        //ads.ShowInterstitialAd();
    }
    void CheckBestTime()
    {
        if (MenuManager.Instance.Time[Level.lvlNum] == 0 || timer._Time < MenuManager.Instance.Time[Level.lvlNum])
        {
            MenuManager.Instance.Time[Level.lvlNum] = timer._Time;
        }
        MenuManager.Instance.SaveInfo();
    }
    IEnumerator gameOver()
    {
        if (!isGameOver)
        {
            yield return null;
            GameOver();
            StartCoroutine(gameOver());
        }
    }
    public void Difficulty()
    {
        var dif = init.difficulty;
        if (scrollbar.value < 0.26) init.difficulty = Initialize.Difficulty.easy;
        else if (scrollbar.value < 0.75)
        {
            init.difficulty = Initialize.Difficulty.medium;
            angleToRotate = 180;
        }
        else
        {
            init.difficulty = Initialize.Difficulty.hard;
            angleToRotate = 90;
        }
    }
    public void ShuffleButton()
    {
        StopAllCoroutines();
        startPanel.gameObject.SetActive(false); // deactivate button after press
        isGameRunning = true;
        timer.TimerIsRunning = true;
        init.Shuffle(); 
        StartCoroutine(gameOver());
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(2);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void PausePlay()
    {
        if (!pausePanel.gameObject.activeSelf)
        {
            pausePanel.gameObject.SetActive(true);
            isGameRunning = false;
            timer.TimerIsRunning = false;
        }
        else
        {
            pausePanel.gameObject.SetActive(false);
            isGameRunning = true;
            timer.TimerIsRunning = true;
        }
    }
    public void Hint()
    {
        StartCoroutine(ShowBackground());
    }
    IEnumerator ShowBackground()
    {
        background.SetActive(true);
        yield return new WaitForSeconds(5);
        background.SetActive(false);
    }


}
