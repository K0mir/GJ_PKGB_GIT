using UnityEngine.InputSystem;
using UnityEngine;


public class SparkyMoveLv3 : MonoBehaviour
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

    //  Escala original del personaje
    private Vector3 originalScale;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Guardar la escala original al iniciar
        originalScale = transform.localScale;

        // Si no se asignó groundCheck, usar el transform actual
        if (groundCheck == null)
            groundCheck = transform;
    }

    void Update()
    {
        // Leer input de movimiento
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Verificar si está en el suelo usando OverlapCircle
        CheckGrounded();

        // Verificar si está corriendo
        bool isRunning = Mathf.Abs(moveInput.x) > 0.1f;
        animator.SetBool("running", isRunning);

        //  Voltear sprite sin modificar la escala base
        if (moveInput.x > 0.1f)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (moveInput.x < -0.1f)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // Salto - usando triggered para detectar solo una vez
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            Jump();
        }

        // Actualizar animación de salto
        animator.SetBool("jumping", !isGrounded);

        // Acciones adicionales (comentadas)
        // if (playerInput.actions["Oler"]?.triggered ?? false)
        // {
        //     animator.SetTrigger("smell");
        //     Debug.Log("Oler activado");
        // }

        // if (playerInput.actions["Morder"]?.triggered ?? false)
        //     animator.SetTrigger("bite");

        // if (playerInput.actions["Ladrar"]?.triggered ?? false)
        //     animator.SetTrigger("bark");
    }

    void FixedUpdate()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    void CheckGrounded()
    {
        // Verificar si está tocando el suelo usando Physics2D.OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Jump()
    {
        // Aplicar fuerza de salto
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;

    }

    // Dibujar gizmo para ver el área de detección de suelo en el Editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detectar suelo (backup por si acaso)
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("jumping", false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Mantener la detección de suelo mientras esté en contacto
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Cuando deja de tocar el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}