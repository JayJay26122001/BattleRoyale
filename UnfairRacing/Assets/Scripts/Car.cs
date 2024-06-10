using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : Racer
{
    public float motorTorque = 2000;
    public float brakeTorque = 500;
    public float maxSpeed = 10;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;
    public float jumpForce = 8;
    public float acceleration = 1;
    NavMeshAgent agent;

    public bool playerCar;
    public bool canJump;
    public bool jumpCooldown;
    public bool canTurbo;
    public bool turboCooldown;
    public bool canMove;

    public LayerMask endLayer;

    public Wheel[] wheels;
    Rigidbody rb;
    void Start()
    {
        if(!playerCar)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.up * centreOfGravityOffset;
        canMove = false;
    }

    void Update()
    {
        if(canMove)
        {
            if (playerCar)
            {
                float vInput = Input.GetAxis("Vertical");
                float hInput = Input.GetAxis("Horizontal");
                float forwardSpeed = Vector3.Dot(transform.forward, rb.velocity);
                float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);
                float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
                float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);
                bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

                foreach (Wheel wheel in wheels)
                {
                    if (wheel.steerable)
                    {
                        wheel.wCollider.steerAngle = hInput * currentSteerRange;
                    }

                    if (isAccelerating)
                    {
                        if (wheel.motorized)
                        {
                            wheel.wCollider.motorTorque = vInput * currentMotorTorque * acceleration;
                        }
                        wheel.wCollider.brakeTorque = 0;
                    }
                    else
                    {
                        wheel.wCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                        wheel.wCollider.motorTorque = 0;
                    }
                }

                if (canJump && !jumpCooldown && Input.GetKeyDown(KeyCode.Space) && (wheels[0].wCollider.isGrounded || wheels[1].wCollider.isGrounded))
                {
                    jumpCooldown = true;
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                    Invoke("ClearJumpCooldown", 5);
                }

                if (canTurbo && !turboCooldown && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    turboCooldown = true;
                    acceleration *= 2;
                    maxSpeed *= 2;
                    Invoke("StopTurbo", 1);
                    Invoke("ClearTurboCooldown", 10);
                }

                if(Physics.Raycast(transform.position, Vector3.down, 2, endLayer))
                {
                    if (race.GetPlayerPos(this) == 0)
                    {
                        GameManager.manager.uiController.ChangeScene("Victory");
                    }
                    else
                    {
                        GameManager.manager.uiController.ChangeScene("Defeat");
                    }
                }
            }
            else
            {
                /*
                float forwardSpeed = Vector3.Dot(transform.forward, rb.velocity);
                float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);
                float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
                foreach (Wheel wheel in wheels)
                {
                    if (wheel.motorized)
                    {
                        wheel.wCollider.motorTorque = currentMotorTorque * acceleration;
                    }
                }*/
                MoveTo(new Vector3(135, 1.5f, 310), 0, maxSpeed);
            }
        }
    }

    public void MoveTo(Vector3 Destination, float StopDistance, float Speed)
    {
        agent.stoppingDistance = StopDistance;
        agent.speed = Speed;
        agent.acceleration = Speed * 10;
        agent.destination = Destination;
    }

    void ClearTurboCooldown()
    {
        turboCooldown = false;
    }
    void ClearJumpCooldown()
    {
        jumpCooldown = false;
    }

    void StopTurbo()
    {
        acceleration /= 2;
        maxSpeed /= 2;
    }

    public void AccelUp()
    {
        acceleration *= 2;
    }

    public void SpeedUp()
    {
        maxSpeed *= 2;
    }

    public void UnlockTurbo()
    {
        canTurbo = true;
    }

    public void UnlockJump()
    {
        canJump = true;
    }

    public void BrakeUp()
    {
        brakeTorque = 3000;
    }

    public void SteerUp()
    {
        steeringRange = 50;
        steeringRangeAtMaxSpeed = 30;
    }

    public void StartRace()
    {
        canMove = true;
    }
}
