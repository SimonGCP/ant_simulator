using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate() {
        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");
        if(Camera.current != null)
        {
            Camera.current.transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue));
        }
    }
}
