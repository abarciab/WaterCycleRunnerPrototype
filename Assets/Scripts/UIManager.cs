using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager i;

    [SerializeField] private TextMeshProUGUI _currentStateText;
    [SerializeField] private TextMeshProUGUI _currentPlaceText;
    public QuestionUIController Question;
    public NewInfoController NewInfo;
    [SerializeField] private Slider _healthSlider;

    private void Awake() => i = this; 
    private void Start()
    {
        SetHealth(1);   
    }

    public void SetHealth(float percent)
    {
        _healthSlider.value = percent;
    }

    public void SetStateText(PlayerState state)
    {
        _currentStateText.text = "Current state: " + state;
    }

    public void SetLocationText(PlayerLocation location)
    {
        _currentPlaceText.text = "Current location: " + location;
    }
}
