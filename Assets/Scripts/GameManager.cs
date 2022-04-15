using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Initialize init;
    public Timer timer;
    public bool isGameOver;
    GameObject[] puzzles;
    public Button button;
    public Text gameOverText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public bool isGameRunning;
    public GameObject background;
    private void Start()
    {
        timer = GetComponent<Timer>();
        init = GameObject.Find("InitializeObjects").GetComponent<Initialize>();
        button.gameObject.SetActive(true);
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
        timer.timerIsRunning = false;
        MenuManager.Instance.levels[Level.lvlNum+1] = false;
        CheckBestTime();
        MenuManager.Instance.SaveInfo();
    }
    void CheckBestTime()
    {
        if (timer.time > MenuManager.Instance.time[Level.lvlNum])
            MenuManager.Instance.time[Level.lvlNum] = timer.time;
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
    public void ShuffleButton()
    {
        StopAllCoroutines();
        button.gameObject.SetActive(false); // deactivate button after press
        isGameRunning = true;
        timer.timerIsRunning = true;
        puzzles = GameObject.FindGameObjectsWithTag("puzzle");

        foreach (var item in puzzles)
        {
            item.GetComponent<MovePuzzle>().canMove = true;
            item.GetComponent<Rigidbody2D>().simulated = true;
            item.transform.parent = null;
        }
        init.Shuffle();
        StartCoroutine(gameOver());
    }
    public void Repeat()
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
            timer.timerIsRunning = false;
        }
        else
        {
            pausePanel.gameObject.SetActive(false);
            isGameRunning = true;
            timer.timerIsRunning = true;
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
