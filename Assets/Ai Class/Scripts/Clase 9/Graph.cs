using System.Collections.Generic;
using UnityEngine;

namespace Clase9
{
    public class Graph : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private int _gridWidth = 10;
        [SerializeField] private int _gridHeight = 10;
        [SerializeField] private float _nodeOffset = 1f;
        [SerializeField] private Node _nodePrefab;

        private Node[,] _grid;

        private void Start()
        {
            GenerateGrid();
        }

        public void GenerateGrid()
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
            Coordinates coordinates = current.Coordinates;
            var directions = coordinates.GetAllAdyacentDirections;

            foreach (Coordinates coord in directions)
            {
                // Chequeo de limites: evita IndexOutOfRangeException en los bordes del grid
                if (!IsInsideGrid(coord)) continue;

                Node neighbor = _grid[coord.X, coord.Y];
                // Los nodos bloqueados (paredes) no son transitables
                if (neighbor.Blocked) continue;

                neighbors.Add(neighbor);
            }
            return neighbors;
        }

        private bool IsInsideGrid(Coordinates coord) =>
            coord.X >= 0 && coord.X < _gridWidth &&
            coord.Y >= 0 && coord.Y < _gridHeight;
    }
}
