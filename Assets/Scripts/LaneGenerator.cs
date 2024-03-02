using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class LaneGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _laneBlockPrefab;
    [SerializeField] private float _blockDist;
    [SerializeField] private GameObject _parentPrefab;
    [SerializeField] private int _numBlocks = 50;
    [SerializeField] private Transform _objectParent;

    private void Start()
    {
        GenerateObjects();
    }

    [ButtonMethod]
    private void GenerateObjects()
    {
        ClearObjects();
        for (int i = 0; i < _numBlocks; i++) {
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
        var position = transform.position + transform.forward * index * _blockDist;
        var newObject = Instantiate(_laneBlockPrefab, _objectParent);
        newObject.name = "object " + index;
        newObject.transform.position = position;
    }
}
