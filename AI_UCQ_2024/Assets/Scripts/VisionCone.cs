using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugManager;

public class VisionCone : MonoBehaviour
{
    public float viewRadius = 5f; // Radio del cono de visi�n
    [Range(0, 360)]
    public float viewAngle = 90f; // �ngulo del cono de visi�n

    public Transform target; // El GameObject que queremos detectar

    private bool targetDetected = false; // Variable que almacena si el objetivo est� dentro del cono de visi�n
    private Color coneColor = Color.green; // Color del cono (verde por defecto)

    private void Update()
    {
        DetectTargetInView();
    }

    void DetectTargetInView()
    {
        targetDetected = false; // Reiniciamos la detecci�n

        // Verificamos si el target est� dentro del radio de visi�n
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        if (Vector3.Distance(transform.position, target.position) < viewRadius)
        {
            // Verificamos si el target est� dentro del �ngulo de visi�n
            float angleBetweenAgentAndTarget = Vector3.Angle(transform.forward, directionToTarget);
            if (angleBetweenAgentAndTarget < viewAngle / 2)
            {
                targetDetected = true; // El objetivo est� dentro del cono de visi�n
                coneColor = Color.red; // Cambiamos el color a rojo
                Debug.Log("Target Detectado: " + target.name);
            }
        }

        // Si no detectamos el target, el color vuelve a verde
        if (!targetDetected)
        {
            coneColor = Color.green;
        }
    }

    // M�todo auxiliar para visualizar el cono de visi�n en la escena de Unity
    private void OnDrawGizmos()
    {
        Gizmos.color = coneColor; // El color del cono depende de la detecci�n

        // Dibujamos el cono de visi�n como un sector circular
        DrawVisionCone();
    }

    // Dibuja el cono de visi�n
    private void DrawVisionCone()
    {
        // Cono de visi�n
        Vector3 forward = transform.forward * viewRadius;
        float step = viewAngle / 20; // Aumentar el n�mero de pasos para suavizar el cono
        Vector3 start = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;

        for (float i = -viewAngle / 2; i <= viewAngle / 2; i += step)
        {
            Vector3 direction = Quaternion.Euler(0, i, 0) * forward;
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }

        // Opci�n: dibujamos una l�nea hacia el target solo para depuraci�n (puede eliminarse)
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
