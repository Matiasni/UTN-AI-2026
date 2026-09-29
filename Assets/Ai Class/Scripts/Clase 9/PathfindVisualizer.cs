using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clase9
{
    public class PathfindVisualizer : MonoBehaviour
    {
        [SerializeField] private Graph _graph;
        [SerializeField] private float _stepDelay = 0.5f;

        private Node _startNode;
        private Node _endNode;

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

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) ExecutePathfindVisualizer();
        }

        private void ExecutePathfindVisualizer()
        {
            if (_startNode == null) return;

            StopAllCoroutines();
            StartCoroutine(PaintBFS(_startNode));
        }

        #region Node Selection
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

        #region Search Algorithms
        // Version "seca" del algoritmo, sin visualizacion
        private void BreadthFirstSearch(Node startingNode)
        {
            // FIFO = First In First Out
            Queue<Node> frontier = new();
            frontier.Enqueue(startingNode);

            HashSet<Node> reached = new();
            reached.Add(startingNode);

            Node current;
            while (frontier.Count != 0)
            {
                current = frontier.Dequeue();

                foreach (Node neighbor in _graph.GetNeighbors(current))
                {
                    if (reached.Contains(neighbor)) continue;
                    frontier.Enqueue(neighbor);
                    reached.Add(neighbor);
                }
            }
        }

        // Comparacion: con un Stack (LIFO) en vez de una Queue obtenemos DFS
        //private void DepthFirstSearch(Node startingNode)
        //{
        //    // LIFO = Last In First Out
        //    Stack<Node> frontier = new();
        //    frontier.Push(startingNode);
        //
        //    HashSet<Node> reached = new();
        //    reached.Add(startingNode);
        //
        //    Node current;
        //    while (frontier.Count != 0)
        //    {
        //        current = frontier.Pop();
        //
        //        foreach (Node neighbor in _graph.GetNeighbors(current))
        //        {
        //            if (reached.Contains(neighbor)) continue;
        //            frontier.Push(neighbor);
        //            reached.Add(neighbor);
        //        }
        //    }
        //}

        private IEnumerator PaintBFS(Node startingNode)
        {
            Queue<Node> frontier = new();
            frontier.Enqueue(startingNode);

            HashSet<Node> reached = new();
            reached.Add(startingNode);

            Node current;
            while (frontier.Count != 0)
            {
                current = frontier.Dequeue();
                current.ChangeColor(Color.blue);
                yield return new WaitForSeconds(_stepDelay);

                foreach (Node neighbor in _graph.GetNeighbors(current))
                {
                    if (reached.Contains(neighbor)) continue;
                    frontier.Enqueue(neighbor);
                    reached.Add(neighbor);
                    neighbor.ChangeColor(Color.lightSalmon);
                }
                current.ChangeColor(Color.gray);
            }

            _startNode?.ChangeColor(Color.green);
            _endNode?.ChangeColor(Color.red);
        }
        #endregion

        private void OnValidate()
        {
            if (_graph == null) _graph = FindAnyObjectByType<Graph>();
        }
    }
}
