using UnityEngine;

public class PathfindVisualizer : MonoBehaviour
{
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
}
