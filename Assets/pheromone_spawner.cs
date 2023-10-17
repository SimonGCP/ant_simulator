using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pheromone_spawner : MonoBehaviour
{
    public float lifetime = 100.0f;
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        duration = lifetime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        duration = duration - 1.0f;
        var color = gameObject.GetComponent<Renderer>().material.color;
        color.a = duration / lifetime;
        gameObject.GetComponent<Renderer>().material.color = color;

        if (duration <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
