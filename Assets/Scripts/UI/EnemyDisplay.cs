using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyDisplay : MonoBehaviour
{
    public EnemyScriptableObject enemy;

    //Bars
    public Slider enemyHealthSlider;
    public Slider enemyPostureSlider;

    //Text
    public TMP_Text nameText;

    //Sprite
    public SpriteRenderer sprite;

    private void Start()
    {
        //Bars
        SetMaxHealth();
        SetMaxPosture();

        //Text
        nameText.text = enemy.name;

        //Sprite
        sprite.sprite = enemy.neutralSprite;
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
}
