using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform backButtonTransform;
    [SerializeField] Transform forwardButtonTransform;
    [SerializeField] Vector3 startPos;
    [SerializeField] float speed;
    public bool backButtonPressed;
    public bool forwardButtonPressed;

    public bool returned;

    [SerializeField] Blocker blocker;
    [SerializeField] PlayerSpriteHandler playerSpriteHandler;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (backButtonPressed)
        {
            MoveCameraBack();
        }

        else if (forwardButtonPressed)
        {
            MoveCameraForward();
        }

        else
        {
            MoveCameraToStart();
        }
    }

    public void MoveCameraBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, backButtonTransform.position, (speed * Time.deltaTime));
    }

    public void MoveCameraForward()
    {
        transform.position = Vector3.MoveTowards(transform.position, forwardButtonTransform.position, (speed * Time.deltaTime));
    }

    public void MoveCameraToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPos, (speed * Time.deltaTime));
        if (!returned)
        {
            playerSpriteHandler.NeutralShieldArt();
            playerSpriteHandler.NeutralSwordArt();
            returned = true;
        }

    }

    public void SetBackButtonPressedFalse()
    {
        backButtonPressed = false;
        blocker.dodgeSlam = false;
        
    }
    public void SetForwardButtonPressedFalse()
    {
        forwardButtonPressed = false;
        //blocker.dodgeSlam = false;

    }
}
