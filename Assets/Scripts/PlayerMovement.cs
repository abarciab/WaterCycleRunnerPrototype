using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _forwardSpeed = 30;
    [SerializeField] private float _jumpForce = 100;
    [SerializeField] private float _sidewaysDist;

    private Rigidbody _rb;
    private int _currentLane;
    bool _isMoving = true;
    private bool OnGround => transform.position.y <= 0.01f;

    public void SetIsMoving(bool state ) => _isMoving = state;
    public void SetLane(int lane ) => _currentLane = lane;
    
    public void AddSpeed(float delta)
    {
        _forwardSpeed += delta;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isMoving) return;
        if (InputManager.i.LeftDown) SwitchLanes(-1);
        if (InputManager.i.RightDown) SwitchLanes(1);
        if (InputManager.i.JumpDown && OnGround) Jump();
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce);
    }

    private void FixedUpdate()
    {
        float y = _rb.velocity.y < 0 ? _rb.velocity.y * 1.01f : _rb.velocity.y;
        var targetVelocity = new Vector3(0, y, _forwardSpeed * Time.deltaTime);
        if (!_isMoving) _rb.velocity = Vector3.zero;
        else _rb.velocity = targetVelocity;
    }

    private void SwitchLanes(int dir)
    {
        if (_currentLane == dir) return;
        transform.position += _sidewaysDist * dir * transform.right;
        _currentLane += dir;
    }
}
