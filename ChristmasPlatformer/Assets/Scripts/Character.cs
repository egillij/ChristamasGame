using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Animator Animator { get; private set; }

    [SerializeField]
    protected float movementSpeed;

    [SerializeField]
    protected Transform throwPosition;

    [SerializeField]
    protected GameObject throwablePrefab;

    [SerializeField]
    protected int health;

    public bool Attack { get; set; }
    public abstract bool IsDead { get; }

    protected bool facingRight;

    public bool TakingDamage { get; set; }

    public abstract IEnumerator TakeDamage();

    public virtual void Start()
    {
        facingRight = true;
        Animator = GetComponent<Animator>();

    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        Vector3 positionBefore = transform.position;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        transform.position = positionBefore + (facingRight ? Vector3.right : Vector3.left) * 1.6f;
        
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
                Destroy(other.gameObject);
                StartCoroutine(TakeDamage());
            }            
        }
    }
}            