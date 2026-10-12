using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clase10
{
    public class PathfindVisualizer : MonoBehaviour
    {
        private enum PathfindingMethod { BFS, DFS, GenericBFS }

        [SerializeField] private Graph _graph;
        [SerializeField] private PathfindingMethod _currentPathfindingMethod;
        [SerializeField] private float _stepDelay = 0.1f;
        [SerializeField] private float _pathStepDelay = 0.01f;

        private Node _startNode;
        private Node _endNode;
        private Coroutine _paintRoutine;

        private void OnEnable()
        {
            Node.OnNodeClicked += NodeClicked;
            Node.OnNodeRightClicked += NodeRightClicked;
        }

        private void OnDisable()
        {
            Node.OnNodeClicked -= NodeClicked;
            Node.OnNodeRightClicked -= NodeRightClicked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) ExecutePathfindVisualizer();
        }

        #region Selección de nodos

        public void NodeClicked(Node node)
        {
            _startNode = HandleNodeClicked(node, _startNode);
            if (_startNode != null)
            {
                _startNode.ChangeColor(Color.green);
                if (_startNode == _endNode) _endNode = null;
            }
        }

        public void NodeRightClicked(Node node)
        {
            _endNode = HandleNodeClicked(node, _endNode);
            if (_endNode != null)
            {
                _endNode.ChangeColor(Color.red);
                if (_startNode == _endNode) _startNode = null;
            }
        }

        private Node HandleNodeClicked(Node node, Node current)
        {
            if (current != null)
            {
                current.ChangeColor(Color.white);
                if (current == node) return null;
            }
            return node;
        }

        #endregion

        private void ExecutePathfindVisualizer()
        {
            if (_startNode == null) return;

            List<Node> visited = new();
            List<Node> path = _currentPathfindingMethod switch
            {
                PathfindingMethod.BFS => BreadthFirstSearch(_startNode, _endNode, out visited),
                PathfindingMethod.DFS => DepthFirstSearch(_startNode, _endNode, out visited),
                PathfindingMethod.GenericBFS => Pathfinding<Node>.BreadthFirstSearch(_startNode, _endNode,
                    current  => _graph.GetNeighbors(current),
                    neighbor => !neighbor.Blocked),
                _ => new()
            };

            foreach (var item in _graph.GetAllGridNodes)
            {
                if (item == _startNode || item == _endNode) continue;
                item.ChangeColor(Color.white);
            }

            if (_paintRoutine != null) StopCoroutine(_paintRoutine);
            _paintRoutine = StartCoroutine(PaintSearch(visited, path));
        }

        private List<Node> BreadthFirstSearch(Node startingNode, Node endNode, out List<Node> visited)
        {
            visited = new();

            Queue<Node> frontier = new();
            frontier.Enqueue(startingNode);

            Dictionary<Node, Node> cameFrom = new();
            cameFrom.Add(startingNode, null);

            Node current;
            while (frontier.Count != 0)
            {
                current = frontier.Dequeue();
                visited.Add(current);

                if (current == endNode) return ReconstructPath(cameFrom, current);

                foreach (Node neighbor in _graph.GetNeighbors(current))
                {
                    if (neighbor.Blocked) continue;
                    if (cameFrom.ContainsKey(neighbor)) continue;
                    frontier.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
            return new();
        }

        private List<Node> DepthFirstSearch(Node startingNode, Node endNode, out List<Node> visited)
        {
            visited = new();

            Stack<Node> frontier = new();
            frontier.Push(startingNode);

            Dictionary<Node, Node> cameFrom = new();
            cameFrom.Add(startingNode, null);

            Node current;
            while (frontier.Count != 0)
            {
                current = frontier.Pop();
                visited.Add(current);

                if (current == endNode) return ReconstructPath(cameFrom, current);

                foreach (Node neighbor in _graph.GetNeighbors(current))
                {
                    if (neighbor.Blocked) continue;
                    if (cameFrom.ContainsKey(neighbor)) continue;
                    frontier.Push(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
            return new();
        }

        private List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
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

        private IEnumerator PaintSearch(List<Node> visited, List<Node> path)
        {
            PaintStartAndEnd();

            foreach (Node node in visited)
            {
                if (node == _startNode || node == _endNode) continue;

                node.ChangeColor(Color.blue);
                yield return new WaitForSeconds(_stepDelay);
                node.ChangeColor(Color.gray);
            }

            for (int i = 0; i < path.Count; i++)
            {
                path[i].ChangeColor(Color.Lerp(Color.green, Color.red, (float)i / path.Count));
                yield return new WaitForSeconds(_pathStepDelay);
            }

            PaintStartAndEnd();
        }

        private void PaintStartAndEnd()
        {
            if (_startNode != null) _startNode.ChangeColor(Color.green);
            if (_endNode != null) _endNode.ChangeColor(Color.red);
        }

        private void OnValidate()
        {
            if (_graph == null) _graph = FindAnyObjectByType<Graph>();
        }
    }
}
