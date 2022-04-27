using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Initialize : MonoBehaviour
{
    [SerializeField] private GameObject backgroundPrefab;
    private Puzzle [,] puzzles;
    [SerializeField] private Data backgroundData;
    [SerializeField] private GameObject puzzleImage;
    private const int rows = 4, cols = 8;
    [SerializeField] private GameObject[] Puzzles;
    public enum Difficulty { easy, medium, hard};
    public Difficulty difficulty;
    private void Start()
    {
        difficulty = Difficulty.easy;
        puzzles = new Puzzle[rows, cols];
        Init();
    }
    void Init()
    {

        puzzleImage.GetComponent<SpriteRenderer>().sprite = backgroundData.Sprites[Level.lvlNum];
        backgroundPrefab.GetComponent<SpriteRenderer>().sprite = backgroundData.Sprites[Level.lvlNum];
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                puzzles[i, j] = Puzzles[index].GetComponent<Puzzle>();
                puzzles[i, j].Tile = Instantiate(backgroundPrefab, backgroundPrefab.transform.position, Quaternion.identity);
                puzzles[i, j].Tile.transform.SetParent(Puzzles[index].transform);
                
                puzzles[i, j].Tile.name = "Background";
                
                Puzzles[index].GetComponent<Puzzle>().Row = i;
                Puzzles[index].GetComponent<Puzzle>().Col = j;
                Puzzles[index].GetComponent<Puzzle>().OriginalPos = Puzzles[index].transform.position;

                Puzzles[index].GetComponent<Puzzle>().OriginalAngle = Puzzles[index].transform.rotation;

                index++;
            }
        }
    }

    Vector2 SetRandomPos()
    {
        Vector2 pos = new Vector2();
        float offset = 1.7f;
        pos.y = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y+offset, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y-3*offset);
        pos.x = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x+offset, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x-offset);
        return pos;
    }
    int SetRandomAngle(Difficulty dif)
    {
        int angle = 0;
        switch(dif)
        {
            case Difficulty.easy:
                angle = 0;
                break;
            case Difficulty.medium:
                int random = Random.Range(0, 2);
                angle = random == 0 ? 0 : 180;
                break;
            case Difficulty.hard:
                int random2 = Random.Range(0, 4);
                {
                    if (random2 == 0) angle = 0;
                    if (random2 == 1) angle = 90;
                    if (random2 == 2) angle = 180;
                    if (random2 == 3) angle = 270;
                }
                break;
        }
        return angle;
    }

    // shuffle puzzles after pressing button        
    public void Shuffle()
    {
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (Puzzles[index].GetComponent<MovePuzzle>().canMove && Puzzles[index].transform.parent == null)
                {
                    Puzzles[index].SetActive(true);
                    Puzzles[index].transform.position = SetRandomPos();
                    Puzzles[index].transform.Rotate(0, 0, SetRandomAngle(difficulty));
                }
                index++;
            }
        }
    }
}

