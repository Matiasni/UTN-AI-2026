using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    public static event Action<Node> OnNodeClicked = delegate { };
    public static event Action<Node> OnNodeRightClicked = delegate { };

    [SerializeField] private Renderer _renderer;
    [SerializeField] private GameObject _wallGameObject;

    public bool Blocked
    {
        get => _blocked;
        private set
        {
            _blocked = value;
            ChangeColor(Color.red);
            //_wallGameObject.SetActive(value);
        }
    }
    private bool _blocked;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            OnNodeClicked?.Invoke(this);
        else if (Input.GetMouseButtonDown(1))
            OnNodeRightClicked?.Invoke(this);
        else if (Input.GetMouseButtonDown(2))
            Blocked = !Blocked;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
            Blocked = true;
    }

    public void ChangeColor(Color color) => _renderer.material.color = color;

    private void OnValidate()
    {
        if (_renderer != null) return;
        if (!TryGetComponent(out _renderer)) Debug.LogError("This gameobject doesn't have Renderer component");
    }
}