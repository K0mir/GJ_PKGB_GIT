using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMoveCasaDentro : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 7f;

    [Header("Configuración Sprite")]
    public bool spriteDefaultFacesRight = true; // AJUSTA ESTO SEGÚN TU SPRITE
    
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Animator animator;
    private Vector3 originalScale;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Leer input
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Animación de correr
        bool isRunning = Mathf.Abs(moveInput.x) > 0.1f;
        animator.SetBool("running", isRunning);

        // Volteo simple y efectivo
        if (moveInput.x > 0.1f) // Moverse a la derecha
        {
            if (spriteDefaultFacesRight)
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            else
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (moveInput.x < -0.1f) // Moverse a la izquierda
        {
            if (spriteDefaultFacesRight)
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            else
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        // Salto
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
