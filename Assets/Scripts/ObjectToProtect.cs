using UnityEngine;
using UnityEngine.Windows;

public class ObjectToProtect : Entity
{

    private Transform player;

    protected override void Awake()
    {
        base.Awake();

        player = FindFirstObjectByType<Player>().transform;

    }

    protected override void Update()
    {
        HandleFlip();
    }

    protected override void HandleFlip()
    {
        if (player == null)
        {
            return;
        }

        if (player.transform.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (player.transform.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    protected override void Die()
    {
        base.Die();
        UI.Instance.EnableGameOverUI();
    }

}
