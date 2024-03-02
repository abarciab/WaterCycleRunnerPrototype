using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private TextAsset _questionText;
    private Dictionary<int, List<string>> _questionDict = new Dictionary<int, List<string>>();

    private void Start()
    {
        ParseQuestionFile();
    }

    private void ParseQuestionFile()
    {
        var lines = _questionText.text.Split('\n');
        foreach (var line in lines) ParseLine(line);
    }

    private void ParseLine(string line)
    {
        var parts = line.Split(":");
        int ID = int.Parse(parts[0]);

        if (!_questionDict.ContainsKey(ID)) _questionDict.Add(ID, new List<string>());    
        _questionDict[ID].Add(parts[1]);
    }

    public List<string> GetRandomQuestion()
    {
        var keyList = new List<int>(_questionDict.Keys);
        int chosenKey = keyList[Random.Range(0, keyList.Count)];
        return new List<string>(_questionDict[chosenKey]);
    }
}
