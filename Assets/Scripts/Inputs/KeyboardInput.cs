using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] CameraMove cameraMove;
    [SerializeField] Blocker blocker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //BLOCK
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log("BlockButtonDown");
            if (!blocker.enemyDowned)
            {
                blocker.BlockButton();
            }

        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            //Debug.Log("BlockButtonUp");
            blocker.BlockButtonUp();
        }

        //ATTACK
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Debug.Log("AttackButtonDown");


            if (!blocker.enemyDowned)
            {
                blocker.AttackButton();
            }

        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            //Debug.Log("AttackButtonUp");
        }

        //UP
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Debug.Log("UpButtonDown");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            //Debug.Log("UpButtonUp");
        }

        //DOWN
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Debug.Log("DownButtonDown");
            if (!blocker.enemyDowned)
            {
                if (!cameraMove.backButtonPressed)
                {
                    cameraMove.Invoke("SetBackButtonPressedFalse", 0.25f);
                    blocker.dodgeSlam = true;
                }

                cameraMove.backButtonPressed = true;
            }


        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            //Debug.Log("DownButtonUp");
        }
    }
}
