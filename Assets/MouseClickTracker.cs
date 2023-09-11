using System.Collections.Generic;
using UnityEngine;

public class MouseClickTracker : MonoBehaviour
{
    public List<float> clickTimestamps = new List<float>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            clickTimestamps.Add(Time.time);
        }
    }
}
