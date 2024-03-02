using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScolling : MonoBehaviour
{
    [SerializeField] private float _repeatDist;
    [SerializeField] private Transform _currentBlock;
    [SerializeField] private Transform _nextBlock;
    [SerializeField] private Transform _player;
    private Vector2 _lastPosition;

    private void Start()
    {
        _lastPosition = new Vector2(_player.position.x, _player.position.z);
    }

    private void Update()
    {
        var playerPos = new Vector2(_player.position.x, _player.position.z);
        float dist = Vector2.Distance(playerPos, _lastPosition);
        if (dist >= _repeatDist) MoveBlock();
    }

    private void MoveBlock()
    {
        _currentBlock.position += _currentBlock.forward * _repeatDist * 2;

        (_nextBlock, _currentBlock) = (_currentBlock, _nextBlock);
        _lastPosition = new Vector2(_player.position.x, _player.position.z);
        _nextBlock.GetComponent<LocationBlock>().ReGenerateObstacles();
    }

    public void ResetBlocks()
    {
        _lastPosition = Vector2.zero;
        _currentBlock.position = Vector3.zero;
        _nextBlock.position = _currentBlock.position + Vector3.forward * _repeatDist;
    }
}
