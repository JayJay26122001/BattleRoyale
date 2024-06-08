using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public float force;
    public float lifetime = 1f; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().KnockbackOn();
            collision.gameObject.GetComponent<Rigidbody>().AddForce((transform.forward + transform.up) * force, ForceMode.Impulse);
            collision.gameObject.GetComponent<Player>().DirectionInput = collision.gameObject.GetComponent<Rigidbody>().velocity;
        }
        this.gameObject.SetActive(false);
        Destroy(gameObject, 0.01f);
    }
}
