using System.Collections.Generic;
using UnityEngine;

public class GridGraph : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private float _nodeOffset = 1f;
    [SerializeField] private GridNode _nodePrefab;

    private GridNode[,] _grid;

    private void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        _grid = new GridNode[_gridWidth, _gridHeight];

        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                _grid[x, y] = Instantiate(_nodePrefab, transform);
                _grid[x, y].transform.position = new(x * _nodeOffset, 0f, y * _nodeOffset);
                _grid[x, y].Setup(new(x, y));
            }
        }
    }

    public List<GridNode> GetNeighbors(GridNode current)
    {
        List<GridNode> neighbors = new();
        Coordinates coordinates = current.Coordinates;
        var directions = coordinates.GetAllAdyacentDirections;

        foreach (Coordinates coord in directions)
        {
            if (!IsInsideGrid(coord)) continue;

            GridNode neighbor = _grid[coord.X, coord.Y];
            if (neighbor.Blocked) continue;

            neighbors.Add(neighbor);
        }
        return neighbors;
    }

    public void ResetColors()
    {
        foreach (GridNode node in _grid)
            node.ResetColor();
    }

    private bool IsInsideGrid(Coordinates coord) =>
        coord.X >= 0 && coord.X < _gridWidth &&
        coord.Y >= 0 && coord.Y < _gridHeight;
}
