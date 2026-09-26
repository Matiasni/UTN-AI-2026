using System.Collections;
using UnityEngine;

public class AgentFeedback : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _destinationColor = Color.yellow;
    [SerializeField] private Color _interactColor = Color.cyan;
    [SerializeField] private float _flashDuration = 0.3f;

    private Color _baseColor;
    private Coroutine _flashRoutine;

    private void Awake()
    {
        _baseColor = _renderer.material.color;
        SetComponents();
    }

    private void SetComponents()
    {
        if (TryGetComponent(out IDestinationNotifier mover))
            mover.OnDestinationReached += () => Flash(_destinationColor);

        if (TryGetComponent(out AgentInteractor interactor))
            interactor.OnInteracted += x => Flash(_interactColor);
    }

    private void Flash(Color color)
    {
        if (_flashRoutine != null) StopCoroutine(_flashRoutine);
        _flashRoutine = StartCoroutine(FlashRoutine(color));
    }

    private IEnumerator FlashRoutine(Color color)
    {
        _renderer.material.color = color;
        yield return new WaitForSeconds(_flashDuration);
        _renderer.material.color = _baseColor;
    }

    private void OnValidate()
    {
        if (_renderer != null) return;
        TryGetComponent(out _renderer);
    }
}