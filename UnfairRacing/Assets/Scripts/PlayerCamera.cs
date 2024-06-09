using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    Vector3 relativePosition;
    void Awake()
    {
        relativePosition = transform.position - player.transform.position;
    }
    void Update()
    {
        transform.position = player.transform.position + relativePosition;
    }
}
