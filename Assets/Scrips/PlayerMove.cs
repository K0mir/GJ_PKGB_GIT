using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMove : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    private PlayerInput playerInput;
    private Vector2 moveInput;

    [Header("Animación")]
    private Animator animator;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Leer input de movimiento
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Verificar si está corriendo
        bool isRunning = Mathf.Abs(moveInput.x) > 0.1f;
        animator.SetBool("running", isRunning);

        // Voltear sprite según dirección
        if (moveInput.x > 0.1f)
            transform.localScale = new Vector3(2, 2, 1);
        else if (moveInput.x < -0.1f)
            transform.localScale = new Vector3(-2, 2, 1);

        // Salto
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            // animator.SetBool("jumping", true);
        }

        // Animaciones especiales
        if (playerInput.actions["Oler"]?.triggered ?? false)
        {
            animator.SetTrigger("smell");
            Debug.Log("Oler activado");
        }
        // if (playerInput.actions["Oler"]?.triggered ?? false)
        //     animator.SetTrigger("smell");

        if (playerInput.actions["Morder"]?.triggered ?? false)
            animator.SetTrigger("bite");

        if (playerInput.actions["Ladrar"]?.triggered ?? false)
            animator.SetTrigger("bark");
    }

    void FixedUpdate()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detectar suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            // animator.SetBool("jumping", false);
        }
    }
}
