using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePuzzle : MonoBehaviour
{
    bool isMoving;
    public bool canMove;
    bool isNeibor;
    float offset = .2f;
    float offset2;
    GameObject neibor;
    public Puzzle puzzle;
    public BoxCollider2D boxCollider2D;
    public Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;
    private void Start()
    {
         
        puzzle = GetComponent<Puzzle>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        offset2 = boxCollider2D.size.x + offset;
    }
    private void OnMouseDrag()
    {
        if (canMove)
        {
            spriteRenderer.color = Color.green;
            isMoving = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            if (mousePos.x < Screen.width / 2 && mousePos.x > -Screen.width / 2)
            {
                if (transform.parent != null)
                    transform.parent.position = mousePos;
                else transform.position = mousePos;
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
   }
    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        isMoving = false;

        float distance = Vector2.Distance(transform.position, transform.GetComponent<Puzzle>().originalPos);

        if (distance < 0.5f)
        {
            transform.position = transform.GetComponent<Puzzle>().originalPos;
            rigidbody2D.simulated = false;
            
            if (transform.parent != null) // item has parent
            {
                Debug.Log("has parent");
                transform.parent.GetComponent<Rigidbody2D>().simulated = false;

                transform.parent.position = transform.parent.GetComponent<Puzzle>().originalPos;
                DeactivatePuzzles(transform.parent);
            }
            else // item doesnt have parent
            {
                Debug.Log("no parent");
                transform.GetComponentInChildren<Rigidbody2D>().simulated = false;
                transform.GetComponentInChildren<Transform>().position = transform.GetComponentInChildren<Puzzle>().originalPos;
                DeactivatePuzzles(transform);
            }

        }
        spriteRenderer.color = Color.white;
    }
    void DeactivatePuzzles(Transform transform)
    {
        var rowParent = transform.GetComponent<Puzzle>().row;
        var colParent = transform.GetComponent<Puzzle>().col;

        Debug.Log("" + transform.childCount);
        int i = 0;
        foreach (Transform item in transform.GetComponentInChildren<Transform>())
        {
            item.position = item.GetComponent<Puzzle>().originalPos;
           // item.SetParent(null);
            item.GetComponent<Rigidbody2D>().simulated = false;

            Debug.Log("itearaion " + i);
            i++;
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
        int rowOther = other.GetComponent<Puzzle>().row;
        int colOther = other.GetComponent<Puzzle>().col;
        int rowThis = puzzle.row;
        int colThis = puzzle.col;

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
            if(direction == "left")
            {
                if (differenceX > 0)
                {
                    SetParent();
                    transform.position = other.transform.position + new Vector3(boxCollider2D.size.x, 0);
                }
            }
            else 
            {
                if (differenceX < 0)
                {
                    SetParent();
                    transform.position = other.transform.position - new Vector3(boxCollider2D.size.x, 0);
                }
            }
        }
    }
   void ComparePositionsY(GameObject other, string direction)
    {
        float differenceX = transform.position.x - other.transform.position.x;
        float differenceY = transform.position.y - other.transform.position.y;

        if (differenceX < offset && differenceX > -offset) // if  -offset < x < offset
        {
            if (direction == "down")
            {
                if (differenceY < 0) // if  y<0 
                {
                    SetParent();
                    transform.position = other.transform.position - new Vector3(0, boxCollider2D.size.y);
                }
            }
            else
            {
                if (differenceY > 0)
                {
                    SetParent();
                    transform.position = other.transform.position + new Vector3(0, boxCollider2D.size.y);
                }
            }
        }
    }

    void SetParent()
    {
        if (neibor.transform.parent != null)
            this.transform.SetParent(neibor.transform.parent);
        else this.transform.SetParent(neibor.transform);
    }
}
