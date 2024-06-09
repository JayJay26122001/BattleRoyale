using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Transform model;
    public WheelCollider wCollider;

    public bool steerable;
    public bool motorized;

    Vector3 position;
    Quaternion rotation;
    void Start()
    {
        wCollider = GetComponent<WheelCollider>();
    }

    void Update()
    {
        wCollider.GetWorldPose(out position, out rotation);
        model.transform.position = position;
        model.transform.rotation = rotation;
    }
}
