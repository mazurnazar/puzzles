using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Level : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public int levelNumber;
    public static int lvlNum;
    public bool isLocked;
    private void Start()
    {
        MenuManager.Instance.LoadInfo();
        isLocked = MenuManager.Instance.levels[levelNumber];

        if (isLocked)
        {
            this.GetComponent<Image>().sprite = MenuManager.Instance.locked;
        }
        else
        {
            this.GetComponent<Image>().sprite = MenuManager.Instance.background.Sprites[levelNumber];
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked)
        {

            this.GetComponent<Image>().color = Color.red;
        }
        else
        {
            lvlNum = levelNumber;
            SceneManager.LoadScene(2);

        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) return;
        this.GetComponent<Image>().color = Color.green;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        this.GetComponent<Image>().color = Color.white;
    }
}
