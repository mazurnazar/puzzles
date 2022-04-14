using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    public Puzzle [,] puzzles;
    [SerializeField] private Data data;
    [SerializeField] private GameObject background;
    private int rows, cols;
    public GameObject originalPos;
    public MovePuzzle movePuzzle;
    private void Start()
    {
        movePuzzle = new MovePuzzle();
        rows = 4;
        cols = 7;
        puzzles = new Puzzle[rows, cols];
        Init();
    }

    void Init()
    {
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                puzzles[i, j] = new Puzzle();
                puzzles[i,j].tile = Instantiate(tilePrefab, SetOriginalPos(i,j), Quaternion.identity);
                puzzles[i, j].tile.name ="Puzzle " + i +""+ j;

                puzzles[i, j].tile.GetComponent<Puzzle>().row = i;
                puzzles[i, j].tile.GetComponent<Puzzle>().col = j;
                puzzles[i, j].tile.GetComponent<Puzzle>().originalPos = SetOriginalPos(i, j);

                puzzles[i, j].tile.GetComponent<SpriteRenderer>().sprite = data.Sprites[index];
                puzzles[i,j].tile.AddComponent<BoxCollider2D>();
                puzzles[i, j].tile.AddComponent<Rigidbody2D>();
                puzzles[i, j].tile.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                puzzles[i, j].tile.GetComponent<BoxCollider2D>().isTrigger = true;
                index++;
            }
            
           
        }
    }
    
    Vector2 SetRandomPos()
    {
        Vector2 pos = new Vector2();
        pos.y = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        pos.x = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        return pos;
    }
    Vector2 SetOriginalPos(int row, int col)
    {
        Vector2 pos = new Vector2();
        pos.y = originalPos.transform.position.y - row * originalPos.transform.GetComponent<BoxCollider2D>().size.y;
        pos.x = originalPos.transform.position.x + col * originalPos.transform.GetComponent<BoxCollider2D>().size.x;
        
        return pos;
    }

    public void Shuffle()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                puzzles[i, j].tile.transform.position = SetRandomPos();
            }
        }
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("puzzle");
        foreach (var item in gameObjects)
        {
            item.GetComponent<MovePuzzle>().canMove = true;
        }
       
    }
}
