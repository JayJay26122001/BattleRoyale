using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRacer : Racer
{
    public float speed;
    void Update()
    {
        if (race.RaceActive)
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
    }
}
