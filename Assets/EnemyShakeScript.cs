using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShakeScript : MonoBehaviour
{
    private bool shaking;

    [SerializeField]
    private float shakeIntensity;

    private void Update()
    {
        if (shaking)
        {
            Vector3 newPos = Random.insideUnitSphere * (Time.deltaTime * shakeIntensity);
            newPos.y = transform.position.y;
            newPos.z = transform.position.z;

            transform.position = newPos;
        }
    }

    public void ShakeMe()
    {
        StartCoroutine("ShakeNow");
    }

    IEnumerator ShakeNow()
    {
        Vector3 originalPos = transform.position;

        if (!shaking)
        {
            shaking = true;
        }

        yield return new WaitForSeconds(0.25f);

        shaking = false;
        transform.position = originalPos;
    }
}
