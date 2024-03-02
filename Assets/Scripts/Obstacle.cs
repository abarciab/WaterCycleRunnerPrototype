using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum ObstacleType { Sunlight, Pressure, Frost, Ground, Ocean, Blocker, QuestionBlocker}

[SelectionBase]
public class Obstacle : MonoBehaviour
{
    [SerializeField, ReadOnly] private ObstacleType _type;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private List<Material> _materials;
    [SerializeField, Range(0, 1)] private float _blockerChance = 0.7f;
    [SerializeField, Range(0, 1)] private float _questionBlockerChance = 0.2f;
    Collider _player;
    [SerializeField] private float _resetRange = 0.5f;
    [SerializeField] private TextMeshProUGUI _labelText;

    private bool _triggered;

    private void Start()
    {
        GameManager.i.Obstacles.Add(this);
        gameObject.name = "obstacle " + Random.Range(0, 100);
        UpdateType(GameManager.i.CurrentState);
    }

    private void Update()
    {
        if (!_player) return;

        var colliders = Physics.OverlapSphere(transform.position + Vector3.up * 0.4f, _resetRange).ToList();
        if (!colliders.Contains(_player)) {
            _triggered = false;
            _player = null;
        }
    }

    public void UpdateType(PlayerState currentState)
    {
        var options = new List<ObstacleType>();

        if (currentState == PlayerState.Rain) options = new List<ObstacleType> { ObstacleType.Ground, ObstacleType.Frost, ObstacleType.Ocean};
        if (currentState == PlayerState.SurfaceWater) options = new List<ObstacleType> { ObstacleType.Sunlight, ObstacleType.Ocean, ObstacleType.Ground };

        if (currentState == PlayerState.OceanWater) options = new List<ObstacleType> { ObstacleType.Sunlight, ObstacleType.Frost };
        if (currentState == PlayerState.Hail) options = new List<ObstacleType> { ObstacleType.Sunlight, ObstacleType.Ground };

        if (currentState == PlayerState.Vapor) options = new List<ObstacleType> { ObstacleType.Pressure};
        if (currentState == PlayerState.Iceberg) options = new List<ObstacleType> { ObstacleType.Sunlight};
        if (currentState == PlayerState.UndergroundWater) options = new List<ObstacleType> { ObstacleType.Ocean };

        if (Random.Range(0, 1f) < _blockerChance) SetAsBlocker();
        else _type = options[Random.Range(0, options.Count)];
        if (_renderer) _renderer.material = _materials[(int)_type];

        SetLabelText();
    }

    private void SetLabelText()
    {
        string displayName = "Sunlight";
        switch (_type) {
            case ObstacleType.Pressure:
                displayName = "Pressure";
                break;
            case ObstacleType.Frost:
                displayName = "Frost";
                break;
            case ObstacleType.Ground:
                displayName = "Dirt";
                break;
            case ObstacleType.Ocean:
                displayName = "Ocean";
                break;
            case ObstacleType.Blocker:
                displayName = "Obstacle";
                break;
            case ObstacleType.QuestionBlocker:
                displayName = "Bystander";
                break;
        }
        _labelText.text = displayName;
    }

    private void SetAsBlocker()
    {
        if (Random.Range(0, 1f) < _questionBlockerChance) _type = ObstacleType.QuestionBlocker;
        else _type = ObstacleType.Blocker;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (other.GetComponent<PlayerMovement>()) Trigger(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_triggered) return;
        if (other.GetComponent<PlayerMovement>()) Trigger(other);
    }

    private void Trigger(Collider collider)
    {
        _triggered = true;
        GameManager.i.TriggerChange(_type);
        _player = collider;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.4f, _resetRange);
    }
}
