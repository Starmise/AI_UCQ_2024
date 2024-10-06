using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Utilities;
using DebugManager;
using Unity.VisualScripting;

public class SteeringBehaviorMovement : SimpleMovement
{
    protected Rigidbody rb = null;

    public Vector3 TargetPos = Vector3.zero;

    public Vector3 SpherePos = Vector3.zero;
    public float SphereRadius = 1.0f;
    // Para obtener la distancia entre dos puntos en el espacio, simplemente hacemos
    // Punta menos Cola, pero nos quedamos únicamente con la magnitud de dicho vector.

    bool IsInsideSphere()
    {
        // Para saber si un punto en el espacio (llamado TargetPos) está dentro o fuera de una esfera en el espacio, 
        // hacemos un vector que inicia en el origen de la esfera y que termine en TargetPos (punta menos cola)
        Vector3 SphereToTarget = TargetPos - SpherePos;
        // Y luego obtenemos la magnitud de dicho vector
        float VectorMagnitude = SphereToTarget.magnitude;
        // y finalmente comparamos esa magnitud con el radio de la esfera.
        // Si el radio es mayor o igual que la magnitud de ese vector, entonces TargetPos está dentro de la esfera
        if (SphereRadius >= VectorMagnitude)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entré a OnTriggerEnter chocando con: " + other.gameObject.name);
    }

    void Start()
    {
        // En vez de sobreescribir el método Start de la clase padre, lo vamos a extender.
        base.Start();

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 PosToTarget = PuntaMenosCola(targetGameObject.transform.position, transform.position); // SEEK

        // Force o Acceleration nos dan lo mismo ahorita porque no vamos a modificar la masa.
        rb.AddForce(PosToTarget.normalized * maxAcceleration, ForceMode.Force);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

    }

    void OnDrawGizmos()
    {

        if (DebugGizmoManager.DetectionSphere)
        {
            if (Utility.IsInsideRadius(targetGameObject.transform.position, transform.position, SphereRadius))
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            // Vamos a dibujar nuestra esfera (con su radio)
            Gizmos.DrawWireSphere(transform.position, SphereRadius);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetGameObject.transform.position, Vector3.one);

        if (DebugGizmoManager.DesiredVectors)
        {
            Gizmos.color = Color.blue;
            // Y la flecha del origen de nuestra esfera (transform.position) hasta la TargetPos
            Gizmos.DrawLine(transform.position, targetGameObject.transform.position);
        }
    }
}
