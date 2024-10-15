using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Node
{
    public Node(string inName)
    {
        Name = inName;
        Parent = null;
    }
    public string Name = "";  // es string por pura claridad, idealmente se usan ints para diferenciar objetos.
    public Node Parent;  // referencia al nodo padre de este nodo en el �rbol que se genera durante un Pathfinding.
}

public class Edge
{
    public Edge(string inName, Node inA, Node inB, float inWeight = 1.0f)
    {
        Name = inName;
        A = inA;
        B = inB;
        Weight = inWeight;
    }
    public string Name = ""; // es string por pura claridad, las aristas normalmente no necesitan un nombre.
    public Node A;
    public Node B;
    public float Weight = 1.0f;
    // EdgeA == EdgeB
    // Si son punteros/referencias pues nom�s comparan la direcci�n de memoria y ya.
    // PERO SI NO, ustedes tendr�an que comparar una o m�s cosas.
    // Por ejemplo podr�amos checar EdgeA.A == EdgeB.A && EdgeA.B == EdgeB.B && EdgeA.Weight == EdgeB.Weight
    // Un hash te da un solo n�mero que representa a ese objeto.
    // Vector3 A == Vector3 B?
    // A.x == B.x && A.y == B.y && A.z == B.z
}

public class Graph : MonoBehaviour
{
    // Podr�amos guardarlos en un array.
    // Podr�amos guardarlos en un List, Set
    // Dictionary, Queue, Stack, DynamicArray, Heap
    // Array:
    // Ventajas: super r�pido de acceder de manera secuencial. Te da el espacio de memoria completo.
    // int [10]Array
    // Desventajas: Te da el espacio de memoria completo (lo vayas a usar o no, lo que puede llevar a desperdicios).
    // desventajas: su tama�o (capacidad de almacenamiento) es totalmente est�tico.
    // �Qu� es un "Set" en estructuras de datos / programaci�n?
    // Un set es una estructura de datos que no permite repetidos
    // espec�ficamente en nuestros grafos, no vamos a querer ni nodos ni aristas repetidas.
    protected HashSet<Node> NodeSet = new HashSet<Node>();
    protected HashSet<Edge> EdgeSet = new HashSet<Edge>();
    // Start is called before the first frame update
    void Start()
    {
        // Vamos a llenar nuestros sets de nodos y aristas.
        // Comenzamos creando todos los nodos, porque las aristas necesitan que ya existan los nodos.
        Node NodeA = new Node("A");
        Node NodeB = new Node("B");
        Node NodeC = new Node("C");
        Node NodeD = new Node("D");
        Node NodeE = new Node("E");
        Node NodeF = new Node("F");
        Node NodeG = new Node("G");
        Node NodeH = new Node("H");

        NodeSet.Add(NodeA);
        NodeSet.Add(NodeB);
        NodeSet.Add(NodeC);
        NodeSet.Add(NodeD);
        NodeSet.Add(NodeE);
        NodeSet.Add(NodeF);
        NodeSet.Add(NodeG);
        NodeSet.Add(NodeH);

        // Ahora queremos declarar las aristas.
        Edge EdgeAB = new Edge("AB", NodeA, NodeB);
        Edge EdgeAE = new Edge("AE", NodeA, NodeE);
        Edge EdgeBC = new Edge("BC", NodeB, NodeC);
        Edge EdgeBD = new Edge("BD", NodeB, NodeD);
        Edge EdgeEF = new Edge("EF", NodeE, NodeF);
        Edge EdgeEG = new Edge("EG", NodeE, NodeG);
        Edge EdgeEH = new Edge("EH", NodeE, NodeH);

        EdgeSet.Add(EdgeAB);
        EdgeSet.Add(EdgeAE);
        EdgeSet.Add(EdgeBC);
        EdgeSet.Add(EdgeBD);
        EdgeSet.Add(EdgeEF);
        EdgeSet.Add(EdgeEG);
        EdgeSet.Add(EdgeEH);

        if (RecursiveDFS(NodeA, NodeH))
        {
            Debug.Log("Hay camino del nodo: " + NodeA.Name + " hacia el nodo: " + NodeH.Name);
        }
        else
        {
            Debug.Log("NO hay camino del nodo: " + NodeA.Name + " hacia el nodo: " + NodeH.Name);
        }

        // FuncionRecursiva(0);  // comentada para que no truene ahorita.
    }

    bool RecursiveDFS(Node Origin, Node Goal)
    {
        // Hacemos que el nodo raiz de arbol sea su propio padre
        if (Origin.Parent == null)
        {
            Origin.Parent = Origin;
        }

        // La condici�n de terminaci�n de la funci�n es que el nodo actual sea la meta
        if (Origin == Goal)
        {
            return true;
        }

        // Revisar cuales son los nodos vecinos y comprobar todas las artistas
        foreach (Edge curreentEdge in EdgeSet)
        {
            bool Result = false;
            Node Neighbor = null;

            // Checar si la arista hace referencia al nodo Origin
            if (curreentEdge.A == Origin && curreentEdge.B.Parent == null)
            {
                // Si es as�, se mete al nodo vecino, si hay vecino se usa el nodo
                // vecino actual como el nuevo nodo origen
                curreentEdge.B.Parent = Origin;
                Neighbor = curreentEdge.B;
            }
            else if (curreentEdge.B == Origin && curreentEdge.A.Parent == null)
            {
                curreentEdge.A.Parent = Origin;
                Neighbor = curreentEdge.A;
            }

            // Comprobar si se accede al if else de arriba
            if (Neighbor != null)
            {
                Result = RecursiveDFS(Neighbor, Goal);
            }

            if (Result == true)
            {
                // Si el nodo fue parte del camino a la meta (Goal), imprime su nombre
                Debug.Log("El nodo: " + Origin.Name + " fue parte del camino");

                return true;
            }
        }
        return false;
    }

    // Funciones recursivas VS funciones iterativas.
    // las funciones recursivas son funciones que se mandan a llamar a s� mismas.
    void FuncionRecursiva(int Counter)
    {
        Debug.Log("Hola n�mero: " + Counter);
        FuncionRecursiva(Counter + 1);
    }
}