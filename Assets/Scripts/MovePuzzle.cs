using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePuzzle : MonoBehaviour
{
    public bool isMoving;
    public bool isNeibor;
    public float offset = .2f;
    public float offset2 = 2.8f;
    public GameObject neibor;
    private void Start()
    {

    }
    private void OnMouseDrag()
    {
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
    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        isMoving = false;
    }
    private void OnMouseDown()
    {
           // SetParent();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isMoving&&other.gameObject.tag == "puzzle")
        {
            Debug.Log(other.gameObject.name);
            neibor = other.gameObject;
            IsNeighbour(other.gameObject, this.gameObject);
            
        }

            
    }
    void IsNeighbour(GameObject first, GameObject second)
    {
        int rowFirst = first.GetComponent<Puzzle>().row;
        int colFirst = first.GetComponent<Puzzle>().col;
        int rowSecond = second.GetComponent<Puzzle>().row;
        int colSecond = second.GetComponent<Puzzle>().col;

        if (rowFirst == rowSecond)
        {
            if(colFirst == colSecond+1|| colFirst == colSecond - 1)
            {
              ComparePositionsX(first, second);
            }
        }

        if (colFirst == colSecond)
        {
            if (rowFirst == rowSecond + 1 || rowFirst == rowSecond - 1)
            {
               ComparePositionsY(first, second);
            }
        }
    }
    void ComparePositionsX(GameObject first, GameObject second)
    {
        float differenceX = second.transform.position.x - first.transform.position.x;
        float differenceY = second.transform.position.y - first.transform.position.y;
        if (differenceY < offset && differenceY > -offset)
        {
            if (Mathf.Abs(differenceX) < offset2 && Mathf.Abs(differenceX) > 1)
            {
                SetParent();
                if(differenceX<1)
                second.transform.position = first.transform.position - new Vector3(transform.GetComponent<BoxCollider2D>().size.x, 0);
                else second.transform.position = first.transform.position + new Vector3(transform.GetComponent<BoxCollider2D>().size.x, 0);
            }
        }
    }
   void ComparePositionsY(GameObject first, GameObject second)
    {
        float differenceX = second.transform.position.x - first.transform.position.x;
        float differenceY = second.transform.position.y - first.transform.position.y;

        if (differenceX < offset && differenceX > -offset)
        {
            if (Mathf.Abs(differenceY) < offset2 && Mathf.Abs(differenceY) > 1)
            {
                SetParent();
                if(differenceY<0)
                second.transform.position = first.transform.position -  new Vector3(0, transform.GetComponent<BoxCollider2D>().size.y);
                else second.transform.position = first.transform.position +  new Vector3(0, transform.GetComponent<BoxCollider2D>().size.y);

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
