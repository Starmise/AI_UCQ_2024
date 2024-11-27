using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : BaseState
{

    public AlertState()
    {
        Name = "Alert State";
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        EnemyFSM enemyFSM = (EnemyFSM)(FSMRef);
        if (enemyFSM != null)
        {
            FSMRef.ChangeState(enemyFSM.PatrolState);
            // FSMRef.ChangeState((EnemyFSM)(FSMRef).AlertState); // esta linea es lo mismo pero sin el chequeo de seguridad.
            return; // siempre que hagan un llamado a cambio de estado, debe de ir seguido por un return,
                    // para no ejecutar solo al OnUpdate del estado que va de salida.
        }

        // Cuando sales de un estado X, ya no tienes por que hacer nada mas correspondiente a dicho estado X.

        Debug.Log("Nyejeje, nyejeje sigo en el update del estado de Alerta");


        // Gameplay Ability System (GAS)

        // ActivateAbility()

        // NotifyFinish // hay otras varias

        // EndAbility()

        // Si tu activas tu habilidad, y ya estaba activada no entra la nueva.
        // si estas activando tu habilidad, y la habilidad no se puede activar porque algo fallo, no hay recursos, etc.
        // entonces tu mandas el EndAbility de manera anticipada Y mandas a llamar un return, porque pues lo demas ya no aplicaria.

    }


}