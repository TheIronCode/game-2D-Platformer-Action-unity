using UnityEngine;
using UnityEngine.Windows;

public class Enemy : Entity
{

    private bool playerDetected;
    private bool canAttack = true;
    private float attackCooldown = 1f;

    protected override void Update()
    {
        HandleCollision();
        HandleAnimation();
        HandleMovement();
        HandleFlip();
        HandleAttack();
    }

    protected override void HandleAttack()
    {
        if(playerDetected && canAttack == true)
        {
            canAttack = false;
            animator.SetTrigger("attack");
        }
    }

    public override void EnableMovementAndJumpAndAttack(bool enable)
    {
        canAttack = enable;
        canMove = enable;
    }

    protected override void HandleMovement()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(facingDir * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }
}
