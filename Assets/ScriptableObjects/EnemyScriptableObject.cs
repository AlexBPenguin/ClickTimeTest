using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField]
    public Sprite neutralSprite;
    public new string name;
    public int health;
    public int posture;

    public GameObject[] atkCombos;
    public Sprite[] atkComboSprites;

    public GameObject[] counterAtks;
    public Sprite[] counterAtksComboSprites;
    
    public void Print()
    {
        Debug.Log("Name: " + name + ", Health: " + health + ", Posture: " + posture);
    }
    
}
