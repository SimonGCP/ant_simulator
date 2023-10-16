using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ant_movement : MonoBehaviour
{
    public Transform transform;
    public CircleCollider2D leftCollider;
    public CircleCollider2D rightCollider;
    public float maxSpeed = 3.0f;
    public float wallDetectSpeed = 270.0f;
    float changeAngular;
    float changeAcceleration;
    float acceleration;
    float angularAcceleration;

    float count = 0.0f;
    float angleCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        changeAcceleration = Random.Range(0.1f, 1.5f);
        acceleration = Random.Range(-1.0f, 1.0f);
        changeAngular = Random.Range(0.1f, 1.5f);
        angularAcceleration = Random.Range(3.0f, 40.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(count >= changeAcceleration) {
            changeAcceleration = Random.Range(0.1f, 1.5f);
            acceleration = Random.Range(-1.0f, 1.0f);
            count = 0.0f;
        } else {
            count += 0.1f;
        }

        if(angleCount >= changeAngular) {
            changeAngular = Random.Range(0.1f, 7.5f);
            angularAcceleration = Random.Range(-40.0f, 40.0f);
            angleCount = 0.0f;
        } else {
            angleCount += 0.1f;
        }
        transform.position += transform.right * maxSpeed * Mathf.Max(maxSpeed / 5, (acceleration / 50));
        transform.Rotate(0.0f, 0.0f, angularAcceleration / 50, Space.Self);
    }

    void OnTriggerStay2D (Collider2D collision)
    {
        angularAcceleration = wallDetectSpeed;
        changeAngular = 2.0f;
    }
}
