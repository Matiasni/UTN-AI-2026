using UnityEngine;

public class AgentAudioFeedback : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _interactSound;

    private void OnEnable()
    {
        if (TryGetComponent(out AgentInteractor interactor))
            interactor.OnInteracted += HandleInteracted;
    }

    private void OnDisable()
    {
        if (TryGetComponent(out AgentInteractor interactor))
            interactor.OnInteracted -= HandleInteracted;
    }

    private void HandleInteracted(IInteractable item)
    {
        _audioSource.PlayOneShot(_interactSound);
    }
}
