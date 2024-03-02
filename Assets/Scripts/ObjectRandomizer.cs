using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private GameObject _parentPrefab;
    [SerializeField] private Vector2 _scaleLimits;
    [SerializeField] private int _numObjects = 50;
    [SerializeField] private Vector4 _bounds;
    [SerializeField] private Transform _objectParent;

    private void Start()
    {
        GenerateObjects();
    }

    [ButtonMethod]
    private void GenerateObjects()
    {
        ClearObjects();
        for (int i = 0; i < _numObjects; i++) {
            GenerateObject(i);
        }
    }

    [ButtonMethod]  
    private void ClearObjects()
    {
        if (_objectParent != null) {
            if (Application.isPlaying) Destroy(_objectParent.gameObject);
            else DestroyImmediate(_objectParent.gameObject);
        }

        var newParent = Instantiate(_parentPrefab, transform).transform;
        newParent.name = "object parent " + Random.Range(1, 11);

        _objectParent = newParent.transform;
    }

    private void GenerateObject(int index)
    {
        var position = GetRandomPos();
        float scale = Random.Range(_scaleLimits.x, _scaleLimits.y);
        var newObject = Instantiate(_obstaclePrefab, _objectParent);
        newObject.name = "object " + index;
        newObject.transform.position = position;
        newObject.transform.localScale = Vector3.one * scale;
        newObject.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
    }

    private Vector3 GetRandomPos()
    {
        float x = Random.Range(_bounds.x, _bounds.y);
        float z = Random.Range(_bounds.z, _bounds.w);
        return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }

    private void OnDrawGizmosSelected()
    {
        float posX = transform.position.x + _bounds.x;
        float negX = transform.position.x + _bounds.y;
        float posZ = transform.position.z + _bounds.z;
        float negZ = transform.position.z + _bounds.w;
        float y = transform.position.y + 2.5f;

        var corner1 = new Vector3(negX, y, negZ);
        var corner2 = new Vector3(negX, y, posZ);
        var corner3 = new Vector3(posX, y, posZ);
        var corner4 = new Vector3(posX, y, negZ);

        Gizmos.DrawLine(corner1, corner2);
        Gizmos.DrawLine(corner2, corner3);
        Gizmos.DrawLine(corner3, corner4);
        Gizmos.DrawLine(corner4, corner1);
    }
}
