using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : Racer
{
    PlayerInput playerAction;
    Rigidbody rb;
    [SerializeField] float Speed, airborneSpeedMultiplier, maxJumpDuration, JumpForce, fallMultiplier, gravityMultiplier, jumpCooldown, coyoteTime;
    [SerializeField] Transform CameraAxis, cameraPivot;
    bool onGround, airborne, jump, jumpPressed, jumpOnCooldown, onCoyoteTime, knockback;
    float gravity = -9.81f, rotationAux;
    Vector3 speedVector, lastDirection;
    Vector2 directionInput;
    //[SerializeField] Camera playerCamera;
    public Vector3 SpeedVector
    {
        get { return speedVector; }
    }
    public Vector2 DirectionInput
    {
        get { return directionInput; }
        set { directionInput = value; }
    }

    //Linha do tempo
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAction = GetComponent<PlayerInput>();
        knockback = false;
    }
    void Update()
    {
        MovementCalculation();
    }
    void FixedUpdate()
    {
        PhysicMovement();
    }

    void SmoothRotation()
    {
        float angle = Vector3.SignedAngle(Vector3.forward, lastDirection, Vector3.up);
        if (Math.Abs(MathF.Abs(transform.eulerAngles.y) - angle) > 0.1f)
        {
            float SmoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref rotationAux, Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0, SmoothAngle, 0);
        }
    }

    public void Jump()
    {
        AudioManager.instance.PlaySoundEffects("Jump");
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * MathF.Sqrt(JumpForce * 2.1f * -gravity), ForceMode.Impulse);
    }

    IEnumerator JumpAction(float time)//time é o tempo maximo que o impulso do pulo dura
    {
        jump = true;
        yield return new WaitForSeconds(time);
        jump = false;
    }

    IEnumerator JumpCooldown(float time)//time é o tempo minimo entre um pulo e outro
    {
        jumpOnCooldown = true;
        yield return new WaitForSeconds(time);
        jumpOnCooldown = false;
    }
    IEnumerator CoyoteTime()
    {
        onCoyoteTime = true;
        yield return new WaitForSeconds(coyoteTime);
        onCoyoteTime = false;
    }
    void MovementCalculation() //calcula vetores e variaveis de movimentação
    {
        onGround = IsGrounded();
        if (airborne == true && onGround == true)
        {
            StartCoroutine(JumpCooldown(jumpCooldown));
        }
        airborne = !onGround;
        if (maxJumpDuration <= 0)
        {
            maxJumpDuration = Time.fixedDeltaTime;
        }
        float speedMultiplier = 1;
        if (!onGround)
        {
            speedMultiplier *= airborneSpeedMultiplier;
        }
        speedVector = directionInput.x * Speed * speedMultiplier * CameraAxis.right + directionInput.y * Speed * speedMultiplier * CameraAxis.forward;
        speedVector.y = rb.velocity.y;
        if (directionInput != Vector2.zero)
        {
            lastDirection = speedVector.normalized;
            lastDirection.y = 0;
        }
        //transform.LookAt(lastDirection + transform.position);
        if (!jumpPressed)
        {
            jump = false;
        }
        if (jumpPressed && !jump && onGround && !jumpOnCooldown && (onGround || onCoyoteTime))
        {
            onCoyoteTime = false;
            StartCoroutine(JumpAction(maxJumpDuration));
        }
    }
    void PhysicMovement() // bota vetores e variaveis de movimentação em pratica com fisica
    {
        if (!rb.useGravity /*&& !onGround*/)
        {
            rb.AddForce(Vector3.up * gravity * gravityMultiplier, ForceMode.Acceleration);
            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.up * gravity * gravityMultiplier * (fallMultiplier - 1), ForceMode.Acceleration);
            }
        }
        rb.velocity = speedVector;
        SmoothRotation();
        if (jump)
        {
            Jump();
        }
    }

    public void CheatInput(InputAction.CallbackContext context)
    {
        GameManager.manager.uiController.ChangeScene("CarLevel");
    }
    public void MoveInput(InputAction.CallbackContext context)
    {
        if (race.RaceActive)
        {
            if(!knockback)
            {
                directionInput = context.ReadValue<Vector2>();
            }
        }
        else
        {
            directionInput = Vector2.zero;
        }
    }

    public void KnockbackOn()
    {
        knockback = true;
        Invoke("KnockbackOff", 0.1f);
    }
    public void KnockbackOff()
    {
        knockback = false;
    }
    public void jumpInput(InputAction.CallbackContext context)
    {
        if (!race.RaceActive)
        {
            return;
        }
        else
        {
            if (context.phase == InputActionPhase.Started)
            {
                jumpPressed = true;
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                jumpPressed = false;
            }
        }
    }
    public bool IsGrounded()
    {
        RaycastHit hit = new RaycastHit();
        if (!Physics.CapsuleCast(transform.position + Vector3.down * 0.5f, transform.position + Vector3.up * 0.5f, 0.3f, Vector3.down, out hit, 0.3f) || hit.collider.isTrigger)
        {
            if (!jump && onGround)
            {
                StartCoroutine(CoyoteTime());
            }
            return false;
        }
        return true;
    }
    public GameObject OnTopOf()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.CapsuleCast(transform.position + Vector3.down * 0.5f, transform.position + Vector3.up * 0.5f, 0.5f, Vector3.down, out hit, 0.3f))
        {
            if (!hit.collider.isTrigger)
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.SetParent(collision.gameObject.transform);
            collision.gameObject.GetComponent<MovingPlatform>().StartMoving();
        }
        if(collision.gameObject.tag == "DeathPlane")
        {
            GameManager.manager.uiController.ChangeScene("Defeat");
        }
        if (collision.gameObject.tag == "Win")
        {
            if(race.GetPlayerPos(this) < 3)
            {
                GameManager.manager.uiController.ChangeScene("CarLevel");
            }
            else
            {
                GameManager.manager.uiController.ChangeScene("Defeat");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "MovingPlatform" && transform.parent != null)
        {
            transform.parent.GetComponent<MovingPlatform>().StopMoving();
            transform.SetParent(null);
        }
    }
}
