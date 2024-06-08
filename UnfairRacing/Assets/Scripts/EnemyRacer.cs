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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Win")
        {
            GameManager.manager.uiController.ChangeScene("Defeat");
        }
    }
}
