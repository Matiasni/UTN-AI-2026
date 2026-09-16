using UnityEngine;

[RequireComponent(typeof(FieldOfViewSensor))]
public class FieldOfViewGizmos : MonoBehaviour
{
    private FieldOfViewSensor _sensor;

    private Vector3 DirFromAngle(float angleDegrees)
    {
        float angleRad = (transform.eulerAngles.y + angleDegrees) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(angleRad), 0f, Mathf.Cos(angleRad));
    }

    private void OnDrawGizmosSelected()
    {
        if (_sensor == null) _sensor = GetComponent<FieldOfViewSensor>();
        if (_sensor == null) return;

        Gizmos.color = _sensor.CanSeeTarget ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sensor.ViewRadius);

        Vector3 left = DirFromAngle(-_sensor.ViewAngle * 0.5f);
        Vector3 right = DirFromAngle(_sensor.ViewAngle * 0.5f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + left * _sensor.ViewRadius);
        Gizmos.DrawLine(transform.position, transform.position + right * _sensor.ViewRadius);

        if (_sensor.Target != null)
        {
            Gizmos.color = _sensor.CanSeeTarget ? Color.green : Color.gray;
            Gizmos.DrawLine(transform.position + Vector3.up * _sensor.EyeHeight, _sensor.Target.position + Vector3.up * _sensor.EyeHeight);
        }
    }
}
