using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCamera : MonoBehaviour
{

    public float rotateSpeedCamera;
    private float horizontalInput;

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // rotar la camara con los cursores
        transform.Rotate(Vector3.up, horizontalInput * rotateSpeedCamera * Time.deltaTime);
    }
}
