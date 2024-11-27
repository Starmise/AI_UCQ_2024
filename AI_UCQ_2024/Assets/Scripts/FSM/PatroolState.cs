using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatroolState : BaseState
{
    public PatroolState()
    {
        Name = "Patrol State";
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // Oye, maquina de estados que es dueña de este script, cambia hacia el estado de Alerta.
        // ESTO NO ME DEJA PORQUE LA FSMRef NO ES DE TIPO EnemyFSM que es la que tiene el AlertState
        // FSMRef.ChangeState( FSMRef.AlertState ); 

        EnemyFSM enemyFSM = (EnemyFSM)(FSMRef);
        if (enemyFSM != null)
        {
            FSMRef.ChangeState(enemyFSM.AlertState);
        }
    }

}