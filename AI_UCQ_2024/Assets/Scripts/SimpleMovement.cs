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

    // Qu� tanto tiempo a futuro (o pasado, si es negativa) va a predecir el movimiento de su target.
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
        Debug.Log(message: "Se est� ejecutando Start." + gameObject.name);
        //debugManagerRef = GameObject.FindAnyObjectByType<DebugManager>();

        // Asegurar de que VisionCone est� asignado
        if (visionCone == null)
        {
            visionCone = GetComponentInChildren<VisionCone>();
        }

        return;
    }

    void Update()
    {
        //Debug.Log("Upadate n�mero: " + ContadorCuadros);
        //ContadorCuadros++;
        //Movimiento por cuadros
        //transform.position = new Vector3(x:ContadorCuadros, y:0, z:-1);

        //Cada cuadro se actualiza el vector que dice hacia donde perseguir al objetivo
        //Vector3 PosToTarget = PuntaMenosCola(targetGameObject.transform.position, transform.position); //seek
        // Vector3 PosToTarget = -PuntaMenosCola(targetGameObject.transform.position, transform.position);  // FLEE
        //velocity += PosToTarget.normalized * maxAcceleration * Time.deltaTime;

        // Hay que pedirle al targetGameObject que nos d� acceso a su Velocity, la cual est� en el script SimpleMovement
        Vector3 currentVelocity = targetGameObject.GetComponent<SimpleMovement>().velocity;

        PursuitTimePrediction = CalculatePredictedTime(maxSpeed, transform.position, targetGameObject.transform.position);

        // Primero predigo d�nde va a estar mi objetivo
        Vector3 PredictedPosition =
            PredictPosition(targetGameObject.transform.position, currentVelocity, PursuitTimePrediction);

        // Hago seek hacia la posici�n predicha.
        Vector3 PosToTarget = PuntaMenosCola(PredictedPosition, transform.position); // SEEK

        velocity += PosToTarget.normalized * maxAcceleration * Time.deltaTime;

        if (visionCone.targetDetected)
        {
            // Si el jugador est� dentro del cono de visi�n, mueve al enemigo
            PerseguirJugador();
            Debug.Log("Target detectado. Empezando movimiento.");
        }
        else
        {
            // Si no detecta al jugador, frenar el enemigo o hacer otra acci�n
            DetenerMovimiento();
            Debug.Log("Target no detectado. Deteniendo movimiento.");
        }

        //// Queremos que lo m�s r�pido que pueda ir sea a MaxSpeed unidades por segundo. Sin importar qu� tan grande sea la
        //// flecha de PosToTarget.
        //// Como la magnitud y la direcci�n de un vector se pueden separar, �nicamente necesitamos limitar la magnitud para
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
        // Con base en la Velocity dada vamos a calcular en qu� posici�n estar� nuestro objeto con posici�n InitialPosition,
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
            // Velocity S� tiene direcci�n y magnitud (es un vector de 1 o m�s dimensiones),
            // mientras que Speed no, �nicamente es una magnitud (o sea, un solo valor flotante)
            // Primero, dibujamos nuestra velocidad actual, partiendo desde nuestra posici�n.
            Gizmos.DrawLine(transform.position, transform.position + velocity);
        }
        // Ahora vamos con la "flecha azul" que es la direcci�n y magnitud hacia nuestro objetivo (la posici�n de nuestro objetivo).
        if (DebugGizmoManager.DesiredVectors && targetGameObject != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (targetGameObject.transform.position - transform.position));
        }

        if (targetGameObject != null)
        {
            // Vamos a dibujar la posici�n a futuro que est� prediciendo.
            Vector3 currentVelocity = targetGameObject.GetComponent<SimpleMovement>().velocity;

            PursuitTimePrediction = CalculatePredictedTime(maxSpeed, transform.position, targetGameObject.transform.position);

            // Primero predigo d�nde va a estar mi objetivo
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