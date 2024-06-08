using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonball;
    public GameObject cannonballSpawn;
    public float cooldown;

    private void Start()
    {
        Invoke("Shoot", cooldown);
    }

    void Shoot()
    {
        GameObject bulletObj = Instantiate(cannonball, cannonballSpawn.transform.position, Quaternion.LookRotation(transform.forward)) as GameObject;
        Physics.IgnoreCollision(bulletObj.GetComponent<Collider>(), this.gameObject.GetComponent<Collider>());
        Physics.IgnoreCollision(bulletObj.GetComponent<Collider>(), cannonballSpawn.GetComponent<Collider>());
        Invoke("Shoot", cooldown);
    }
}
