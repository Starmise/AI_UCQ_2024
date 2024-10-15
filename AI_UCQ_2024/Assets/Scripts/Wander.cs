using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Wander : SimpleMovement
{
    // Qué tan lejos genera el punto de Wander para hacer seek.
    [SerializeField] private float WanderDistance = 3.0f;
    [SerializeField] private float ToleranceRadius = 3.0f;
    Vector3 CurrentWanderPoint = Vector3.zero;

    Vector3 GenerateWanderPoint()
    {
        // Se rota una cantidad aleatoria de grados
        float RandomDegrees = Random.value * 359.9f;
        Vector3 RotatedForward = Quaternion.AngleAxis(RandomDegrees, transform.up) * transform.forward;

        // Se calcula desde la posición del personaje, por la magnitud (WanderDistance).
        Vector3 WanderPosition = transform.position + RotatedForward * WanderDistance;
        return WanderPosition;
    }

    void Start()
    {
        CurrentWanderPoint = GenerateWanderPoint();
    }

    void Update()
    {
        if (Utilities.Utility.IsInsideRadius(CurrentWanderPoint, transform.position, ToleranceRadius))
        {
            CurrentWanderPoint = GenerateWanderPoint();
        }
        Vector3 OurPositionToWanderPoint = CurrentWanderPoint - transform.position;
        velocity += OurPositionToWanderPoint.normalized * maxAcceleration * Time.deltaTime;

        // Queremos que lo más rápido que pueda ir sea a MaxSpeed unidades por segundo.
        // Separamos la magnitud del vector para que no sobrepase el valor de MaxSpeed.
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(CurrentWanderPoint, 2.0f);
    }
}
