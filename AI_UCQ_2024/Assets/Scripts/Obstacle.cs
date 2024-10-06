using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private float ObstacleForceToApply = 1.0f;

    // Queremos que cuando detecte un agente en su trigger, que lo empuje en otra direccion para que lo
    // desvie ligeramente hacia otra direccion.

    void OnTriggerStay(Collider other)
    {
        // Si detectamos a un agente dentro del trigger,
        // calculamos un vector con origen en la posici�n de este objeto, y cuyo fin es la posici�n de ese agente.
        Vector3 OriginToAgent = other.transform.position - transform.position;
        // y despu�s se lo aplicamos al agente como una fuerza que afecta su steering behavior.
        SimpleMovement otherSimpleMovement = other.gameObject.GetComponent<SimpleMovement>();
        // (le podemos poner una variable para ajustar la cantidad de fuerza que se le puede aplicar al agente).

        if (otherSimpleMovement == null )
        {
            return;
        } 
        else
        {
            //Entre más cerca esté el agenta del obstáculo, más grande la fuerza.
            //Entre menor distancia entre ambos objetos, mayor fuerza aplicada.
            float distance = OriginToAgent.magnitude;

            SphereCollider collider = GetComponent<SphereCollider>();
            if (collider == null)
            {
                return;
            }
            
            float obstacleColliderRadius = collider.radius;

            float calculateForce = ObstacleForceToApply * (1.0f - distance / obstacleColliderRadius);

            otherSimpleMovement.AddExternalForce(OriginToAgent.normalized * ObstacleForceToApply);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}