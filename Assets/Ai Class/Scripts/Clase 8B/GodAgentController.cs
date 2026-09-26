using UnityEngine;
using UnityEngine.UI;

//MAL
public class GodAgentController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _areaRadius = 8f;
    [SerializeField] private float _arrivalDistance = 0.2f;

    [Header("Interaction")]
    [SerializeField] private float _interactRadius = 1.2f;
    [SerializeField] private LayerMask _interactableMask = ~0;

    [Header("Feedback")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _baseColor = Color.red;
    [SerializeField] private Color _flashColor = Color.cyan;
    [SerializeField] private float _flashDuration = 0.3f;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _interactSound;

    private Vector3 _areaCenter;
    private Vector3 _destination;
    private float _flashTimer;
    private int _interactionCount;
    private Text _counterLabel;

    private void Start()
    {
        _areaCenter = transform.position;
        _destination = _areaCenter;
        _renderer.material.color = _baseColor;
        _counterLabel = WorldSpaceCounterLabel.Create(transform.position + new Vector3(0f, 2.2f, 0f));
        PickNewDestination();
    }

    private void Update()
    {        
        Collider[] hits = Physics.OverlapSphere(transform.position, _interactRadius, _interactableMask);
        if (hits.Length > 0)
        {
            _renderer.material.color = _flashColor;
            _flashTimer = _flashDuration;
            _audioSource.PlayOneShot(_interactSound);

            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent(out InteractableItem item))
                {
                    item.Interact();
                    _interactionCount++;
                    _counterLabel.text = _interactionCount.ToString();
                }
            }
        }

        Vector3 direction = _destination - transform.position;
        direction.y = 0f;

        _counterLabel.transform.parent.position = transform.position + new Vector3(0f, 2.2f, 0f);

        if (direction.magnitude <= _arrivalDistance)
        {
            PickNewDestination();
        }
        else
        {
            transform.position += direction.normalized * _speed * Time.deltaTime;
            transform.forward = direction.normalized;
        }

        if (_flashTimer > 0f)
        {
            _flashTimer -= Time.deltaTime;
            if (_flashTimer <= 0f) _renderer.material.color = _baseColor;
        }
    }

    private void PickNewDestination()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _areaRadius;
        _destination = _areaCenter + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }
}
