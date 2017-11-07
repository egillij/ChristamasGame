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

    public override void Start()
    {
        base.Start();
        ChangeState(new IdleState());
    }

    void Update()
    {
        currentState.Execute();

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter(collision);
    }


}
