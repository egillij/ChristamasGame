using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState {

    private Enemy enemy;

    private float patrolTimer;

    private float patrolDuration = 10.0f;

    private float turnaroundTimer;

    private float turnaroundCooldown = 3.0f;

    private float randomThrowTimer;

    private float randomThrowCooldown = 5.0f;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Patrol();

        SurpriseTurnaround();
        RandomThrow();

        enemy.Move();

        if (enemy.target != null && enemy.InThrowRange)
        {
            enemy.ChangeState(new RangedState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Edge")
        {
            enemy.ChangeDirection();
            turnaroundTimer = 0.0f;
        }

        else if (other.tag.Contains("Jump"))
        {
            if (Random.value > 0.0)
            {
                if (other.tag.Contains("Left") && !enemy.isFacingRight)
                {
                    enemy.MakeJump();
                    turnaroundTimer = 0.0f;
                }
                else if (other.tag.Contains("Right") && enemy.isFacingRight)
                {
                    enemy.MakeJump();
                    turnaroundTimer = 0.0f;
                }
            }
            
        }
    }

    private void Patrol()
    {

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    private void SurpriseTurnaround()
    {
        turnaroundTimer += Time.deltaTime;
        if (Random.value > 0.99f && turnaroundTimer > turnaroundCooldown)
        {
            enemy.ChangeDirection();
            turnaroundTimer = 0.0f;
        }
    }

    private void RandomThrow()
    {
        randomThrowTimer += Time.deltaTime;
        if (Random.value > 0.9f && randomThrowTimer > randomThrowCooldown)
        {
            enemy.ChangeState(new RangedState());
            randomThrowTimer = 0.0f;
        }
    }
}
