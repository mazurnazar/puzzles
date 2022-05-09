using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MovePuzzle : MonoBehaviour
{
    private bool isMoving;
    public bool canMove;
    bool isNeibor;
    private float offset;
        //offset2;
    private GameObject neibor;
    private Puzzle puzzle;
   // private BoxCollider2D boxCollider2D;
    private GameManager gameManager;
    private const float xRange = 22, yMinRange = -10f, yMaxRange = 6;
    private const float distanceToOrigin = 0.7f;
    private float timeClick;
    private float puzzleSize = 1.7f;
    private void Start()
    {
        offset = .2f;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        puzzle = GetComponent<Puzzle>();
      //  boxCollider2D = GetComponent<BoxCollider2D>();
        //offset2 = puzzleSize + offset;
    }
    
    // when you drag puzzle
    private void OnMouseDrag()
    {
        if (canMove && gameManager.IsGameRunning)  // if youv can move and game is running
        {
            
            isMoving = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            //if puzzle is in range of screen
            if (mousePos.x < xRange && mousePos.x > -xRange && mousePos.y < yMaxRange && mousePos.y > yMinRange)
            {
                if (transform.parent != null && transform.parent.tag == "puzzle")
                    transform.parent.position = mousePos;
                else transform.position = mousePos;
                GetComponent<SortingGroup>().sortingOrder = 1;
            }

        }
    }
    private void OnMouseUp()
    {
        // detect short click for rotate
        if (Time.time - timeClick < 0.2f && gameManager.IsGameRunning)
        {
            if(transform.parent!=null) transform.parent.Rotate(0, 0, gameManager.AngleToRotate);
            else transform.Rotate(0, 0, gameManager.AngleToRotate);
            transform.GetComponent<Puzzle>().currentAngle += gameManager.AngleToRotate;
           
            if (transform.GetComponent<Puzzle>().currentAngle >= 360) transform.GetComponent<Puzzle>().currentAngle -= 360; 
            gameManager.GetComponent<Sound>().PlaySound("rotate");
        }
        else
        {
            gameManager.GetComponent<Sound>().PlaySound("select");
            GetComponent<SortingGroup>().sortingOrder = 0;
            isMoving = false;
            float distance = Vector2.Distance(transform.position, transform.GetComponent<Puzzle>().OriginalPos);

            // checking if puzzle is in its original position and if is then put it there and cannot move then
            if (distance < distanceToOrigin && (int)transform.eulerAngles.z == (int)transform.GetComponent<Puzzle>().OriginalAngle.eulerAngles.z)
            {
                if (transform.parent != null) // if item has parent
                {
                    // set original pos og parent puzzle and deactivate it and its children
                    transform.parent.GetComponent<MovePuzzle>().canMove = false;
                    transform.parent.position = transform.parent.GetComponent<Puzzle>().OriginalPos;
                    transform.parent.GetComponent<BoxCollider2D>().enabled = false;
                    transform.parent.GetComponent<SortingGroup>().sortingOrder = -1;
                    DeactivatePuzzles(transform.parent);
                }
                else // item doesnt have parent
                {
                    DeactivatePuzzles(transform);
                }
                transform.position = transform.GetComponent<Puzzle>().OriginalPos;
                transform.GetComponent<BoxCollider2D>().enabled = false;
                canMove = false;
                GetComponent<SortingGroup>().sortingOrder = -1;
            }
            gameManager.GetComponent<Sound>().PlaySound("put");
            gameManager.PieceSelected = false;
        }

    }
    private void OnMouseDown()
    {
        // set time of click
        gameManager.PieceSelected = true;
        timeClick = Time.time;       
    }
    void DeactivatePuzzles(Transform transform)
    {
        // deactivate puzzle and its children so they cant move
        var children = transform.GetComponentInChildren<Transform>();
        foreach (Transform item in children)
        {
            if (item.transform.tag == "puzzle")
            {
                item.GetComponent<BoxCollider2D>().enabled = false;
                item.GetComponent<MovePuzzle>().canMove = false;
                GetComponent<SortingGroup>().sortingOrder = -1;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isMoving && collision.gameObject.tag == "puzzle"
           && collision.GetComponent<Puzzle>().currentAngle == this.GetComponent<Puzzle>().currentAngle)
        {
            if (Input.GetMouseButtonUp(0))
            {
                neibor = collision.gameObject;
                IsNeighbour(collision.gameObject);
            }
            
        }
        
    }
    /* private void OnTriggerEnter2D(Collider2D other)
     {
         // if collides with other puzzle
         if (isMoving && other.gameObject.tag == "puzzle" 
             && other.GetComponent<Puzzle>().currentAngle == this.GetComponent<Puzzle>().currentAngle) 
         {
             neibor = other.gameObject;
             IsNeighbour(other.gameObject);
         }
     }*/
    void IsNeighbour(GameObject other)
    {
        switch(other.GetComponent<Puzzle>().currentAngle)
        {
            case 0:
                AngleCompare(other, "right", "left", "up", "down");
                break;
            case 90:
                AngleCompare(other, "left", "right", "up", "down");
                break;
            case 180:
                AngleCompare(other, "left", "right", "down", "up");
                break;
            case 270:
                AngleCompare(other, "righ", "left", "down", "up");
                break;
        }
    }
    // comparing in what angle position neibor puzzle is
    void AngleCompare(GameObject other, string right, string left, string up, string down)
    {
        int rowOther = other.GetComponent<Puzzle>().Row;
        int colOther = other.GetComponent<Puzzle>().Col;
        int rowThis = puzzle.Row;
        int colThis = puzzle.Col;

        if (rowOther == rowThis) // if same row
        {
            if (other.GetComponent<Puzzle>().currentAngle == 90 || other.GetComponent<Puzzle>().currentAngle == 270)
            {
                if (colOther == colThis + 1) // if next column
                {
                    ComparePositionsY(other, down);
                }
                else if (colOther == colThis - 1) // if previous column
                {
                    ComparePositionsY(other, up);
                }
            }
            else
            {
                if (colOther == colThis + 1)
                {
                    ComparePositionsX(other, right);
                }
                else if (colOther == colThis - 1)
                {
                    ComparePositionsX(other, left);
                }
            }
        }

        if (colOther == colThis) // if same column
        {
            if (other.GetComponent<Puzzle>().currentAngle == 90 || other.GetComponent<Puzzle>().currentAngle == 270)
            {
                if (rowOther == rowThis + 1)// if next row
                {
                    ComparePositionsX(other, left);
                }
                else if (rowOther == rowThis - 1)// if previous row
                {
                    ComparePositionsX(other, right);
                }
            }
            else
            {
                if (rowOther == rowThis + 1)
                {
                    ComparePositionsY(other, up);
                }
                else if (rowOther == rowThis - 1)
                {
                    ComparePositionsY(other, down);
                }
            }
        }
    }
    //comparing 'x' positions of two puzzles
    void ComparePositionsX(GameObject other, string direction)
    {
        float differenceX = transform.position.x - other.transform.position.x;
        float differenceY = transform.position.y - other.transform.position.y;

        if (Mathf.Abs(differenceY) < offset )// if  -offset < y < offset
        {
            if((direction == "left" && differenceX > 0) || (direction == "right" && differenceX < 0))
            {
                  ParentPos(other);
                  SetParent();
            }
        }
    }
    //comparing 'y' positions of two puzzles
    void ComparePositionsY(GameObject other, string direction)
    {
        float differenceX = transform.position.x - other.transform.position.x;
        float differenceY = transform.position.y - other.transform.position.y;

        if (Mathf.Abs(differenceX) < offset ) // if  -offset < x < offset
        {
            if ((direction == "down" && differenceY < 0) || (direction == "up" && differenceY > 0))
            {
                ParentPos(other);
                SetParent();
            }
        }
    }
    // set parent position, and all children with it
    void ParentPos(GameObject other)
    {
        int signX = 1, signY = 1;
        switch(other.GetComponent<Puzzle>().currentAngle)
        {
            case 90:
                signY = -1;
                break;
            case 180:
                signX = -1;
                signY = -1;
                break;
            case 270:
                signX = -1;
                break;

        }

        if (other.GetComponent<Puzzle>().currentAngle == 90 || other.GetComponent<Puzzle>().currentAngle == 270)
        {
            if (transform.parent != null)
            {
                transform.parent.position = other.transform.position
                            + new Vector3(signX * (transform.parent.GetComponent<Puzzle>().Row - other.GetComponent<Puzzle>().Row) * puzzleSize * 2,
                            signY * (-transform.parent.GetComponent<Puzzle>().Col + other.GetComponent<Puzzle>().Col) * puzzleSize * 2);
            }
            else
                transform.position = other.transform.position
                            + new Vector3(signX * (transform.GetComponent<Puzzle>().Row - other.GetComponent<Puzzle>().Row) * puzzleSize * 2,
                            signY * (-transform.GetComponent<Puzzle>().Col + other.GetComponent<Puzzle>().Col) * puzzleSize * 2);
        }
        else
        {
            if (transform.parent != null)
            {
                transform.parent.position = other.transform.position
                            + new Vector3(signX * (transform.parent.GetComponent<Puzzle>().Col - other.GetComponent<Puzzle>().Col) * puzzleSize * 2,
                            signY * (-transform.parent.GetComponent<Puzzle>().Row + other.GetComponent<Puzzle>().Row) * puzzleSize * 2);
            }
            else
                transform.position = other.transform.position
                            + new Vector3(signX * (transform.GetComponent<Puzzle>().Col - other.GetComponent<Puzzle>().Col) * puzzleSize * 2,
                            signY * (-transform.GetComponent<Puzzle>().Row + other.GetComponent<Puzzle>().Row) * puzzleSize * 2);
        }
    }
    
    // setting parent
    void SetParent()
    {
        if (transform.parent != null) // if moving puzzle has parent
        {
            if (neibor.transform.parent != null) // if neibor to connect has parent
            {
                NeiborParent(neibor.transform.parent, transform.parent);
            }
            else // if neibor to connect doesnt have parent
            {
                NeiborParent(neibor.transform, transform.parent);
            }
        }
        else // if moving puzzle doesnt have parent
        {
            if (neibor.transform.parent != null) // if neibor to connect has parent
            {
                NeiborParent(neibor.transform.parent, transform);
            }
            else // if neibor to connect doesnt have parent
            {
                NeiborParent(neibor.transform, transform);
            }   
        }
    }
    // set all children to new parent
    void NeiborParent(Transform neibor, Transform parent)
    {
        parent.SetParent(neibor);
        foreach (Transform item in parent.GetComponentInChildren<Transform>())
        {
            if (item.tag == "puzzle")
                item.SetParent(neibor);
        }
    }
}
