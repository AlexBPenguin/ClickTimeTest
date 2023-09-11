using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostureBars : MonoBehaviour
{
    public Slider enemyPostureSlider;
    public Slider playerPostureSlider;

    public void SetMaxEnemyPosture(int posture)
    {
        enemyPostureSlider.maxValue = posture;
    }

    public void SetEnemyPosture(int posture)
    {
        enemyPostureSlider.value = posture;
    }

    public void SetMaxPlayerPosture(int posture)
    {
        playerPostureSlider.maxValue = posture;
    }

    public void SetPlayerPosture(int posture)
    {
        playerPostureSlider.value = posture;
    }
}
