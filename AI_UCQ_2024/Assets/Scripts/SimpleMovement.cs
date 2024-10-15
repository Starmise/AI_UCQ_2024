using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugManager;
using System;

public enum WeaponType
{
    Weak,
    Normal,
    Hielo,
    Fuego,
    Trueno
}
public enum EnemyState
{
    Idle,
    Alert, // sospechoso/investigando/etc.
    Attack // ya te detectó y te ataca.
}

public enum ExampleMessageBits
{
    ClientOrServer = 1, // [0 0 0 0 0 0 0 X ]
    PremiumUser = 2, // [0 0 0 0 0 0 X 0 ]
    EstáVolando = 4, // [0 0 0 0 0 X 0 0 ]
    OtraCosa = 8, // [0 0 0 0 X 0 0 0 ]
    OtraMas = 16,
}
public enum Layers
{
    Default = 1,
    TransparentFX = 2,
}

public class SimpleMovement : MonoBehaviour
{
    [SerializeField]
    protected float maxSpeed = 5;
    [SerializeField]
    protected float maxAcceleration = 1.0f;

    public Vector3 velocity = Vector3.zero;

    public GameObject targetGameObject = null;

    // Qué tanto tiempo a futuro (o pasado, si es negativa) va a predecir el movimiento de su target.
    protected float PursuitTimePrediction = 1.0f;

    public VisionCone visionCone;

    [SerializeField]
    protected float ObstacleForceToApply = 1.0f;

    protected Vector3 ExternalForces = Vector3.zero;

    [SerializeField] private WeaponType myWeaponType;
    protected EnemyState currentEnemyState = EnemyState.Idle;

    [SerializeField] protected String ObstacleLayerName = "Obstacle";

    public void AddExternalForce(Vector3 ExternalForce)
    {
        ExternalForces += ExternalForce;
    }

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

    void OnTriggerStay(Collider other)
    {

        // Si esta colisión es contra alguien que NO es un obstáculo (no tiene la Layer Obstacle),
        // entonces, no hagas nada.
        if (other.gameObject.layer != LayerMask.NameToLayer("ObstacleLayerName"))
        {
            return;
        }

        // Si detectamos que un agente está dentro de nuestro radio/área de activación,
        // calculamos un vector con origen en la posición de este objeto, y cuyo fin es la posición de ese agente
        // NOTA: Esta resta es hacia el CENTRO del agente.
        Vector3 OriginToAgent = transform.position - other.transform.position;

        Debug.Log("Entré a OnTriggerStay de SimpleMovement con: " + other.gameObject.name);

        float distance = OriginToAgent.magnitude;

        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider == null)
        {
            return;
        }

        // collider.radius nos da el radio en espacio local, nosotros lo necesitamos
        // escalado por las escalas de sus padres en la Jerarquía de la escena. 
        float obstacleColliderRadius = collider.radius;

        float calculatedForce = ObstacleForceToApply * (1.0f - distance / obstacleColliderRadius);

        AddExternalForce(OriginToAgent.normalized * calculatedForce);
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

        switch (myWeaponType)
        {
            case WeaponType.Weak:
                // Atacar con el arma weak
                break;
            case WeaponType.Normal:
                // atacar con el arma Normal.
                break;
        }

        if (targetGameObject == null)
        {
            return;
        }

        Vector3 currentVelocity = targetGameObject.GetComponent<SimpleMovement>().velocity;

        PursuitTimePrediction = CalculatePredictedTime(maxSpeed, transform.position, targetGameObject.transform.position);

        Vector3 PredictedPosition =
            PredictPosition(targetGameObject.transform.position, currentVelocity, PursuitTimePrediction);

        Vector3 PosToTarget = PuntaMenosCola(PredictedPosition, transform.position); // SEEK

        PosToTarget += ExternalForces;

        velocity += PosToTarget.normalized * maxAcceleration * Time.deltaTime;

        // Queremos que lo más rápido que pueda ir sea a MaxSpeed unidades por segundo.
        // Limitamos la magnitud para que no sobrepase el valor de MaxSpeed.
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * Time.deltaTime;

        // Hay que resetearlas cada frame al final del cuadro, si no se quitará antes de usarla.
        ExternalForces = Vector3.zero;

        /*if (visionCone.targetDetected)
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
        }*/
    }

    /*void PerseguirJugador()
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
    }*/

    protected Vector3 PredictPosition(Vector3 InitialPosition, Vector3 Velocity, float TimePrediction)
    {
        // Con base en la Velocity dada vamos a calcular en qué posición estará nuestro objeto con posición InitialPosition,
        // tras una cantidad X de tiempo (TimePrediction).
        return InitialPosition + Velocity * TimePrediction;
    }

    protected float CalculatePredictedTime(float MaxSpeed, Vector3 InitialPosition, Vector3 TargetPosition)
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

            // Predecir dónde va a estar mi objetivo
            Vector3 PredictedPosition =
                PredictPosition(targetGameObject.transform.position, currentVelocity, PursuitTimePrediction);

            Gizmos.color = Color.red;
            Gizmos.DrawCube(PredictedPosition, Vector3.one);
        }
    }

    protected Vector3 Pursuit(Vector3 TargetCurrentPosition, Vector3 TargetCurrentVelocity)
    {
        PursuitTimePrediction = CalculatePredictedTime(maxSpeed, transform.position, TargetCurrentPosition);
        // Predecir dónde va a estar mi objetivo
        Vector3 PredictedPosition =
            PredictPosition(TargetCurrentPosition, TargetCurrentVelocity, PursuitTimePrediction);
        // Seek a la posición predicha.
        return PuntaMenosCola(PredictedPosition, transform.position); // SEEK
    }
    protected Vector3 Evade(Vector3 TargetCurrentPosition, Vector3 TargetCurrentVelocity)
    {
        return -Pursuit(TargetCurrentPosition, TargetCurrentVelocity);
    }

    int RetornarInt()
    {
        return 0;
    }
}