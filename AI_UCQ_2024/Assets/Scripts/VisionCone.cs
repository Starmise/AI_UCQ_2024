using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public float viewRadius = 5f; // Radio del cono de visión
    [Range(0, 360)]
    public float viewAngle = 90f; // Ángulo del cono de visión

    public Transform target; // El GameObject que queremos detectar

    private void Update()
    {
        DetectTargetInView();
    }

    void DetectTargetInView()
    {
        // Verificamos si el target está dentro del radio de visión
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        if (Vector3.Distance(transform.position, target.position) < viewRadius)
        {
            // Verificamos si el target está dentro del ángulo de visión
            float angleBetweenAgentAndTarget = Vector3.Angle(transform.forward, directionToTarget);
            if (angleBetweenAgentAndTarget < viewAngle / 2)
            {
                Debug.Log("Target Detectado: " + target.name);
            }
        }
    }

    // Método auxiliar para visualizar el cono de visión en la escena de Unity
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        // Dibujamos el cono de visión como un sector circular
        DrawVisionCone();
    }

    // Dibuja el cono de visión
    private void DrawVisionCone()
    {
        // Dibujamos una línea desde el agente hacia el target (solo para depuración)
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }

        // Cono de visión
        Vector3 forward = transform.forward * viewRadius;
        float step = viewAngle / 20; // Aumentar el número de pasos para suavizar el cono
        Vector3 start = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;

        Gizmos.color = Color.yellow;
        for (float i = -viewAngle / 2; i <= viewAngle / 2; i += step)
        {
            Vector3 direction = Quaternion.Euler(0, i, 0) * forward;
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }
    }
}