using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
    public Race race;
    public void InicialPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
