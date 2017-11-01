using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Rigidbody2D rgdbody;

    private bool isGrounded;
    private bool jump;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    public override void Start()
    {
        base.Start();
        rgdbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        isGrounded = IsGrounded();

        HandleMovement(horizontal);
        Flip(horizontal);
       
        HandleLayers();

        ResetValues();
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }        
    }

    private bool IsGrounded()
    {
        if (rgdbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        Animator.ResetTrigger("jump");
                        Animator.SetBool("land", false);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void HandleMovement(float horizontal)
    {
        if (rgdbody.velocity.y < 0)
        {
            Animator.SetBool("land", true);
        }

        if (isGrounded && jump)
        {
            isGrounded = false;
            rgdbody.AddForce(new Vector2(0, jumpForce));
            Animator.SetTrigger("jump");
        }

        rgdbody.velocity = new Vector2(horizontal * movementSpeed, rgdbody.velocity.y);
        Animator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleLayers()
    {
        if (!isGrounded)
        {
            Animator.SetLayerWeight(1, 1);
            Animator.SetLayerWeight(0, 0);
        }
        else
        {
            Animator.SetLayerWeight(0, 1);
            Animator.SetLayerWeight(1, 0);
        }
    }

    private void ResetValues()
    {        
        jump = false;        
    }
}
