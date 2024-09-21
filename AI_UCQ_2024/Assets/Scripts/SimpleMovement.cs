using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugManager;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] 
    private int ContadorCuadros = 0;
    [SerializeField]
    protected float maxSpeed = 5;
    [SerializeField] 
    protected float maxAcceleration = 1.0f;

    public Vector3 velocity = Vector3.zero;

    public GameObject targetGameObject = null;

    // Qué tanto tiempo a futuro (o pasado, si es negativa) va a predecir el movimiento de su target.
    protected float PursuitTimePrediction = 1.0f;

    public VisionCone visionCone;

    public Vector3 PuntaMenosCola(Vector3 punta, Vector3 cola)
    {
        float x = punta.x - cola.x;
        float y = punta.y - cola.y;
        float z = punta.z - cola.z;

        return new Vector3(x, y, z);
    }

    protected void Start()
    {
        Debug.Log(message: "Se está ejecutando Start." + gameObject.name);
        //debugManagerRef = GameObject.FindAnyObjectByType<DebugManager>();

        // Asegurar de que VisionCone esté asignado
        if (visionCone == null)
        {
            visionCone = GetComponentInChildren<VisionCone>();
        }

        return;
    }

    void Update()
    {
        //Debug.Log("Upadate número: " + ContadorCuadros);
        //ContadorCuadros++;
        //Movimiento por cuadros
        //transform.position = new Vector3(x:ContadorCuadros, y:0, z:-1);

        //Cada cuadro se actualiza el vector que dice hacia donde perseguir al objetivo
        //Vector3 PosToTarget = PuntaMenosCola(targetGameObject.transform.position, transform.position); //seek
        // Vector3 PosToTarget = -PuntaMenosCola(targetGameObject.transform.position, transform.position);  // FLEE
        //velocity += PosToTarget.normalized * maxAcceleration * Time.deltaTime;

        // Hay que pedirle al targetGameObject que nos dé acceso a su Velocity, la cual está en el script SimpleMovement
        Vector3 currentVelocity = targetGameObject.GetComponent<SimpleMovement>().velocity;

        PursuitTimePrediction = CalculatePredictedTime(maxSpeed, transform.position, targetGameObject.transform.position);

        // Primero predigo dónde va a estar mi objetivo
        Vector3 PredictedPosition =
            PredictPosition(targetGameObject.transform.position, currentVelocity, PursuitTimePrediction);

        // Hago seek hacia la posición predicha.
        Vector3 PosToTarget = PuntaMenosCola(PredictedPosition, transform.position); // SEEK

        velocity += PosToTarget.normalized * maxAcceleration * Time.deltaTime;

        if (visionCone.targetDetected)
        {
            // Si el jugador está dentro del cono de visión, mueve al enemigo
            PerseguirJugador();
            Debug.Log("Target detectado. Empezando movimiento.");
        }
        else
        {
            // Si no detecta al jugador, frenar el enemigo o hacer otra acción
            DetenerMovimiento();
            Debug.Log("Target no detectado. Deteniendo movimiento.");
        }

        //// Queremos que lo más rápido que pueda ir sea a MaxSpeed unidades por segundo. Sin importar qué tan grande sea la
        //// flecha de PosToTarget.
        //// Como la magnitud y la dirección de un vector se pueden separar, únicamente necesitamos limitar la magnitud para
        //// que no sobrepase el valor de MaxSpeed.
        //velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        //transform.position += velocity * Time.deltaTime;
    }

    void PerseguirJugador()
    {
        if (targetGameObject != null)
        {
            Vector3 PosToTarget = PuntaMenosCola(targetGameObject.transform.position, transform.position);
            
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            transform.position += velocity * Time.deltaTime;
        }
    }

    void DetenerMovimiento()
    {
        velocity = Vector3.zero;
    }

    Vector3 PredictPosition(Vector3 InitialPosition, Vector3 Velocity, float TimePrediction)
    {
        // Con base en la Velocity dada vamos a calcular en qué posición estará nuestro objeto con posición InitialPosition,
        // tras una cantidad X de tiempo (TimePrediction).
        return InitialPosition + Velocity * TimePrediction;

        // nosotros empezamos
    }

    float CalculatePredictedTime(float MaxSpeed, Vector3 InitialPosition, Vector3 TargetPosition)
    {
        // Primero obtenemos la distancia entre InitialPosition y TargetPosition. 
        // y nos quedamos con la pura magnitud, porque solo queremos saber la distancia.
        float Distance = PuntaMenosCola(TargetPosition, InitialPosition).magnitude;

        return Distance / MaxSpeed;
    }

    void OnDrawGizmos()
    {
        if (DebugGizmoManager.VelocityLines)
        {
            Gizmos.color = Color.yellow;
            // Velocity SÍ tiene dirección y magnitud (es un vector de 1 o más dimensiones),
            // mientras que Speed no, únicamente es una magnitud (o sea, un solo valor flotante)
            // Primero, dibujamos nuestra velocidad actual, partiendo desde nuestra posición.
            Gizmos.DrawLine(transform.position, transform.position + velocity);
        }
        // Ahora vamos con la "flecha azul" que es la dirección y magnitud hacia nuestro objetivo (la posición de nuestro objetivo).
        if (DebugGizmoManager.DesiredVectors && targetGameObject != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (targetGameObject.transform.position - transform.position));
        }

        if (targetGameObject != null)
        {
            // Vamos a dibujar la posición a futuro que está prediciendo.
            Vector3 currentVelocity = targetGameObject.GetComponent<SimpleMovement>().velocity;

            PursuitTimePrediction = CalculatePredictedTime(maxSpeed, transform.position, targetGameObject.transform.position);

            // Primero predigo dónde va a estar mi objetivo
            Vector3 PredictedPosition =
                PredictPosition(targetGameObject.transform.position, currentVelocity, PursuitTimePrediction);

            Gizmos.color = Color.red;
            Gizmos.DrawCube(PredictedPosition, Vector3.one);
        }
    }

    int RetornarInt()
    {
        return 0;
    }
}