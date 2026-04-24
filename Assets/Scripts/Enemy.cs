using UnityEngine;
using UnityEngine.Windows;

public class Enemy : Entity
{

    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;

    private bool playerDetected;
    private bool canAttack = true;

    protected override void Update()
    {
        base.Update();

        HandleAttack();
    }

    protected override void HandleAttack()
    {
        if(playerDetected && canAttack)
        {
            canAttack = false;
            animator.SetTrigger("attack");
        }
    }

    public override void EnableAction(bool enable)
    {
        base.EnableAction(enable);
        canAttack = enable;
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

    protected override void Die()
    {
        base.Die();
        UI.Instance.AddKillCount();
    }
}
