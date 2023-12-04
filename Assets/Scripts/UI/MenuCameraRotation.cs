using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraRotation : MonoBehaviour
{
    public float speedX;
    public float speedY;
    public float speedZ;

    void FixedUpdate()
    {
        transform.Rotate(speedX, speedY, speedZ, Space.Self);
    }
}
