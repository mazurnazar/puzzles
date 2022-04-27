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
    private float offset, offset2;
    private GameObject neibor;
    private Puzzle puzzle;
    private BoxCollider2D boxCollider2D;
    private new Rigidbody2D rigidbody2D;
    private GameManager gameManager;
    private const float xRange = 22, yMinRange = -10f, yMaxRange = 6;
    private const float distanceToOrigin = 0.7f;
    private float timeClick;

    private void Start()
    {
        offset = .2f;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        puzzle = GetComponent<Puzzle>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        offset2 = boxCollider2D.size.x + offset;
    }
    
    private void OnMouseDrag()
    {
        if (canMove && gameManager.IsGameRunning) 
        {
            
            isMoving = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

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
        
        if (Time.time - timeClick < 0.2f && gameManager.IsGameRunning)
        {
            transform.Rotate(0, 0, gameManager.AngleToRotate);
            gameManager.GetComponent<Sound>().PlaySound("rotate");
        }
        else
        {
            gameManager.GetComponent<Sound>().PlaySound("select");
            GetComponent<SortingGroup>().sortingOrder = 0;
            isMoving = false;
            float distance = Vector2.Distance(transform.position, transform.GetComponent<Puzzle>().OriginalPos);
            if (distance < distanceToOrigin && (int)transform.eulerAngles.z == (int)transform.GetComponent<Puzzle>().OriginalAngle.eulerAngles.z)
            {
                if (transform.parent != null) // item has parent
                {
                    
                    transform.parent.GetComponent<MovePuzzle>().canMove = false;
                    transform.parent.position = transform.parent.GetComponent<Puzzle>().OriginalPos;
                    transform.parent.GetComponent<BoxCollider2D>().enabled = false;
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
        gameManager.PieceSelected = true;
        timeClick = Time.time;       
    }
    void DeactivatePuzzles(Transform transform)
    {
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isMoving && other.gameObject.tag == "puzzle") 
        {
            neibor = other.gameObject;
            IsNeighbour(other.gameObject);
        }
    }
    void IsNeighbour(GameObject other)
    {
        int rowOther = other.GetComponent<Puzzle>().Row;
        int colOther = other.GetComponent<Puzzle>().Col;
        int rowThis = puzzle.Row;
        int colThis = puzzle.Col;

        if (rowOther == rowThis)
        {
            if(colOther == colThis+1)
            {
              ComparePositionsX(other, "right");
            }
            else if(colOther == colThis - 1)
            {
                ComparePositionsX(other, "left");
            }
        }

        if (colOther == colThis)
        {
            if (rowOther == rowThis + 1  )
            {
               ComparePositionsY(other, "up");
            }
            else if (rowOther == rowThis - 1)
            {
                ComparePositionsY(other, "down");
            }
        }
    }

    void ComparePositionsX(GameObject other, string direction)
    {
        float differenceX = transform.position.x - other.transform.position.x;
        float differenceY = transform.position.y - other.transform.position.y;

        if (differenceY < offset && differenceY > -offset)
        {
            if((direction == "left" && differenceX > 0) || (direction == "right" && differenceX < 0))
            {
                  ParentPos(other);
                  SetParent();
            }
        }
    }
    void ComparePositionsY(GameObject other, string direction)
    {
        float differenceX = transform.position.x - other.transform.position.x;
        float differenceY = transform.position.y - other.transform.position.y;

        if (differenceX < offset && differenceX > -offset) // if  -offset < x < offset
        {
            if ((direction == "down" && differenceY < 0) || (direction == "up" && differenceY > 0))
            {
                ParentPos(other);
                SetParent();
            }
        }
    }
    void ParentPos(GameObject other)
    {
        if (transform.parent != null)
        {
            transform.parent.position = other.transform.position
                        + new Vector3((transform.parent.GetComponent<Puzzle>().Col - other.GetComponent<Puzzle>().Col) * boxCollider2D.size.x * 2,
                        (-transform.parent.GetComponent<Puzzle>().Row + other.GetComponent<Puzzle>().Row) * boxCollider2D.size.y * 2);
        }
        else
            transform.position = other.transform.position
                        + new Vector3((transform.GetComponent<Puzzle>().Col - other.GetComponent<Puzzle>().Col) * boxCollider2D.size.x * 2,
                        (-transform.GetComponent<Puzzle>().Row + other.GetComponent<Puzzle>().Row) * boxCollider2D.size.y * 2);

    }
    

    void SetParent()
    {
        if (transform.parent != null)
        {
            if (neibor.transform.parent != null)
            {
                NeiborParent(neibor.transform.parent, transform.parent);
            }
            else
            {
                NeiborParent(neibor.transform, transform.parent);
            }
        }
        else
        {
            if (neibor.transform.parent != null)
            {
                NeiborParent(neibor.transform.parent, transform);
            }
            else
            {
                NeiborParent(neibor.transform, transform);
            }   
        }
    }
    void NeiborParent(Transform neibor, Transform parent)
    {
        neibor.SetParent(parent);
        foreach (Transform item in neibor.GetComponentInChildren<Transform>())
        {
            if (item.tag == "puzzle")
                item.SetParent(parent);
        }
    }
}
