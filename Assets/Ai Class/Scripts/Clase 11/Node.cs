using System;
using TMPro;
using UnityEngine;

namespace Clase11
{
    public class Node : MonoBehaviour
    {
        public static event Action<Node> OnNodeClicked = delegate { };
        public static event Action<Node> OnNodeRightClicked = delegate { };

        [SerializeField] private Renderer _renderer;
        [SerializeField] private GameObject _wallGameObject;
        [SerializeField] private TextMeshPro _costText;

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

        public int Cost
        {
            get => _cost;
            private set
            {
                _cost = Mathf.Clamp(value, 1, 5);
                _costText.text = _cost.ToString();
                _costText.enabled = _cost > 1;
            }
        }
        private int _cost = 1;

        public void Setup(Coordinates coordinates)
        {
            Coordinates = coordinates;
            name = $"Node {coordinates}";
            Cost = 1;
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0)) OnNodeClicked?.Invoke(this);
            if (Input.GetMouseButtonDown(1)) OnNodeRightClicked?.Invoke(this);
            if (Input.GetMouseButtonDown(2)) Blocked = !Blocked;
            if (Input.GetKeyDown(KeyCode.S)) Cost = 1;
            if (Input.GetKeyDown(KeyCode.W)) Cost = 5;
        }

        public void ChangeColor(Color color) => _renderer.material.color = color;

        private void OnValidate()
        {
            if (_renderer != null) return;
            if (!TryGetComponent(out _renderer)) Debug.LogError("This gameobject doesn't have Renderer component");
        }
    }
}
