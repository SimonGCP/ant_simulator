using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ant_movement : MonoBehaviour
{
    public GameObject trailPheromone;
    public GameObject foodPheromone;
    GameObject curPheromone;
    public Transform transform;
    public float leavePheromone = 0.2f;
    public float maxSpeed = 0.4f;
    public float wallDetectSpeed = 270.0f;
    public float angularAcceleration;
    public float test_accel = 40.0f;
    public int turnCountMax = 5;
    float changeAngular;
    float changeAcceleration;
    float acceleration;
    float pheromoneCount = 0.0f;
    int collisionCount = 0;
    int turncount = 0;

    float count = 0.0f;
    float angleCount = 0.0f;
    float angle = 0.0f;
    Vector2 direction;
    Vector2 foodPos;

    List<Collider2D> pheromones;

    bool foundFood = false;

    // Start is called before the first frame update
    void Start()
    {
        changeAcceleration = Random.Range(0.1f, 1.5f);
        acceleration = Random.Range(-1.0f, 1.0f);
        changeAngular = Random.Range(0.1f, 1.5f);
        angularAcceleration = Random.Range(3.0f, 40.0f);
        transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f), Space.Self);
        curPheromone = trailPheromone;
        // pheromones = new(List<Collider2D>);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        wander();
    }

    // wander around randomly until food is found
    void wander()
    {
        turncount++;
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

        if (pheromoneCount >= leavePheromone)
        {
            if (foundFood)
            {
                curPheromone = foodPheromone;
                leaveTrailPheromone();
                followTrailPheromones();
            }
            else
            {
                curPheromone = trailPheromone;
                leaveTrailPheromone();
                followFoodPheromones();
            }
            pheromoneCount = 0.0f;
        }
        else
        {
            pheromoneCount += 0.1f;
        }
        transform.position += transform.right * Mathf.Max(maxSpeed * acceleration / 50, maxSpeed / 10);
        transform.Rotate(0.0f, 0.0f, angularAcceleration / 50, Space.Self);
    }

    void followTrailPheromones()
    {
        if (pheromones == null)
        {
            Debug.Log("pheromones == null");
            return;
        }
        else
        {
            Debug.Log("pheremones != null");
        }
        float maxStrength = float.MaxValue;
        GameObject strongest = null;
        foreach (Collider2D pheromone in pheromones)
        {
            float d = pheromone.gameObject.GetComponent<float>();
            if (d < maxStrength)
            {
                strongest = pheromone.gameObject;
            }
        }
        if (strongest != null)
        {

        }
    }

    void followFoodPheromones()
    {
        if (pheromones == null)
        {
            // Debug.Log("pheromones == null");
            return;
        }
        else
        {
            // Debug.Log("pheremones != null");
        }
        float maxStrength = float.MaxValue;
        GameObject strongest = null;
        foreach (Collider2D pheromone in pheromones)
        {
            float d = pheromone.gameObject.GetComponent<float>();
            if (d > maxStrength)
            {
                strongest = pheromone.gameObject;
            }
        }
        if (strongest != null)
        {

        }
    }

    void pickupFood(Vector2 foodPos)
    {
        Debug.Log("Found food");
        foundFood = !foundFood;
    }

    // leave a trail of pheromones (without food)
    void leaveTrailPheromone()
    {
        Instantiate(curPheromone, transform.position, Quaternion.identity);
    }

    // turns the ant away from a wall before hitting it
    void turnFromWall(Collider2D collision)
    {
        if (collisionCount > 15)
        {
            transform.Rotate(0, 0, 180f);
            collisionCount = -20;
        }
        Vector2 closestPoint = collision.ClosestPoint(transform.position);
        direction = closestPoint - (Vector2)transform.position;
        angle = Vector2.SignedAngle(transform.up, direction);

        acceleration = -50.0f;
        changeAcceleration = 2.0f;
        count = 0.0f;

        if (Mathf.Abs(angle) <= 90f)
        {
            angularAcceleration = -270f + (angle % 90) * 3;
        }
        // else if(Mathf.Abs(angle) > 80 && Mathf.Abs(angle) <= 100){

        //     angularAcceleration = 360f * 3;
        // }
        else if (Mathf.Abs(angle) > 90f)
        {
            angularAcceleration = 270f - (angle % 90) * 3;
        }

        changeAngular = 200.0f / Mathf.Abs(angularAcceleration);
        angleCount = 0.0f;
        turncount = 0;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "wall")
        {
            collisionCount++;
            turnFromWall(collision);
        }
        if (collision.tag == "food")
        {
            foundFood = true;
            foodPos = collision.transform.position;
        }
        if (collision.tag == "hill")
        {
            foundFood = false;
        }
        if (collision.tag == "food_pheromone" && !foundFood)
        {
            if (collision.gameObject != null)
                pheromones.Add(collision);
        }
        if (collision.tag == "trail_pheromone" && foundFood)
        {
            if (collision.gameObject != null)
                pheromones.Add(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        collisionCount = 0;
        turncount = -20;
    }
}
