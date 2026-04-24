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

    private void DisableAction()
    {
        player.EnableAction(false);
    }

    private void EnableAction()
    {
        player.EnableAction(true);
    }
}
