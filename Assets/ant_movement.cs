using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ant_movement : MonoBehaviour
{
    public GameObject trailPheromone;
    public GameObject foodPheromone;
    public Transform centre;
    public float pheromoneFrequency = 0.5f;
    GameObject food;
    GameObject myPheromone;
    float maxStrengthPheromone = 0.0f;
    Vector2 foodPos;
    Vector2 pheromonePos;
    Vector2 wallAngle;
    bool foundFood = false;
    bool gotFood = false;
    bool hitWall = false;
    bool foundPheromone = false;
    Vector2 position;

    enum pheromone{TRAIL, FOOD};
    // the pheromone to be followed
    pheromone curPheromone = pheromone.FOOD;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0.0f, 0.0f, Random.Range(0, 360), Space.Self);
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        wander();
        handlePheromoneTrail();
    }

    public float maxSpeed = 1;
    public float turnStrength = 1;
    public float wanderStrength = 1;
    public int turnFrames = 50;
    public int foodFrames = 500;
    int frames = 0;
    Vector2 velocity;
    // nextTurn is the direction that the ant turns this frame - determined in wander()
    Vector2 nextTurn;
    // controls ant movement
    void wander()
    {
        // first priority - turn away from wall 
        if(hitWall)
        {
            nextTurn = wallAngle;
            frames++;
            // foundFood = false;
            if(frames >= turnFrames)
            {
                frames = 0;
                hitWall = false;
            }
        }
        // if food is nearby, go to it
        else if(foundFood && !gotFood)
        {
            // pos is the current position of the ant
            Vector2 pos;
            pos.x = centre.position.x;
            pos.y = centre.position.y;
            Vector2 direction = (foodPos - pos).normalized;
            // pick up the food when close enough
            if(Vector2.Distance(centre.position, foodPos) <= 0.2f)
            {
                pickupFood(food);
                foundFood = false;
                // turn around when food is found
                nextTurn = -nextTurn;
            }
            else 
            {
                nextTurn = direction;
            }

            // if the food is picked up by another ant, keep searching
            frames++;
            if (frames >= foodFrames)
            {
                frames = 0;
                foundFood = false;
            }
        }
        // if a desired pheromone is found, follow it
        else if(foundPheromone)
        {
            // pos is the current position of the ant
            Vector2 pos;
            pos.x = centre.position.x;
            pos.y = centre.position.y;
            Vector2 direction = (pheromonePos - pos).normalized;
            nextTurn = direction;
            if(Vector2.Distance(centre.position, pheromonePos) <= 0.2f)
            {
                foundPheromone = false;
            }
        }
        // otherwise wander randomly
        else
        {
            nextTurn = (nextTurn + Random.insideUnitCircle * wanderStrength);
        }

        // nextTurnStrength and acceleration are lower if the next turn is sharper
        Vector2 nextTurnStrength = nextTurn * turnStrength;
        Vector2 acceleration = Vector2.ClampMagnitude(nextTurn, turnStrength);

        // stay under max speed
        velocity = Vector2.ClampMagnitude(velocity + acceleration * Time.deltaTime, maxSpeed);
        position += velocity * Time.deltaTime;

        // calculate angle between this frame and last frame
        float theta = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, theta));
    }

    void pickupFood(GameObject food)
    {
        // don't pick up the food if another ant already got it
        if(food.transform.parent != null)
        {
            foundFood = false;
        }
        else
        {
            food.transform.parent = transform;
            gotFood = true;
            foundFood = true;
            curPheromone = pheromone.TRAIL;
        }
    }

    float pheromoneTime = 0.0f;
    // handle pheromones to leave behind
    void handlePheromoneTrail() {
        // each frame increment pheremoneTime
        pheromoneTime += Time.deltaTime;
        // once pheromoneTime >= pheromoneFrequency, leave one pheromone
        if(pheromoneTime >= pheromoneFrequency)
        {
            // leave a trail pheromone if the ant has no food, otherwise a food pheromone
            if(gotFood)
            {
                Instantiate(foodPheromone, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(trailPheromone, transform.position, Quaternion.identity);
            }
            pheromoneTime = 0.0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // get the food if food hasn't already been found
        if (collision.tag == "food" && !foundFood)
        {
            Vector2 curFoodPos;
            curFoodPos.x = collision.transform.position.x;
            curFoodPos.y = collision.transform.position.y;
            foodPos = curFoodPos;
            food = collision.gameObject;
            foundFood = true;
        }
        // follow pheromones to destination
        else if (collision.tag == "food_pheromone" && curPheromone == pheromone.FOOD ||
        collision.tag == "trail_pheromone" && curPheromone == pheromone.TRAIL)
        {
            if (foundPheromone)
            {
                float duration = myPheromone.GetComponent<pheromone_spawner>().getDuration();
                if (curPheromone == pheromone.TRAIL)
                {
                    if(duration < maxStrengthPheromone)
                    {
                        maxStrengthPheromone = duration;
                        myPheromone = collision.gameObject;
                    }
                }
                else
                {
                    if (duration > maxStrengthPheromone)
                    {
                        maxStrengthPheromone = duration;
                        myPheromone = collision.gameObject;
                    }
                }
            }
            foundPheromone = true;
            myPheromone = collision.gameObject;
            pheromonePos = myPheromone.transform.position;
        }
        if (collision.tag == "hill" && gotFood)
        {
            gotFood = !gotFood;
            curPheromone = pheromone.FOOD;
        }
    }

    // make sure ants don't hit the wall 
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "wall")
        {
            hitWall = true;
            wallAngle = collision.transform.right;
        }
    }

    // move to next pheromone after current one leaves hitbox
    void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject == myPheromone)
        {
            foundPheromone = false;
            myPheromone = null;
        }
    }

    // helper functions
    // distance - returns distance between two vector2's
    float Distance(Vector2 a, Vector2 b)
    {
        float x = b.x - a.x;
        float y = b.y - a.y;
        return Mathf.Sqrt(x*x + y*y);
    }
}
