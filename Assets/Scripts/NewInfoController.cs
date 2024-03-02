using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NewInfoController : MonoBehaviour
{
    [SerializeField] private List<TransitionInformationData> _data = new List<TransitionInformationData>();
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _bodyText;

    public void Transition(PlayerState oldState, PlayerState newState, PlayerLocation oldLocation, PlayerLocation newLocation)
    {
        var options = new List<TransitionInformationData>(_data);

        if (oldState == newState) options = options.Where(x => x.SpecificState == false).ToList();
        if (oldLocation == newLocation) options = options.Where(x => x.SpecificLocation == false).ToList();

        options = options.OrderByDescending(x => x.CheckMatch(newState, newLocation)).ToList();

        if (options.Count > 0 && options[0].CheckMatch(newState, newLocation) > 0) DisplayNewInfo(options[0]);
    }

    private void DisplayNewInfo(TransitionInformationData data)
    {
        _data.Remove(data);
        gameObject.SetActive(true);
        FindObjectOfType<PlayerMovement>().SetIsMoving(false);
        _headerText.text = data.Header;
        _bodyText.text = data.Body;
    }

    public void Continue()
    {
        gameObject.SetActive(false);
        FindObjectOfType<PlayerMovement>().SetIsMoving(true);
    }
}
