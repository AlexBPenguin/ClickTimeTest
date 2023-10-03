using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform backButtonTransform;
    [SerializeField] Vector3 startPos;
    [SerializeField] float speed;
    public bool backButtonPressed;

    [SerializeField] Blocker blocker;
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

        else
        {
            MoveCameraToStart();
        }
    }

    public void MoveCameraBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, backButtonTransform.position, (speed * Time.deltaTime));
    }

    public void MoveCameraToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPos, (speed * Time.deltaTime));
    }

    public void SetBackButtonPressedFalse()
    {
        backButtonPressed = false;
        blocker.dodgeSlam = false;
        
    }
}
