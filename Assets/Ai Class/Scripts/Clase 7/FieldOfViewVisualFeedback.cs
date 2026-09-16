using UnityEngine;

[RequireComponent(typeof(FieldOfViewSensor))]
public class FieldOfViewVisualFeedback : MonoBehaviour
{
    [SerializeField] private Renderer _bodyRenderer;
    [SerializeField] private Color _alertColor = Color.red;

    private FieldOfViewSensor _sensor;
    private Material _matInstance;
    private Color _defaultColor;

    private void Awake()
    {
        _sensor = GetComponent<FieldOfViewSensor>();

        if (_bodyRenderer == null) return;

        _matInstance = _bodyRenderer.material;
        _defaultColor = _matInstance.color;
    }

    private void Update()
    {
        if (_matInstance == null) return;

        _matInstance.color = _sensor.CanSeeTarget ? _alertColor : _defaultColor;
    }
}
