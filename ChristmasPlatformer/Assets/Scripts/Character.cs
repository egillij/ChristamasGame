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

    public bool Attack { get; set; }

    protected bool facingRight;


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
        transform.position = positionBefore;
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
}
