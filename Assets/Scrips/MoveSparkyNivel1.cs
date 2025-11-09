using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSparkyNivel1 : MonoBehaviour
{
[Header("Movimiento")]
    public float speed = 5f;

    private Rigidbody2D rb;

    private PlayerInput playerInput;
    private Vector2 moveInput;

    [Header("Animación")]
    private Animator animator;

    // 🔹 Guardamos la escala original para usar float libremente (1.5, 2, etc.)
    private Vector3 originalScale;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Guardamos la escala original del personaje
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Leer input de movimiento
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Verificar si está corriendo
        bool isRunning = Mathf.Abs(moveInput.x) > 0.1f;
        animator.SetBool("running", isRunning);

        // Voltear sprite según dirección, respetando escala con decimales
        if (moveInput.x > 0.1f)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (moveInput.x < -0.1f)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void FixedUpdate()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }
}
