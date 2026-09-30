using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinder : MonoBehaviour
{
    public enum SearchAlgorithm { BFS, DFS }

    [SerializeField] private GridGraph _graph;
    [SerializeField] private SearchAlgorithm _algorithm = SearchAlgorithm.BFS;
    [SerializeField] private float _stepDelay = 0.1f;

    private GridNode _startNode;
    private GridNode _endNode;

    private void OnEnable()
    {
        GridNode.OnNodeClicked += NodeClicked;
        GridNode.OnNodeRightClicked += NodeRightClicked;
    }

    private void OnDisable()
    {
        GridNode.OnNodeClicked -= NodeClicked;
        GridNode.OnNodeRightClicked -= NodeRightClicked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) ExecuteSearch();
    }

    public void NodeClicked(GridNode node)
    {
        _startNode = HandleNodeClicked(node, _startNode);
        if (_startNode != null)
        {
            _startNode.ChangeColor(Color.green);
            if (_startNode == _endNode) _endNode = null;
        }
    }

    public void NodeRightClicked(GridNode node)
    {
        _endNode = HandleNodeClicked(node, _endNode);
        if (_endNode != null)
        {
            _endNode.ChangeColor(Color.magenta);
            if (_startNode == _endNode) _startNode = null;
        }
    }

    private GridNode HandleNodeClicked(GridNode node, GridNode current)
    {
        if (current != null)
        {
            current.ResetColor();
            if (current == node) return null;
        }
        return node;
    }

    private void ExecuteSearch()
    {
        if (_startNode == null || _endNode == null) return;

        List<GridNode> visited = _algorithm == SearchAlgorithm.BFS
            ? BreadthFirstSearch(_startNode, _endNode)
            : DepthFirstSearch(_startNode, _endNode);

        StopAllCoroutines();
        _graph.ResetColors();
        StartCoroutine(PaintSearch(visited));
    }

    private List<GridNode> BreadthFirstSearch(GridNode startingNode, GridNode goalNode)
    {
        Queue<GridNode> frontier = new();
        frontier.Enqueue(startingNode);

        HashSet<GridNode> reached = new();
        reached.Add(startingNode);

        List<GridNode> visited = new();

        var asd = visited.Where(x => x.transform.position == Vector3.zero);        

        visited.First(x => x.Blocked);

        GridNode current;
        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();
            visited.Add(current);
            if (current == goalNode) break;

            foreach (GridNode neighbor in _graph.GetNeighbors(current))
            {
                if (reached.Contains(neighbor)) continue;
                frontier.Enqueue(neighbor);
                reached.Add(neighbor);
            }
        }
        return visited;
    }

    private List<GridNode> DepthFirstSearch(GridNode startingNode, GridNode goalNode)
    {
        Stack<GridNode> frontier = new();
        frontier.Push(startingNode);

        HashSet<GridNode> reached = new();
        reached.Add(startingNode);

        List<GridNode> visited = new();

        GridNode current;
        while (frontier.Count != 0)
        {
            current = frontier.Pop();
            visited.Add(current);
            if (current == goalNode) break;

            foreach (GridNode neighbor in _graph.GetNeighbors(current))
            {
                if (reached.Contains(neighbor)) continue;
                frontier.Push(neighbor);
                reached.Add(neighbor);
            }
        }
        return visited;
    }

    private IEnumerator PaintSearch(List<GridNode> visited)
    {
        PaintStartAndEnd();

        foreach (GridNode node in visited)
        {
            if (node == _startNode || node == _endNode) continue;

            node.ChangeColor(Color.blue);
            yield return new WaitForSeconds(_stepDelay);
            node.ChangeColor(Color.gray);
        }
    }

    private void PaintStartAndEnd()
    {
        _startNode?.ChangeColor(Color.green);
        _endNode?.ChangeColor(Color.magenta);
    }

    private void OnValidate()
    {
        if (_graph == null) _graph = FindAnyObjectByType<GridGraph>();
    }
}
