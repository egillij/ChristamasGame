using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reindeer : Character
{
    Rigidbody2D rgdbody;

    public override bool IsDead
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override void Start()
    {
        base.Start();
        facingRight = !facingRight;
        rgdbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        HandleMovement(horizontal, vertical);
        Flip(horizontal);
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }

    private void HandleMovement(float horizontal, float vertical)
    {
        /*
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
        */
        rgdbody.velocity = new Vector2(horizontal * movementSpeed, vertical * movementSpeed);
        Animator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    public override IEnumerator TakeDamage()
    {
        throw new NotImplementedException();
    }
}
