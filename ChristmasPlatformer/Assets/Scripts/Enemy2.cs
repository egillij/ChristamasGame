using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Character {

    private static Enemy2 instance;

    public static Enemy2 Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Enemy2>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private bool airControl;

    public bool OnGround { get; set; }

    public bool Slide { get; set; }

    public bool Jump { get; set; }

    //public Rigidbody2D Rbody { get; set; }

    public override bool IsDead
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override void Start () {
        base.Start();
        //Rbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
    }

    // Update is called once per frame
    void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");

        OnGround = IsGrounded();
        HandleMovement(horizontal);
        
        Flip(horizontal);
        HandleLayers();
    }

    private void HandleMovement(float horizontal)
    {   
        if (Rbody.velocity.y < 0)
        {
            Animator.SetBool("land", true);
        }

        if (!Attack && !Slide &&  (OnGround || airControl))
        {
            Rbody.velocity = new Vector2(movementSpeed * horizontal, Rbody.velocity.y);
        }

        if (Jump && Rbody.velocity.y == 0.0f)
        {

            Rbody.AddForce(new Vector2(0.0f, jumpForce));
        }


        Animator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Animator.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Animator.SetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Animator.SetTrigger("slide");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Animator.SetTrigger("throw");
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1.0f;
            transform.localScale = theScale;
        }
    }

    public override void ThrowAttack(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            if (facingRight)
            {
                GameObject throwable = (GameObject)Instantiate(throwablePrefab, throwPosition.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                throwable.GetComponent<Kunai>().Initialize(Vector2.right);
            }
            else
            {
                GameObject throwable = (GameObject)Instantiate(throwablePrefab, throwPosition.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                throwable.GetComponent<Kunai>().Initialize(Vector2.left);
            }
        }

        
    }

    public override IEnumerator TakeDamage()
    {
        throw new NotImplementedException();
    }
}
