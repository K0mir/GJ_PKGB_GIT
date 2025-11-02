using UnityEngine;
using UnityEngine.InputSystem;

public class SparkyRunner : MonoBehaviour
{
 [Header("Movimiento automático")]
    public float speed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    [Header("Animación")]
    private Animator animator;

    private PlayerInput playerInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // Movimiento automático en el eje X
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        // Actualizar animación de correr
        animator.SetBool("running", true);

        // Salto
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
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
