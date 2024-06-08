using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    public Player player;
    public float speed;
    public float sight;
    public float force;
    public LayerMask playerMask;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 dir = player.transform.position - transform.position;
        Debug.DrawRay(transform.position, dir, Color.red, 3);
        if (Physics.Raycast(transform.position, dir, sight, playerMask))
        {
            MoveTo(player.transform.position, 0, speed);
        }
    }

    public void MoveTo(Vector3 Destination, float StopDistance, float Speed)
    {
        agent.stoppingDistance = StopDistance;
        agent.speed = Speed;
        agent.acceleration = Speed * 10;
        agent.destination = Destination;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player.gameObject)
        {
            collision.gameObject.GetComponent<Player>().KnockbackOn();
            collision.gameObject.GetComponent<Rigidbody>().AddForce((transform.forward + transform.up) * force, ForceMode.Impulse);
            collision.gameObject.GetComponent<Player>().DirectionInput = collision.gameObject.GetComponent<Rigidbody>().velocity;
        }
    }
}
