using UnityEngine;

public class Entity_AnimationsEvents : MonoBehaviour
{
    private Entity player;

    private void Awake()
    {
        player = GetComponentInParent<Entity>();

    }

    public void DamageTargets()
    {
        player.DamageTargets();
    }

    private void DisableMovementAndJumpAndAttack()
    {
        player.EnableMovementAndJumpAndAttack(false);
    }

    private void EnableMovementAndJumpAndAttack()
    {
        player.EnableMovementAndJumpAndAttack(true);
    }
}
