using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Animator Animator { get; private set; }

    [SerializeField]
    protected float movementSpeed;

    protected bool facingRight;
    
    public virtual void Start ()
    {
        facingRight = true;
        Animator = GetComponent<Animator>();
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
    }
}
