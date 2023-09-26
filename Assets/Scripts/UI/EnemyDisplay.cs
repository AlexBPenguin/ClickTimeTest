using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyDisplay : MonoBehaviour
{
    //This script displays enemy health and posture bars, enemy name, and the enemy sprite

    public EnemyScriptableObject enemy;

    //Bars
    public Slider enemyHealthSlider;
    public Slider enemyPostureSlider;

    //Text
    public TMP_Text nameText;
    public TMP_Text livesText;

    //set health and posture bars to max and set name text to enemy name
    private void Start()
    {
        //Bars
        SetMaxHealth();
        SetMaxPosture();

        //Text
        nameText.text = enemy.name;
        livesText.text = "Lives: " + enemy.lives.ToString();
    }

    //Health
    public void SetMaxHealth()
    {
        enemyHealthSlider.maxValue = enemy.health;
        enemyHealthSlider.value = enemy.health;
    }

    public void SetHealth(int health)
    {
        enemyHealthSlider.value = health;
    }

    //Posture
    public void SetMaxPosture()
    {
        enemyPostureSlider.maxValue = enemy.posture;
    }

    public void SetPosture(int posture)
    {
        enemyPostureSlider.value = posture;
    }

    public void SetLives(int lives)
    {
        livesText.text = "Lives: " + lives.ToString();
    }
}
