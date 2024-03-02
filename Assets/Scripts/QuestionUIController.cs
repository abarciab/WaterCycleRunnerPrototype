using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private List<TextMeshProUGUI> _optionTexts = new List<TextMeshProUGUI>();
    private int _correntOption;

    public void DisplayQuestion()
    {
        var data = GameManager.i.Data.GetRandomQuestion();

        _questionText.text = data[0];
        data.RemoveAt(0);
        AssignButtonText(data);

        gameObject.SetActive(true);
    }

    private void AssignButtonText(List<string> options)
    {
        _correntOption = Random.Range(0, 3);
        _optionTexts[_correntOption].text = options[0];
        if (Random.Range(0, 1f) < 0.5f) (options[1], options[2]) = (options[2], options[1]);

        _optionTexts[GetNextIndex(_correntOption)].text = options[1];
        _optionTexts[GetPreviousIndex(_correntOption)].text = options[2];
    }

    private int GetNextIndex(int index)
    {
        if (index < 2) return index + 1;
        else return 0;
    }

    private int GetPreviousIndex(int index)
    {
        if (index > 0) return index - 1;
        else return 2;
    }

    public void SelectOption(int index)
    {
        if (index == _correntOption) AnswerCorrectly();
        else AnswerWrong();
    }

    private void AnswerWrong()
    {
        GameManager.i.FailQuestion();
        GameManager.i.ResumeMovement();
        CloseQuestionUI();
    }

    private void AnswerCorrectly()
    {
        GameManager.i.AnswerQuestionCorrectly();
        GameManager.i.ResumeMovement();
        CloseQuestionUI();
    }

    private void CloseQuestionUI()
    {
        gameObject.SetActive(false);
    }
}
