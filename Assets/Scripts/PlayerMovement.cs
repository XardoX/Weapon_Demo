using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed,
            jumpPower;

    [SerializeField]
    private LayerMask groundLayerMask;

    private Vector2 moveInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private Collider coll;

    bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }
    private void Update()
    {
        CheckIsGrounded();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime); 
    }

    private void CheckIsGrounded()
    {
        var rayDistance = (coll.bounds.size.y/2f) + 0.2f;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundLayerMask);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if(value.isPressed && isGrounded)
        {
            Jump();
        }
    }
}
