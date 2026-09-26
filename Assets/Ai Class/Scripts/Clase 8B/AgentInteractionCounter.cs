using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AgentInteractor))]
public class AgentInteractionCounter : MonoBehaviour
{
    [SerializeField] private Vector3 _offset = new(0f, 2.2f, 0f);

    private int _count;
    private Text _label;

    private void Awake()
    {
        _label = WorldSpaceCounterLabel.Create(transform.position + _offset);
    }

    private void OnEnable()
    {
        GetComponent<AgentInteractor>().OnInteracted += HandleInteracted;
    }

    private void OnDisable()
    {
        GetComponent<AgentInteractor>().OnInteracted -= HandleInteracted;
    }

    private void Update()
    {
        _label.transform.parent.position = transform.position + _offset;
    }

    private void HandleInteracted(IInteractable item)
    {
        _count++;
        _label.text = _count.ToString();
    }
}
