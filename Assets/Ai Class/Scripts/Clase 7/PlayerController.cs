using UnityEngine;

public class PlayerController : Agent
{
    [SerializeField] private float _speed = 5f;

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        _velocity = input.normalized * _speed;

        transform.position += _velocity * Time.deltaTime;
        transform.position = Bounds.Instance.OutOfBounds(transform.position);

        if (_velocity != Vector3.zero)
            transform.forward = _velocity;
    }
}
