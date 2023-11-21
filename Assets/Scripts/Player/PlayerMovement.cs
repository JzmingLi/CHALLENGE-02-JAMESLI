using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float airSpeedMultiplier;
    [SerializeField] InputActionAsset input;

    InputActionMap playerActions;
    bool readyToJump;
    bool grounded;
    bool touchingSlope;
    bool jumping;

    float walkSoundCooldown;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
        playerActions = input.FindActionMap("Player");
        input.Enable();
        readyToJump = true; 
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, transform.localScale.y/2 + 0.6f);
        Debug.DrawRay(transform.position, Vector3.down * (transform.localScale.y / 2 + 0.6f));
        jumping = playerActions.FindAction("Jump").IsPressed();
        SpeedControl();

        if (grounded) rb.drag = 5;
        else rb.drag = 0.1f;

        walkSoundCooldown -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        MovePlayer();
        Jump();
    }

    public void MovePlayer()
    {
        Vector2 inputDirection = playerActions.FindAction("Move").ReadValue<Vector2>();
        Vector3 direction = inputDirection.x * transform.right + inputDirection.y * transform.forward;
        if (grounded || touchingSlope)
        {
            if (walkSoundCooldown <= 0 && inputDirection != Vector2.zero)
            {
                walkSoundCooldown = 0.25f;
            }
            rb.AddForce(direction.normalized * moveSpeed);
        }
        else rb.AddForce(direction.normalized * moveSpeed * airSpeedMultiplier);
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    void Jump()
    {
        if ((grounded || touchingSlope) && jumping && readyToJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            readyToJump = false;
            Invoke(nameof(ResetJump), 0.5f);
        }
    }

    void ResetJump()
    {
        readyToJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Slope")) touchingSlope = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Slope")) touchingSlope = false;
    }

}
