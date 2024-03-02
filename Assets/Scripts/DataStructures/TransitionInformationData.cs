using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransitionInformationData
{
    public bool SpecificState;
    [SerializeField, ConditionalField(nameof(SpecificState))] private PlayerState _requiredState;

    public bool SpecificLocation;
    [SerializeField, ConditionalField(nameof(SpecificLocation))] public PlayerLocation _requiredLocation;

    public string Header;
    [TextArea(3,10)] public string Body;

    public int CheckMatch(PlayerState state, PlayerLocation location)
    {
        int match = 0;
        if (SpecificState) match += state == _requiredState ? 1 : -1;
        if (SpecificLocation) match += location == _requiredLocation ? 1 : -1;

        return match;
    }
}
