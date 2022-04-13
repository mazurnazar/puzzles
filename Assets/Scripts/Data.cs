using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data", fileName = "Data")]
public class Data : ScriptableObject
{
    [SerializeField] private Sprite[] sprites;
    public Sprite[] Sprites { get =>sprites; private set { } }
}
