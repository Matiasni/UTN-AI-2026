using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clase11
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;

        private List<Node> _path;
        private Coroutine _walkRoutine;

        public void Reposition(Node node)
        {
            if (_walkRoutine != null) StopCoroutine(_walkRoutine);
            transform.position = node.transform.position + Vector3.up;
        }

        public void CalculatePath(Node startNode, Node endNode)
        {
            _path = Pathfinding.AStar(startNode, endNode);
            if (_path != null && _path.Count > 0)
            {
                if (_walkRoutine != null) StopCoroutine(_walkRoutine);
                _walkRoutine = StartCoroutine(WalkPath(_path));
            }
        }

        private IEnumerator WalkPath(List<Node> path)
        {
            foreach (Node node in path)
            {
                Vector3 target = node.transform.position + Vector3.up;
                while ((transform.position - target).sqrMagnitude > 0.001f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }
}
