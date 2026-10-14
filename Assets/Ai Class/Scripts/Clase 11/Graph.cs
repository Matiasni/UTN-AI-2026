using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Clase11
{
    public class Graph : MonoBehaviour
    {
        public static Graph _current;

        [Header("Grid setup")]
        [SerializeField] private int _gridWidth = 10;
        [SerializeField] private int _gridHeight = 10;
        [SerializeField] private float _nodeOffset = 1f;
        [SerializeField] private bool _allowDiagonals;

        [Header("References")]
        [SerializeField] private Node _nodePrefab;

        private Node[,] _grid;

        public List<Node> GetAllGridNodes => _grid.Cast<Node>().ToList();

        private void Awake()
        {
            _current = this;
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            _grid = new Node[_gridWidth, _gridHeight];

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

        public List<Node> GetNeighbors(Node current)
        {
            List<Node> neighbors = new();

            foreach (Coordinates coord in current.Coordinates.GetAllAdyacentDirections)
            {
                if (!IsInsideGrid(coord)) continue;
                neighbors.Add(_grid[coord.X, coord.Y]);
            }

            if (!_allowDiagonals) return neighbors;

            foreach (Coordinates coord in current.Coordinates.GetAllDiagonalDirections)
            {
                if (!IsInsideGrid(coord)) continue;
                neighbors.Add(_grid[coord.X, coord.Y]);
            }
            return neighbors;
        }

        private bool IsInsideGrid(Coordinates coord) =>
            coord.X >= 0 && coord.X < _gridWidth &&
            coord.Y >= 0 && coord.Y < _gridHeight;
    }
}
