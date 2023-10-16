using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] CameraMove cameraMove;
    [SerializeField] Blocker blocker;
    [SerializeField] EnemyShakeScript enemyShakeScript;
    [SerializeField] PlayerSpriteHandler playerSpriteHandler;
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
            if (!blocker.enemyDowned && cameraMove.returned)
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


            if (!blocker.enemyDowned && cameraMove.returned)
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
            if (!blocker.enemyDowned && cameraMove.returned)
            {
                if (!cameraMove.forwardButtonPressed)
                {
                    cameraMove.Invoke("SetForwardButtonPressedFalse", 0.35f);
                    //blocker.dodgeSlam = true;

                    cameraMove.returned = false;
                    playerSpriteHandler.ShieldBashArt();
                    playerSpriteHandler.DodgeSwordArt();
                    blocker.Flash();
                    enemyShakeScript.ShakeMe();
                }

                cameraMove.forwardButtonPressed = true;

            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            //Debug.Log("UpButtonUp");
        }

        //DOWN
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Debug.Log("DownButtonDown");
            if (!blocker.enemyDowned && cameraMove.returned)
            {
                if (!cameraMove.backButtonPressed)
                {
                    cameraMove.Invoke("SetBackButtonPressedFalse", 0.35f);
                    blocker.dodgeSlam = true;

                    cameraMove.returned = false;
                    playerSpriteHandler.DodgeShieldArt();
                    playerSpriteHandler.DodgeSwordArt();
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
