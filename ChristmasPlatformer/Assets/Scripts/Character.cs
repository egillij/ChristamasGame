using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Animator Animator { get; private set; }

    public Rigidbody2D Rbody { get; private set; }

    [SerializeField]
    public float movementSpeed;

    [SerializeField]
    protected Transform throwPosition;

    [SerializeField]
    protected GameObject throwablePrefab;

    [SerializeField]
    protected int health;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    public bool Attack { get; set; }
    public abstract bool IsDead { get; }

    protected bool facingRight;

    public bool TakingDamage { get; set; }

    public abstract IEnumerator TakeDamage();

    public virtual void Start()
    {
        Rbody = GetComponent<Rigidbody2D>();
        facingRight = true;
        Animator = GetComponentInChildren<Animator>();


    }

    public virtual void ChangeDirection()
    {
        facingRight = !facingRight;
        
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        Vector3 positionBefore = renderer.transform.position;

        renderer.flipX = !renderer.flipX;
        renderer.transform.position = positionBefore + (facingRight ? Vector3.right * 1.6f : Vector3.left * 1.6f);
        
    }

    public bool IsGrounded()
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

    public void HandleLayers()
    {
        if (!IsGrounded())
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


    public virtual void ThrowAttack(int value)
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

    public virtual void OnTriggerEnter2D (Collider2D other)
    {
        if (gameObject.name == "Player")
        {
            if ((other.tag == "Knife" || other.tag == "Sword" || other.tag == "Hell")
                || (other.tag == "Bomb" && other is CircleCollider2D)
            ) {
                if (other.tag == "Knife") Destroy(other.gameObject);
                StartCoroutine(TakeDamage());
            }
        }
        else
        {
            if (other.tag == "Snowball")
            {
                if (!IsDead)
                {
                    Destroy(other.gameObject);
                    StartCoroutine(TakeDamage());
                }
            }            
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (gameObject.name == "Player" && other.gameObject.tag == "Icicle")
        {
            StartCoroutine(TakeDamage());
        }
    }
}            