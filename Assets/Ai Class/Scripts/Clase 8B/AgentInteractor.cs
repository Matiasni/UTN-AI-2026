using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AgentInteractor : MonoBehaviour
{
    public event Action<IInteractable> OnInteracted = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable item)) return;

        item.Interact();
        OnInteracted?.Invoke(item);
    }
}