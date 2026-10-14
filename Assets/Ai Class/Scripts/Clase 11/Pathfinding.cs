using System.Collections.Generic;
using UnityEngine;

namespace Clase11
{
    public static class Pathfinding
    {
        public enum HeuristicMethod { Distance, SQRMagnitude, Manhattan }

        public static float GetHeuristic(Coordinates a, Coordinates b, HeuristicMethod method)
        {
            int dx = Mathf.Abs(a.X - b.X);
            int dy = Mathf.Abs(a.Y - b.Y);

            return method switch
            {
                HeuristicMethod.Distance     => Mathf.Sqrt(dx * dx + dy * dy),
                HeuristicMethod.SQRMagnitude => dx * dx + dy * dy,
                HeuristicMethod.Manhattan    => dx + dy,
                _ => Mathf.Sqrt(dx * dx + dy * dy),
            };

        }

        public static List<Node> BreadthFirstSearch(Node startingNode, Node endNode, List<Node> visited = null)
        {
            if (startingNode == null || endNode == null)
            {
                Debug.LogError("Invalid node parameters");
                return new();
            }

            Queue<Node> frontier = new();
            frontier.Enqueue(startingNode);

            Dictionary<Node, Node> cameFrom = new();
            cameFrom.Add(startingNode, null);

            Node current;
            while (frontier.Count != 0)
            {
                current = frontier.Dequeue();
                visited?.Add(current);

                if (current == endNode) return ReconstructPath(cameFrom, current);

                foreach (Node neighbor in Graph._current.GetNeighbors(current))
                {
                    if (neighbor.Blocked) continue;
                    if (cameFrom.ContainsKey(neighbor)) continue;

                    frontier.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
            return new();
        }

        public static List<Node> Dijkstra(Node startingNode, Node endNode, List<Node> visited = null)
        {
            if (startingNode == null || endNode == null)
            {
                Debug.LogError("Invalid node parameters");
                return new();
            }

            PriorityQueue<Node> frontier = new();
            frontier.Enqueue(startingNode, 0);

            Dictionary<Node, Node> cameFrom = new();
            cameFrom.Add(startingNode, null);

            Dictionary<Node, int> costSoFar = new();
            costSoFar.Add(startingNode, 0);

            HashSet<Node> processed = new();

            Node current;
            while (frontier.Count != 0)
            {
                current = frontier.Dequeue();
                if (!processed.Add(current)) continue;
                visited?.Add(current);

                if (current == endNode) return ReconstructPath(cameFrom, current);

                foreach (Node neighbor in Graph._current.GetNeighbors(current))
                {
                    if (neighbor.Blocked) continue;

                    int newCost = costSoFar[current] + neighbor.Cost;
                    if (cameFrom.ContainsKey(neighbor) && newCost >= costSoFar[neighbor]) continue;

                    frontier.Enqueue(neighbor, newCost);
                    cameFrom[neighbor] = current;
                    costSoFar[neighbor] = newCost;
                    processed.Remove(neighbor);
                }
            }
            return new();
        }

        public static List<Node> GreedyBFS(Node startingNode, Node endNode, HeuristicMethod method = HeuristicMethod.Distance, List<Node> visited = null)
        {
            if (startingNode == null || endNode == null)
            {
                Debug.LogError("Invalid node parameters");
                return new();
            }

            PriorityQueue<Node> frontier = new();
            frontier.Enqueue(startingNode, 0);

            Dictionary<Node, Node> cameFrom = new();
            cameFrom.Add(startingNode, null);

            Node current;
            while (frontier.Count != 0)
            {
                current = frontier.Dequeue();
                visited?.Add(current);

                if (current == endNode) return ReconstructPath(cameFrom, current);

                foreach (Node neighbor in Graph._current.GetNeighbors(current))
                {
                    if (neighbor.Blocked) continue;
                    if (cameFrom.ContainsKey(neighbor)) continue;

                    frontier.Enqueue(neighbor, GetHeuristic(neighbor.Coordinates, endNode.Coordinates, method));
                    cameFrom[neighbor] = current;
                }
            }
            return new();
        }

        public static List<Node> AStar(Node startingNode, Node endNode,
                                       HeuristicMethod method = HeuristicMethod.Distance,
                                       List<Node> visited = null)
        {
            if (startingNode == null || endNode == null)
            {
                Debug.LogError("Invalid node parameters");
                return new();
            }

            PriorityQueue<Node> frontier = new();
            frontier.Enqueue(startingNode, 0);

            Dictionary<Node, Node> cameFrom = new();
            cameFrom.Add(startingNode, null);

            Dictionary<Node, int> costSoFar = new();
            costSoFar.Add(startingNode, 0);

            HashSet<Node> processed = new();

            Node current;
            while (frontier.Count != 0)
            {
                current = frontier.Dequeue();
                if (!processed.Add(current)) continue;
                visited?.Add(current);

                if (current == endNode) return ReconstructPath(cameFrom, current);

                foreach (Node neighbor in Graph._current.GetNeighbors(current))
                {
                    if (neighbor.Blocked) continue;

                    int newCost = costSoFar[current] + neighbor.Cost;
                    if (cameFrom.ContainsKey(neighbor) && newCost >= costSoFar[neighbor]) continue;

                    frontier.Enqueue(neighbor, newCost + GetHeuristic(neighbor.Coordinates, endNode.Coordinates, method));
                    cameFrom[neighbor] = current;
                    costSoFar[neighbor] = newCost;
                    processed.Remove(neighbor);
                }
            }
            return new();
        }

        private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
        {
            List<Node> path = new();
            while (current != null)
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Reverse();
            return path;
        }
    }
}
