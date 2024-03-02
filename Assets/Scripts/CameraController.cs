using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _lookOffset;
    [SerializeField] private float _lerp = 0.01f;
    private Vector3 lookPos;

    public void SnapToPos()
    {
        transform.position = _player.TransformPoint(_offset);
        lookPos = _player.TransformPoint(_lookOffset);
        transform.LookAt(lookPos);
    }

    private void Start()
    {
        lookPos = _player.TransformPoint(_lookOffset);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _player.TransformPoint(_offset), _lerp * Time.deltaTime);
        lookPos = Vector3.Lerp(lookPos, _player.TransformPoint(_lookOffset), _lerp * Time.deltaTime);

        transform.LookAt(lookPos);
    }
}
