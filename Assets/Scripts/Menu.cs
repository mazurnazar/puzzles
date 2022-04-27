using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{

    [SerializeField] GameObject BestScorePanel;
    [SerializeField] Text BestScoresText;
    [SerializeField] GameObject music, sound, donate;
    [SerializeField] GameObject volume;
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
 
    public void ShowBestScores()
    {
        BestScoresText.text = "";
        for (int i = 0; i < MenuManager.Instance.TotalLevels; i++)
        {
            float minutes = Mathf.FloorToInt(MenuManager.Instance.Time[i] / 60);
            float seconds = Mathf.FloorToInt(MenuManager.Instance.Time[i] % 60);
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
        if (!music.gameObject.activeSelf)
        {
            music.SetActive(true);
            sound.SetActive(true);
            donate.SetActive(true);
        }else
        {
            music.SetActive(false);
            sound.SetActive(false);
            donate.SetActive(false);
        }
    }

    public void Music()
    {
        if(volume.gameObject.activeSelf)
        volume.gameObject.SetActive(false);
        else
            volume.gameObject.SetActive(true);
    }
    public void Sound()
    {
        if (MenuManager.Instance.IsSoundOn)
        {
            MenuManager.Instance.IsSoundOn = false;
            sound.GetComponent<Image>().color = Color.red;
        }
        else
        {
            MenuManager.Instance.IsSoundOn = true;
            sound.GetComponent<Image>().color = Color.white;
        }


    }
    public void ChangeVolume()
    {
        if (volume.GetComponent<Scrollbar>().value == 0)
            music.GetComponent<Image>().color = Color.red;
        else
            music.GetComponent<Image>().color = Color.white;
        MenuManager.Instance.GetComponent<AudioSource>().volume = volume.GetComponent<Scrollbar>().value;
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
