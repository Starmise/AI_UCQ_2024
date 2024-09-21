using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugManager;

public class VisionCone : MonoBehaviour
{
    //�ngulo y radio de visi�n
    public float viewRadius = 5f;
    [Range(0, 360)]
    public float viewAngle = 90f;

    public Transform target;

    public bool targetDetected = false;
    private Color coneColor = Color.green;

    private void Update()
    {
        DetectTargetInView();

        if (targetDetected)
        {
            RotateTowardsTarget();
        }
    }

    void DetectTargetInView()
    {
        targetDetected = false;

        // Verificamos si el target est� dentro del radio de visi�n
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        if (Vector3.Distance(transform.position, target.position) < viewRadius)
        {
            // Verificamos si el target est� dentro del �ngulo de visi�n
            float angleBetweenAgentAndTarget = Vector3.Angle(transform.forward, directionToTarget);
            if (angleBetweenAgentAndTarget < viewAngle / 2)
            {
                targetDetected = true;
                coneColor = Color.red;
                Debug.Log("Target Detectado: " + target.name);
            }
        }

        // Si no detectamos el target, el color vuelve a verde
        if (!targetDetected)
        {
            coneColor = Color.green;
        }
    }

    void RotateTowardsTarget()
    {
        // Calculamos la direcci�n hacia el objetivo
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // Calculamos la rotaci�n deseada
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        // Interpolamos la rotaci�n para que sea m�s suave
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
    }

    private void OnDrawGizmos()
    {
        // Verificamos si se debe mostrar el cono de visi�n basado en la variable DebugManager.DebugGizmoManager.ShowVisionCone
        if (DebugManager.DebugGizmoManager.ShowVisionCone)
        {
            Gizmos.color = coneColor;
            DrawVisionCone();
        }
    }

    private void DrawVisionCone()
    {
        // Cono de visi�n
        Vector3 forward = transform.forward * viewRadius;
        float step = viewAngle / 10;
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

//--------REFERENCIAS---------
//https://www.youtube.com/watch?v=lV47ED8h61k&t=1s
//https://docs.unity3d.com/ScriptReference/Quaternion.LookRotation.html
//https://docs.unity3d.com/ScriptReference/Quaternion.RotateTowards.html