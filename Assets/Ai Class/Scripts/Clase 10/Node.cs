using System;
using UnityEngine;

namespace Clase10
{
    public class Node : MonoBehaviour
    {
        public static event Action<Node> OnNodeClicked = delegate { };
        public static event Action<Node> OnNodeRightClicked = delegate { };

        [SerializeField] private Renderer _renderer;
        [SerializeField] private GameObject _wallGameObject;

        public Coordinates Coordinates { get; private set; }

        public bool Blocked
        {
            get => _blocked;
            private set
            {
                _blocked = value;
                _wallGameObject.SetActive(value);
            }
        }
        private bool _blocked = false;

        public void Setup(Coordinates coordinates)
        {
            Coordinates = coordinates;
            name = $"Node {coordinates}";
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0)) OnNodeClicked?.Invoke(this);      
            if (Input.GetMouseButtonDown(1)) OnNodeRightClicked?.Invoke(this); 
            if (Input.GetMouseButtonDown(2)) Blocked = !Blocked;
        }

        public void ChangeColor(Color color) => _renderer.material.color = color;

        private void OnValidate()
        {
            if (_renderer != null) return;
            if (!TryGetComponent(out _renderer)) Debug.LogError("This gameobject doesn't have Renderer component");
        }
    }
}
