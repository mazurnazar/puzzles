using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle: MonoBehaviour
{
    private int row, col;
    private Vector2 originalPos;
    private Quaternion originalAngle;
    public int currentAngle;
    private GameObject tile;

    public int Row { get => row; set { row = value; } }
    public int Col { get => col; set { col = value; } }
    public Vector2 OriginalPos { get => originalPos; set { originalPos = value; } }
    public Quaternion OriginalAngle { get => originalAngle; set { originalAngle = value; } }
    public GameObject Tile{ get => tile; set { tile = value; } }



}
