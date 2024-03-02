using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoalType { State, Location}

public class GoalItem : MonoBehaviour
{
    [SerializeField] private GoalType _type;
    [SerializeField, ConditionalField(nameof(_type), false, GoalType.State)] private PlayerState _state;
    [SerializeField, ConditionalField(nameof(_type), false, GoalType.Location)] private PlayerLocation _location;
    [SerializeField] private GameObject _checkBox;
    [SerializeField] private Sound _successSound;

    private void Start()
    {
        _successSound = Instantiate(_successSound);
        if (_type == GoalType.State) GameManager.i.OnStateChanged.AddListener(CheckForState);
        else GameManager.i.OnLocationChanged.AddListener(CheckForLocation);
    }

    private void CheckForState() {
        if (_checkBox.activeInHierarchy) return;
        if (GameManager.i.CurrentState == _state) MarkAsComplete();
        else GameManager.i.GoalsRemaining += 1;
    }

    private void CheckForLocation()
    {
        if (_checkBox.activeInHierarchy) return;
        if (GameManager.i.CurrentLocation == _location) MarkAsComplete();
        else GameManager.i.GoalsRemaining += 1;
    }

    private void MarkAsComplete()
    {
        _checkBox.SetActive(true);
        _successSound.Play();
    }
}
