using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCombo : MonoBehaviour
{
    public float comboMoveSpeed;
    public bool moveCombo;
    public float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("DestroyThisObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!moveCombo)
            {
                moveCombo = true;
            }
            else if (moveCombo)
            {
                moveCombo = false;
            }
        }

        if (moveCombo)
        {
            transform.Translate(Vector3.down * comboMoveSpeed * Time.deltaTime);
        }*/

        if (moveCombo)
        {
            transform.Translate(Vector3.down * comboMoveSpeed * Time.deltaTime);
        }

    }

    /*
    private void DestroyThisObject()
    {
        Destroy(gameObject);
    }*/
}
