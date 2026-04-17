using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float jumpForse = 8f;
    [SerializeField] private float fallMultiplier = 2f;
    private float xInput;
    private bool isFacingRight = true;
    private bool canMove = true;
    private bool canJump = true;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimation();
        HandleFlip();
        ApplyJumpPhysics();

    }

    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }

    private void HandleAnimation()
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
            TryToAttack();
        }
    }

    private void TryToAttack()
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

    private void HandleMovement()
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

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleFlip()
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
    }
}
