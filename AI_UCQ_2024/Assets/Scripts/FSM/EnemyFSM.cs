using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : BaseFSM
{
    private PatroolState _patrolState;
    private AlertState _alertState;
    private MeleeState _meleeState;

    public MeleeState MeleeState
    { get { return _meleeState; } }

    public PatroolState PatrolState
    { get { return _patrolState; } }

    public AlertState AlertState
    { get { return _alertState; } }

    public BaseEnemy Owner;


    public override void Start()
    {
        _meleeState = gameObject.AddComponent<MeleeState>();
        _meleeState.FSMRef = this;

        _patrolState = gameObject.AddComponent<PatroolState>();
        _patrolState.FSMRef = this;

        _alertState = gameObject.AddComponent<AlertState>();
        _alertState.FSMRef = this;

        base.Start(); // Esto es mandar a llamar funcion del padre.
    }

    public override BaseState GetInitialState()
    {
        return _meleeState; // ESTO NO DEBE SER NULL.
    }

}