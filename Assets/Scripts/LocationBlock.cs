using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationBlock : MonoBehaviour
{
    [SerializeField] private GameObject _surfaceDecor;
    [SerializeField] private GameObject _oceanDecor;
    [SerializeField] private GameObject _cloudDecor;

    public void SetLocation(PlayerLocation newLocation)
    {
        _surfaceDecor.SetActive(newLocation == PlayerLocation.Surface);
        _oceanDecor.SetActive(newLocation == PlayerLocation.Ocean);
        _cloudDecor.SetActive(newLocation == PlayerLocation.Clouds);
    }

    public void ReGenerateObstacles(bool leaveGap = false)
    {
        foreach (var placer in GetComponentsInChildren<ObstaclePlacer>()) {
            placer.RegenerateObstacles(leaveGap);
        }
    }
}
