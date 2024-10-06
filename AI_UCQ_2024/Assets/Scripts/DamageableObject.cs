using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    // Objeto al que le puede poner una cierta resistencia/HP, 
    // despu�s de que se le reduce a <= 0 esa HP, se destruye
    [SerializeField] private float HP;

    // Tenemos que tener una forma de reducirle el HP
    // Si tocan o atacan a este objeto, pierde HP.
    void OnCollisionEnter(Collision other)
    {
        // aqu� se entra cuando hay colisi�n contra MI collider component que NO es trigger.

        Debug.Log("Entramos a OnCollisionEnter de DamageableObject.");

        // Si la colisi�n fue contra algo en la capa de EnemyCollider, este objeto va a perder HP.
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyCollider"))
        {
            // Ahorita significa que sufrimos da�o.
            HP -= 1.0f;
            // despu�s, como se redujo nuestra HP, checamos si este objeto sigue vivo.
            if (HP <= 0.0f)
            {
                // Destroy lo va a destruir, borrar todos sus componentes y borrarlo de la escena.
                // Destroy(gameObject); // aqu� le dices "destruye al due�o de este script".
                // desventajas o cosas de cuidado con Destroy.
                // Se rompen las referencias hacia este gameobject Y a todos los scripts y otros componentes que ten�a.

                // Si vas a reutilizar/respawnear, etc. ese objeto pronto otra vez, es mejor solo ocultarlo.
                gameObject.SetActive(false);

            }
        }

        // Lo primero que tenemos que hacer es qui�n o qu� est� colisionando con este objeto.
        if (other.gameObject.name == "Enemy")
        {
            // Ahorita significa que sufrimos da�o.
            HP -= 1.0f;
            // despu�s, como se redujo nuestra HP, checamos si este objeto sigue vivo.
            if (HP <= 0.0f)
            {
                // Destroy lo va a destruir, borrar todos sus componentes y borrarlo de la escena.
                // Destroy(gameObject); // aqu� le dices "destruye al due�o de este script".
                // desventajas o cosas de cuidado con Destroy.
                // Se rompen las referencias hacia este gameobject Y a todos los scripts y otros componentes que ten�a.

                // Si vas a reutilizar/respawnear, etc. ese objeto pronto otra vez, es mejor solo ocultarlo.
                gameObject.SetActive(false);

            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
