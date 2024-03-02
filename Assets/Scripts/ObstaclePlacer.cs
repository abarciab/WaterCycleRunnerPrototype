using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ObstaclePlacer : MonoBehaviour
{
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private List<Transform> _slots = new List<Transform>();
    [SerializeField] private float _spawnChance = 0.5f;
    private const string _avaliableSlotName = "AVALIABLE";
    private const string _occupiedSlotName = "OCCUPIED";

    private void Start()
    {
        RegenerateObstacles(true);
    }

    private void SpawnObject()
    {
        int tries = 0;
        Transform chosenSlot;
        do {
            tries += 1;
            chosenSlot = _slots[Random.Range(0, _slots.Count)];
            if (tries > 5) return;
        } while (chosenSlot.gameObject.name == _occupiedSlotName);

        Instantiate(_obstaclePrefab, chosenSlot);

        chosenSlot.gameObject.name = _occupiedSlotName;
    }

    public void RegenerateObstacles(bool leaveGap = false)
    {
        if (Application.isPlaying) DestroyObstacles();
        if (leaveGap && transform.GetSiblingIndex() <= 2 && transform.parent.GetSiblingIndex() == 0) return;

        for (int i = 0; i < 1; i++) {
            if (Random.Range(0, 1f) < _spawnChance) SpawnObject();
        }
    }

    private void DestroyObstacles()
    {
        foreach (var s in _slots) {
            foreach (Transform child in s.transform) {
                Destroy(child.gameObject);
            }
            s.gameObject.name = _avaliableSlotName;
        }
    }

}
