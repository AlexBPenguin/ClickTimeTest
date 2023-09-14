using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    //this script handles everything combat

    public EnemyScriptableObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy.Print();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
