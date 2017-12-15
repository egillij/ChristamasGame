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

    public bool aggresive;

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

    public bool isFacingRight
    {
        get
        {
            return facingRight;
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

            HandleLayers();
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

    public void MakeJump()
    {
        if (!Attack)
        {
            Animator.SetTrigger("jump");
            Rbody.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
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

        if (Player.Instance.transform.position.x < transform.position.x)
        {
            if (facingRight) ChangeDirection();
        }
        else
        {
            if (!facingRight) ChangeDirection();
        }
        StartCoroutine(ExtendSight());
        

        if (!IsDead)
        {
            Animator.SetTrigger("damage");
        }
        else
        {
            Animator.SetBool("dead", true);
            GameManager.instance.EnemiesKilled += 1;
            yield return null;
        }
    }

    public void MeleeAttack()
    {
        SwordCollider.enabled = !SwordCollider.enabled;
    }

    public override void ChangeDirection()
    {
        base.ChangeDirection();
        BoxCollider2D[] childs = GetComponentsInChildren<BoxCollider2D>();
        foreach(BoxCollider2D child in childs)
        {
            if (child.gameObject.name == "Sight")
            {
                child.gameObject.transform.localScale = new Vector3(-1f * child.gameObject.transform.localScale.x, 1f, 1f);
            }
        }
        transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
        throwPosition.transform.position = throwPosition.transform.position + (facingRight ? Vector3.right : Vector3.left) * 4.5f;
        Vector3 swordBefore = SwordCollider.transform.position;
        SwordCollider.transform.localScale = new Vector3(SwordCollider.transform.localScale.x * -1.0f, 1.0f, 1.0f);
        SwordCollider.transform.position = swordBefore + (facingRight ? Vector3.right : Vector3.left) * 1.6f;
    }

    private IEnumerator ExtendSight()
    {
        BoxCollider2D[] childs = GetComponentsInChildren<BoxCollider2D>();
        BoxCollider2D sight = null;
        foreach(BoxCollider2D child in childs)
        {
            if (child.gameObject.name == "Sight")
            {
                sight = child;
            }
        }

        if (sight != null)
        {
            sight.size = new Vector2(sight.size.x * 10f, sight.size.y);
        }
        
        yield return new WaitForSeconds(1.0f);

        if (sight != null)
        {
            sight.size = new Vector2(sight.size.x / 10f, sight.size.y);
        }

    }
}
