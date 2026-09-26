using System;
using UnityEngine;

public class MovementNode : MonoBehaviour
{
    public static event Action<MovementNode> OnNodeSelected = delegate { };

    private void OnMouseDown()
    {
        OnNodeSelected?.Invoke(this);
    }
}
