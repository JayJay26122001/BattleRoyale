using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    float t;
    public bool horizontal;
    public bool forward;
    bool moving;
    public float distance;
    Vector3 pos1, pos2;
    void Start()
    {
        moving = false;
        if(horizontal)
        {
            if (forward)
            {
                t = 0;
                pos1 = transform.position;
                pos2 = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
            }
            else
            {
                t = 1;
                pos2 = transform.position;
                pos1 = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
            }
        }
        else
        {
            if (forward)
            {
                t = 0;
                pos1 = transform.position;
                pos2 = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
            }
            else
            {
                t = 1;
                pos2 = transform.position;
                pos1 = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
            }
        }
    }

    void FixedUpdate()
    {
        if (moving)
        {
            if (forward)
            {
                t += 0.01f;
                if (t >= 0.99)
                {
                    forward = false;
                }
            }
            else
            {
                t -= 0.01f;
                if (t <= 0.01)
                {
                    forward = true;
                }
            }
            this.transform.position = Vector3.Lerp(pos1, pos2, t);
        }
    }

    public void StartMoving()
    {
        moving = true;
    }
    public void StopMoving()
    {
        moving = false;
    }
}
