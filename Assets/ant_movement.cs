using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ant_movement : MonoBehaviour
{
    public Transform transform;
    public float maxSpeed = 0.4f;
    public float wallDetectSpeed = 270.0f;
    public float angularAcceleration;
    public float test_accel = 40.0f;
    float changeAngular;
    float changeAcceleration;
    float acceleration;

    float count = 0.0f;
    float angleCount = 0.0f;
    float angle = 0.0f;
    Vector2 direction;
    Vector2 foodPos;

    bool foundFood = false;

    // Start is called before the first frame update
    void Start()
    {
        changeAcceleration = Random.Range(0.1f, 1.5f);
        acceleration = Random.Range(-1.0f, 1.0f);
        changeAngular = Random.Range(0.1f, 1.5f);
        angularAcceleration = Random.Range(3.0f, 40.0f);
        transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f), Space.Self);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!foundFood)
        {
            wander();
        }
        else 
        {
            pickupFood(foodPos);
        }
    }

    // wander around randomly until food is found
    void wander() 
    {
        if (count >= changeAcceleration)
        {
            changeAcceleration = Random.Range(0.1f, 1.5f);
            acceleration = Random.Range(-1.0f, 1.0f);
            count = 0.0f;
        }
        else
        {
            count += 0.1f;
        }

        if (angleCount >= changeAngular)
        {
            changeAngular = Random.Range(0.1f, 7.5f);
            angularAcceleration = Random.Range(-40.0f, 40.0f);
            angleCount = 0.0f;
        }
        else
        {
            angleCount += 0.1f;
        }
        transform.position += transform.right * Mathf.Max(maxSpeed * acceleration / 50, maxSpeed / 10);
        transform.Rotate(0.0f, 0.0f, angularAcceleration / 50, Space.Self);
    }

    void pickupFood(Vector2 foodPos)
    {
        Debug.Log("Found food");
    }

    // turns the ant away from a wall before hitting it
    void turnFromWall(Collider2D collision) 
    {
        Vector2 closestPoint = collision.ClosestPoint(transform.position);
        direction = closestPoint - (Vector2)transform.position;
        angle = Vector2.SignedAngle(transform.up, direction);

        acceleration = -50.0f;
        changeAcceleration = 2.0f;
        count = 0.0f;
        angularAcceleration = 270 - (angle % 90) * 3;

        changeAngular = 200.0f / Mathf.Abs(angularAcceleration);
        angleCount = 0.0f;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "wall")
        {
            turnFromWall(collision);
        }
        if (collision.tag == "food") 
        {
            foundFood = true;
            foodPos = collision.transform.position;
        }
    }
}
