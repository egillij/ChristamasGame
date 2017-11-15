using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {    
    
    private IEnemyState currentState;
    
    public GameObject target;

    [SerializeField]
    private float meleeRange;

    [SerializeField]
    private float throwRange;

    public EdgeCollider2D SwordCollider;

    public bool InMeleeRange
    {
        get
        {
            if (target != null)
            {
                return Vector2.Distance(this.transform.position, target.transform.position) <= meleeRange;
            }

            return false;
        }
    }

    public bool InThrowRange
    {
        get
        {
            if (target != null)
            {
                return Vector2.Distance(this.transform.position, target.transform.position) <= throwRange;
            }

            return false;
        }
    }

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
        ChangeState(new IdleState());
        BoxCollider2D pCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(pCollider, this.GetComponent<BoxCollider2D>(), true);
        SwordCollider.enabled = false;
    }

    void Update()
    {
        if (IsDead)
        {
            return;
        }

        if (!TakingDamage)
        {
            currentState.Execute();
        }

        LookAtTarget();
    }

    private void LookAtTarget()
    {
        if (target != null)
        {
            float xDir = target.transform.position.x - this.transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack)
        {
            Animator.SetFloat("speed", 1.0f);
            transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
        }
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
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

    public void MeleeAttack()
    {
        SwordCollider.enabled = !SwordCollider.enabled;
    }
}
