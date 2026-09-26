using UnityEngine;

public class Graph : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private float _nodeOffset = 1f;
    [SerializeField] private GameObject _nodePrefab;

    private GameObject[,] _grid;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        _grid = new GameObject[_gridWidth, _gridHeight];

        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                GameObject node = Instantiate(_nodePrefab, transform);
                _grid[x, y] = node;
                node.transform.position = new(x * _nodeOffset, 0f, y * _nodeOffset);
                node.name = $"Node ({x},{y})";
            }
        }
    }
}
