using System;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public static event Action<GridNode> OnNodeClicked = delegate { };
    public static event Action<GridNode> OnNodeRightClicked = delegate { };

    [SerializeField] private Renderer _renderer;

    public Coordinates Coordinates { get; set; }

    public bool Blocked
    {
        get => _blocked;
        private set
        {
            _blocked = value;
            ResetColor();
        }
    }
    private bool _blocked;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
            Blocked = true;
    }

    public void ChangeColor(Color color) => _renderer.material.color = color;

    public void ResetColor() => ChangeColor(Blocked ? Color.red : Color.white);

    private void OnValidate()
    {
        if (_renderer != null) return;
        if (!TryGetComponent(out _renderer)) Debug.LogError("This gameobject doesn't have Renderer component");
    }
}
