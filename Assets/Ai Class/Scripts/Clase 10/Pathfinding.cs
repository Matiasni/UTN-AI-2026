using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clase10
{
    public static class Pathfinding<T>
    {
        public static List<T> BreadthFirstSearch(T startingNode, T endNode,
                                                 Func<T, List<T>> getNeighbors,
                                                 Func<T, bool> isTransitable)
        {
            if (startingNode == null || endNode == null)
            {
                Debug.LogError("Invalid node parameters");
                return new();
            }

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
                    path.Add(startingNode);
                    path.Reverse();
                    return path;
                }

                foreach (T neighbor in getNeighbors(current))
                {
                    if (!isTransitable(neighbor)) continue;
                    if (cameFrom.ContainsKey(neighbor)) continue;
                    frontier.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
            return new();
        }
    }
}
