using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private float axisX;

    void Update()
    {
        axisX = Input.GetAxis("Vertical");
    }
}
