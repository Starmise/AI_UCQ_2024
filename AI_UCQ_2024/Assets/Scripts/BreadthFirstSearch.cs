using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : MonoBehaviour
{
    // Método para ejecutar el BFS
    public bool BFS(Node start, Node goal, out List<Node> path)
    {
        path = new List<Node>();
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        queue.Enqueue(start);
        visited.Add(start);
        start.Parent = start; // Asigna el padre para el nodo de inicio

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            Debug.Log("Visitando nodo: " + current.Name);

            if (current == goal)
            {
                path = Backtrack(current);
                return true; // Se encontró el camino
            }

            // Obtener vecinos usando EdgeSet en lugar de Edges en Node
            List<Node> neighbors = GetNeighbors(current);

            foreach (Node neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    neighbor.Parent = current; // Establece el padre
                    queue.Enqueue(neighbor);
                }
            }
        }

        return false; // No se encontró camino
    }

    // Método para realizar el retroceso y obtener el camino
    private List<Node> Backtrack(Node goal)
    {
        List<Node> path = new List<Node>();
        Node current = goal;

        while (current != current.Parent)
        {
            path.Add(current);
            current = current.Parent;
        }
        path.Add(current); // Agrega el nodo de inicio

        path.Reverse(); // Invertir el camino para que esté en el orden correcto
        return path;
    }

    // Método para obtener vecinos de un nodo
    private List<Node> GetNeighbors(Node inNode)
    {
        List<Node> neighbors = new List<Node>();

        foreach (Edge edge in FindObjectOfType<Graph>().EdgeSet)
        {
            if (edge.A == inNode)
            {
                neighbors.Add(edge.B);
            }
            else if (edge.B == inNode)
            {
                neighbors.Add(edge.A);
            }
        }

        return neighbors;
    }
}
