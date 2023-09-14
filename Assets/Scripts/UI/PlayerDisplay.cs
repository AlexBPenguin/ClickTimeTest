using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    //This script displays player health and posture bars and the player sprites

    public int playerMaxHealth;
    public int playerMaxPosture;

    //Bars
    public Slider playerHealthSlider;
    public Slider playerPostureSlider;

    //set health and posture bars to max
    private void Start()
    {
        //Bars
        SetMaxHealth();
        SetMaxPosture();
    }

    //Health
    public void SetMaxHealth()
    {
        playerHealthSlider.maxValue = playerMaxHealth;
        playerHealthSlider.value = playerMaxHealth;
    }

    public void SetHealth(int health)
    {
        playerHealthSlider.value = health;
    }

    //Posture
    public void SetMaxPosture()
    {
        playerPostureSlider.maxValue = playerMaxPosture;
    }

    public void SetPosture(int posture)
    {
        playerPostureSlider.value = posture;
    }
}
