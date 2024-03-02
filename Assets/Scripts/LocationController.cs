using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationController : MonoBehaviour
{
    [SerializeField] List<LocationBlock> _blocks = new List<LocationBlock>();
    private PlayerLocation _currentLocation = PlayerLocation.Falling;
    [SerializeField] private Renderer _groundRenderer;
    [SerializeField] private List<Material> _groundMaterials = new List<Material>();

    private void Start()
    {
        SwitchLocations(PlayerLocation.Surface);
    }

    public void SwitchLocations(PlayerLocation newLocation)
    {
        if (newLocation == _currentLocation) return;

        _currentLocation = newLocation;
        foreach (var b in _blocks) {
            b.SetLocation(newLocation);
            b.ReGenerateObstacles(true);
        }

        _groundRenderer.material = _groundMaterials[(int)newLocation];
        GetComponent<InfiniteScolling>().ResetBlocks();
        FindObjectOfType<PlayerMovement>().transform.position = Vector3.zero;
        FindObjectOfType<PlayerMovement>().SetLane(0);
        FindObjectOfType<CameraController>().SnapToPos();
    }
}
