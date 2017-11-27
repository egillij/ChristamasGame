using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private Transform throwAirPosition;

    [SerializeField]
    private float stoppingSpeed;

    public bool OnGround { get; set; }

    public bool Slide { get; set; }

    public bool Jump { get; set; }

    public bool Run { get; set; }

    public Rigidbody2D Rbody { get; set; }

    public Rigidbody2D MovingPlatform { get; set; }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public override void Start()
    {
        base.Start();
        Rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();   
    }

    void FixedUpdate()
    {
        GameManager.instance.LevelDuration = Time.time;

        if (IsDead)
        {
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");

        OnGround = IsGrounded();
        HandleMovement(horizontal);
        
        Flip(horizontal);
       
        HandleLayers();

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
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    Animator.SetTrigger("attack");
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Animator.SetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Run = true;
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            Run = false;
        }

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Animator.SetTrigger("slide");
        //}

        if (Input.GetKeyDown(KeyCode.K))
        {
            Animator.SetTrigger("throw");
        }
    }

    private bool IsGrounded()
    {
        if (Rbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void HandleMovement(float horizontal)
    {
        float finalSpeed = movementSpeed;

        if (Rbody.velocity.y < 0)
        {
            Animator.SetBool("land", true);
        }

        if (Run)
        {
            finalSpeed *= 1.5f;
        }

        if (!Attack && !Slide && (OnGround || airControl))
        {
            if (MovingPlatform == null)
            {
                Rbody.velocity = new Vector2(finalSpeed * horizontal, Rbody.velocity.y);
                if (horizontal == 0.0f)
                {
                    Rbody.velocity = new Vector2(0.0f, Rbody.velocity.y);
                }

                if (Mathf.Abs(Rbody.velocity.x) > finalSpeed)
                {
                    Rbody.velocity = new Vector2(Mathf.Sign(Rbody.velocity.x) * finalSpeed, Rbody.velocity.y);
                }
                else
                {
                    Rbody.AddForce(new Vector2(finalSpeed * horizontal, 0.0f));
                }
            }

            else
            {
                Rbody.velocity = new Vector2(finalSpeed * horizontal, Rbody.velocity.y) + MovingPlatform.velocity;
            }

        }

        if (Jump && OnGround)//Rbody.velocity.y == 0.0f)
        {
            Rbody.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            Jump = !Jump;
        }
        
        if (Run)
        {
            Animator.SetFloat("speed", 2*Mathf.Abs(horizontal));
        }
        else
        {
            Animator.SetFloat("speed", Mathf.Abs(horizontal));
        }
    }

    private void HandleLayers()
    {
        if (!OnGround)
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

    public override void ThrowAttack(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            Vector3 throwPos = throwPosition.position;
            if (!OnGround)
            {
                throwPos = throwAirPosition.position;
            }
            if (facingRight)
            {
                GameObject throwable = (GameObject)Instantiate(throwablePrefab, throwPos, Quaternion.Euler(new Vector3(0, 0, -90)));
                throwable.GetComponent<Kunai>().Initialize(Vector2.right);
            }
            else
            {
                GameObject throwable = (GameObject)Instantiate(throwablePrefab, throwPos, Quaternion.Euler(new Vector3(0, 0, 90)));
                throwable.GetComponent<Kunai>().Initialize(Vector2.left);
            }
        }
    }

    public override IEnumerator TakeDamage()
    {
        health--;

        if (!IsDead)
        {
            Animator.SetTrigger("damage");
        }
        else
        {
            Animator.SetBool("dead", true);
            yield return null;
        }
    }
}
