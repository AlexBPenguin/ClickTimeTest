using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour
{

    public Slider enemySlider;

    public Slider playerSlider;
    
    public void SetMaxHealth(int health)
    {
        enemySlider.maxValue = health;
        enemySlider.value = health;
    }

    public void SetMaxPlayerHealth(int health)
    {
        playerSlider.maxValue = health;
        playerSlider.value = health;
    }

    public void SetHealth(int health)
    {
        enemySlider.value = health;
    }

    public void SetPlayerHealth(int health)
    {
        playerSlider.value = health;
    }
}
