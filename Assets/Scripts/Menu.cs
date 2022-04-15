using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public Button newGame, exit, rankings;
    public GameObject BestScorePanel;
    public Text BestScoresText;
    public GameObject music, sound, donate;
    // Start is called before the first frame update
    public void ShowBestScores()
    {
        BestScoresText.text = "";
        for (int i = 0; i < 6; i++)
        {
            float minutes = Mathf.FloorToInt(MenuManager.Instance.time[i] / 60);
            float seconds = Mathf.FloorToInt(MenuManager.Instance.time[i] % 60);
            string time =  string.Format("{0:00}:{1:00}", minutes, seconds);
            BestScoresText.text += "Level " + (i+1) + ": " + time + "\n";
        }
    }
    public void NewGame()
    {
        SceneManager.LoadScene("LevelsScene");
    }
    public void BestScore()
    {
        BestScorePanel.SetActive(true);
        ShowBestScores();
    }
    public void ExitScores()
    {
        BestScorePanel.SetActive(false);
    }
    public void Settings()
    {
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }
}
