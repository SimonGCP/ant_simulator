using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastTest : MonoBehaviour
{
    // Start is called before the first frame update

    //I will inlcude a simple movement system to allow the object to change position and rotation
    //simulating an entity navigating its environment
    public Transform transform;
    
    //forward
    public float acceleration = 1.0f;
    public float speed = 1f;

    //rotation
    public float deltaRot = 0;

    public float rayRange = 1f;
    float changeAcceleration;
    float changeAngular;
    int castWaitTime = 50;
    int castInc = 0;


    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position + transform.right * 1.11f, transform.TransformDirection(Vector2.right) * rayRange, Color.red);
        castInc++;

        speed += acceleration/50;
        transform.position += transform.right * (speed / 50);
        //speed is divided by 50 because FixedUpdate is called 50 times a second, and max speed unit is units per second.
        
        // transform.Rotate(0.0f, 0.0f, angularAcceleration / 50, Space.Self);
            
        if(castInc >= castWaitTime){
            CastRay();
            castInc = 0;
        }
    }
    
    bool CastRay()
    {
        Debug.DrawRay(transform.position + transform.right * 1.11f, transform.TransformDirection(Vector2.right) * rayRange, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * 1.11f, transform.TransformDirection(Vector2.right), rayRange, -1);
        if(hit){
            Debug.Log("hit something : " + hit.collider.name);
        }
        return hit;
    }
}
