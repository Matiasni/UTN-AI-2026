using UnityEngine;
using System;
using System.Collections.Generic;

public static class GenericPath<T>
{
    private static List<T> BreadthFirstSearch(T startingNode, T endNode, Func<T,bool> isTransitable, Func<T, List<T>> getNeighbors)
    {
        if (startingNode == null || endNode == null) return new();

        Queue<T> frontier = new();
        frontier.Enqueue(startingNode);

        Dictionary<T, T> cameFrom = new();
        cameFrom.Add(startingNode, default);

        T current;
        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current.Equals(endNode))
            {
                List<T> path = new();
                while (!current.Equals(startingNode))
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse();
                return path;
            }

            foreach (T neighbor in getNeighbors(current))
            {
                if (isTransitable(neighbor)) continue;
                if (cameFrom.ContainsKey(neighbor)) continue;
                frontier.Enqueue(neighbor);
                cameFrom[neighbor] = current;
            }
        }
        return new();
    }
}