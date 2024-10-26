using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    private LinkedList<Node> NodeList;

    // Queremos insertar/quitar elementos seg�n su prioridad, 
    // de manera que vayan quedando siempre ordenados.

    // Funci�n de meter elementos se debe de llamar "insertar"
    public void Insert(Node inNode, float Priority)
    {
        // Primero hay que checar que la NodeList no est� vac�a.
        if (NodeList.Count == 0)
        {
            // si s� est� vac�a metemos el elemento inNode y ya.
            NodeList.AddFirst(inNode);  // NOTA: Aqu� se usa AddFirst, no AddAfter.
            // Ahora nos salimos de la funci�n porque ya metimos el nodo.
            return;
        }

        // Debe de buscar la nodo actualmente dentro de la lista cuya prioridad sea m�s grande que la suya (Priority).
        // cuando encuentras un nodo con prioridad mayor, es porque este nodo inNode debe de ir antes de �l.
        LinkedListNode<Node> currentLinkedNode = NodeList.First;
        while (currentLinkedNode.Value.Priority < Priority)
        {
            // Si se sigue cumpliendo la condici�n del while, pasa al siguiente nodo de la lista.
            currentLinkedNode = currentLinkedNode.Next;
            if (currentLinkedNode == null)
            {
                // Aqu� ya se acabaron los elementos, entonces este nodo inNode debe ser el �ltimo de la NodeList.
                NodeList.AddLast(inNode);
                // Ahora nos salimos de la funci�n porque ya metimos el nodo.
                return;
            }
        }

        // Si s� se sale del while, quiere decir que encontramos d�nde debemos de insertar este nodo.
        // Si el currentLinkedNode tiene una prioridad mayor, entonces inNode debe de ir antes que �l

        // Le pedimos a la NodeList que lo meta antes de currentLinkedNode y listo.
        NodeList.AddBefore(currentLinkedNode, inNode);
    }

    // Ahora hacemos la funci�n de quitar del inicio de la Fila:
    public Node Dequeue()
    {
        // Debemos checar si est� vac�a antes de hacer cualquier cosa.
        if (NodeList.Count == 0)
        {
            Debug.LogWarning("Se llam� la funci�n Dequeue de nuestra PriorityQueue estando vac�a.");
            return null;
        }

        Node OutNode = NodeList.First.Value;

        // Ahora s� lo borramos de la NodeList para que ya no cuente en la fila.
        NodeList.RemoveFirst();

        // y lo regresamos para hacer lo que necesitemos con este nodo.
        return OutNode;
    }

    public bool Contains(Node inNode)
    {
        return NodeList.Contains(inNode);
    }

    public int Count()
    {
        return NodeList.Count;
    }
}
