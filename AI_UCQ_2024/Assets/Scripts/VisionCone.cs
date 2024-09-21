using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public float viewRadius = 5f; // Radio del cono de visi�n
    [Range(0, 360)]
    public float viewAngle = 90f; // �ngulo del cono de visi�n

    public Transform target; // El GameObject que queremos detectar

    private void Update()
    {
        DetectTargetInView();
    }

    void DetectTargetInView()
    {
        // Verificamos si el target est� dentro del radio de visi�n
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        if (Vector3.Distance(transform.position, target.position) < viewRadius)
        {
            // Verificamos si el target est� dentro del �ngulo de visi�n
            float angleBetweenAgentAndTarget = Vector3.Angle(transform.forward, directionToTarget);
            if (angleBetweenAgentAndTarget < viewAngle / 2)
            {
                Debug.Log("Target Detectado: " + target.name);
            }
        }
    }

    // M�todo auxiliar para visualizar el cono de visi�n en la escena de Unity
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        // Dibujamos el cono de visi�n como un sector circular
        DrawVisionCone();
    }

    // Dibuja el cono de visi�n
    private void DrawVisionCone()
    {
        // Dibujamos una l�nea desde el agente hacia el target (solo para depuraci�n)
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }

        // Cono de visi�n
        Vector3 forward = transform.forward * viewRadius;
        float step = viewAngle / 20; // Aumentar el n�mero de pasos para suavizar el cono
        Vector3 start = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;

        Gizmos.color = Color.yellow;
        for (float i = -viewAngle / 2; i <= viewAngle / 2; i += step)
        {
            Vector3 direction = Quaternion.Euler(0, i, 0) * forward;
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }
    }
}