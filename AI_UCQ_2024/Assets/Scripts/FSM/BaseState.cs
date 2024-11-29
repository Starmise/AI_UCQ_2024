using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// No queremos de MonoBehaviour porque queremos mas bien asignarle lo estados a la FSM.
// PERO, como queremos que nuestros estados puedan usar corrutinas y otras cosas utiles que 
// vienen de la classe MonoBehaviour, si vamos a dejar que hereden de ella.

public class BaseState : MonoBehaviour
{
    public string Name = "BaseState";
    // Necesita tener una referencia o forma de contactar a la maquina de estados que es su dueña.
    public BaseFSM FSMRef;

    public BaseState()
    {
        Name = "BaseState";
    }

    public BaseState(string inName, BaseFSM inBaseFSM)
    {
        Name = inName;
        FSMRef = inBaseFSM;
    }

    public virtual void InitializeState(BaseFSM inBaseFSM)
    {
        FSMRef = inBaseFSM;
    }

    // Enter se llama cuando la FSM asigna a este estado como el estado actual.
    public virtual void OnEnter()
    {
        Debug.Log("OnEnter del estado: " + Name);
    }

    // Update se va a llamar cada frame, pero solo cuando sea el estado actual de la FSM.
    public virtual void OnUpdate()
    {
        //Debug.Log("OnUpdate del estado: " + Name);
    }

    // Exit se llama cuando la FSM remueve a este estado de ser el estado actual.
    public virtual void OnExit()
    {
        Debug.Log("OnExit del estado: " + Name);
    }
}