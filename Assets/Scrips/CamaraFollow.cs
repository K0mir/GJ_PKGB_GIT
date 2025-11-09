using UnityEngine;

public class CamaaraFlo : MonoBehaviour
{
    [Header("Seguimiento")]
    public Transform target; // El jugador
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    [Header("Límites de cámara")]
    public bool useLimits = true;
    public float minX, maxX;
    public float minY, maxY;

    void LateUpdate()
    {
        if (target == null) return;

        // Posición deseada (con offset)
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Aplicar límites si están activados
        if (useLimits)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        }

        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
