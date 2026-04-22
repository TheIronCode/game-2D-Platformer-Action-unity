using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = .2f;
    private Coroutine damageFeedbackCoroutine;
    

    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;

    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] private float jumpForse = 8f;
    [SerializeField] private float fallMultiplier = 2f;
    protected int facingDir = 1;
    protected bool canMove = true;
    private bool isFacingRight = true;
    private bool canJump = true;
    private float xInput;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;
    }


    protected virtual void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimation();
        HandleFlip();
        ApplyJumpPhysics();

    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);

        foreach (Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHealth -= 1;
        PlayDamageFeedback();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
        {
            StopCoroutine(damageFeedbackCoroutine);
        }
        StartCoroutine(DamageFeedbackCoroutine());
    }

    private IEnumerator DamageFeedbackCoroutine()
    {
        Material originaMat = sr.material;

        sr.material = damageMaterial;

        yield return new WaitForSeconds(damageFeedbackDuration);

        sr.material = originaMat;
    }

    protected virtual void Die()
    {
        animator.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
    }

    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }

    protected void HandleAnimation()
    {
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToJump();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleAttack();
        }
    }

    protected virtual void HandleAttack()
    {
        if (isGrounded)
        {
            animator.SetTrigger("attack");
        }
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForse);
        }
    }

    protected virtual void HandleMovement()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    protected void HandleFlip()
    {
        if (xInput > 0 && isFacingRight == false)
        {
            Flip();
        }
        else if (xInput < 0 && isFacingRight == true)
        {
            Flip();
        }
    }

    private void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);

        isFacingRight = !isFacingRight;

        facingDir = facingDir * -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
