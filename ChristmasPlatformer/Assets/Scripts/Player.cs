using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public BoxCollider2D SlideCollider;
    public BoxCollider2D WalkCollider;


    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private Transform throwAirPosition;

    

    [SerializeField]
    private float stoppingSpeed;

    private float sleepStop;

    public bool OnGround { get; set; }

    public bool Slide { get; set; }

    public bool Jump { get; set; }

    public bool Run { get; set; }

    public bool AllowedDown { get; set; }

    public bool GoDown { get; set; }

    public GameObject MovingPlatform { get; set; }

    public bool Sleep { get; set; }


    public int BonusScore {get; set;}
    public int EnemiesKilled { get; set; }

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

        //Ignore collision between player and enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Physics2D.IgnoreCollision(enemy.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        }

        BonusScore = 0;
        EnemiesKilled = 0;
        health = GameManager.instance.health;
    }

    void Update()
    {
        if (Sleep)
        {
            if (Time.time >= sleepStop)
            {
                Sleep = !Sleep;
                Rbody.WakeUp();
            }
        }
        else
        {
            HandleInput();
        }

        
    }

    void FixedUpdate()
    {
        if (IsDead)
        {
            Animator.SetBool("dead", true);
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        if (Sleep)
        {
            horizontal = 0.0f;   
        }

        OnGround = IsGrounded();

        HandleMovement(horizontal);

        Flip(horizontal);

        HandleLayers();

    }

    private void Flip(float horizontal)
    {
        if (!Slide)
        {
            if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
            {
                ChangeDirection();
            }
        }
        
    }

    private void HandleInput()
    {
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

        if (Input.GetKeyDown(KeyCode.DownArrow) && AllowedDown)
        {
            GoDown = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Mathf.Abs(Rbody.velocity.x) > 0.0f && OnGround)
                Animator.SetTrigger("slide");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Animator.SetTrigger("throw");
        }
    }

    public void PlayerDown()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GoDown = true;
        }
    }

    private void HandleMovement(float horizontal)
    {
        float finalSpeed = movementSpeed;

        if (GoDown)
        {
            Rbody.velocity = new Vector2(0, -1 * movementSpeed);
            return;
        }

        if (Run)
        {
            finalSpeed *= 1.5f;
        }

        if (!Attack && (OnGround || airControl))
        {
            if (MovingPlatform == null)
            {
                if (!Slide || (Slide && Mathf.Sign(horizontal) == Mathf.Sign(Rbody.velocity.x)))
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
                
            }

            else
            {
                Rbody.velocity = new Vector2(finalSpeed * horizontal, Rbody.velocity.y) + MovingPlatform.GetComponent<Rigidbody2D>().velocity;
            }

        }

        if (Jump && OnGround)
        {
            if (MovingPlatform != null)
            {
                MovingPlatform.GetComponent<PlatformMover>().playerOnTop = false;
            }
            Rbody.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            Jump = false;
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

    public void Sleeping(float sleepDuration)
    {
        Sleep = true;
        Rbody.Sleep();
        sleepStop = Time.time + sleepDuration;
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

    public override void ChangeDirection()
    {
        base.ChangeDirection();
        throwPosition.transform.position = throwPosition.transform.position + (facingRight ? Vector3.right : Vector3.left) * 1.5f;
        
    }

    public void Death()
    {
        StartCoroutine(LevelDeath());
    }

    public override IEnumerator TakeDamage()
    {
        GameManager.instance.health--;
        health = GameManager.instance.health;

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

    private IEnumerator LevelDeath()
    {
        SceneManager.LoadScene("LevelScore", LoadSceneMode.Additive);
        
        while (!SceneManager.GetSceneAt(1).isLoaded)
        {
            yield return null;
        }
        LevelRecap.Instance.InitializeRecap(GameManager.instance.score, GameManager.instance.EnemiesKilled, BonusScore, Convert.ToInt32(SceneManager.GetSceneAt(0).name.Split('l')[1]), false, GameManager.instance.LevelDuration);
        LevelRecap.Instance.SceneName = "LevelSelect";
    }
}
