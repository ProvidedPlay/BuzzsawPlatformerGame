using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPad : MonoBehaviour
{
    public bool isUp, isDown, isLeft, isRight;

    float lastX, lastY;

    private void Update()
    {
        float x = Input.GetAxis("DPad X");
        float y = Input.GetAxis("DPad Y");
        isLeft = isRight = isUp = isDown = false;

        if (lastX != x || lastY != y)
        {
            isLeft = x == -1 ? true : false;
            isRight = x == 1 ? true : false;
            isDown = y == -1 ? true : false;
            isUp = y == 1 ? true : false;
        }

        lastX = x;
        lastY = y;
    }
}
