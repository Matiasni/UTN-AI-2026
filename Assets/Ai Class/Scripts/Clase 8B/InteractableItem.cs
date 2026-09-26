using System;
using System.Collections;
using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable
{
    public event Action OnInteracted = delegate { };

    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _pulseColor = Color.white;
    [SerializeField] private float _pulseDuration = 0.4f;

    private Color _baseColor;

    private void Awake()
    {
        _baseColor = _renderer.material.color;
    }

    public void Interact()
    {
        OnInteracted?.Invoke();
        StopAllCoroutines();
        StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        _renderer.material.color = _pulseColor;
        yield return new WaitForSeconds(_pulseDuration);
        _renderer.material.color = _baseColor;
    }

    private void OnValidate()
    {
        if (_renderer != null) return;
        TryGetComponent(out _renderer);
    }
}