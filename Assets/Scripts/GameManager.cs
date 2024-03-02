using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum PlayerState { SurfaceWater, Iceberg, Vapor, Rain, UndergroundWater, Hail, OceanWater}
public enum PlayerLocation { Surface, Ocean, Clouds, Falling, Underground}

public class GameManager : MonoBehaviour
{
    public static GameManager i;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Fade fade;
    [SerializeField] MusicPlayer music;
    [SerializeField, ReadOnly] private PlayerState _currentState;
    [SerializeField, ReadOnly] private PlayerLocation _currentLocation;
    [SerializeField] private PlayerStats _stats;
    [SerializeField] public List<Obstacle> Obstacles = new List<Obstacle>();
    [SerializeField] private LocationController _locationController;
    [SerializeField] private PlayerMovement _moveScript;

    [HideInInspector] public UnityEvent OnStateChanged;
    [HideInInspector] public UnityEvent OnLocationChanged;

    public DataManager Data;
    [HideInInspector] public PlayerState CurrentState => _currentState;
    [HideInInspector] public PlayerLocation CurrentLocation => _currentLocation;
    [HideInInspector] public int GoalsRemaining;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        fade.Hide();
        _currentLocation = PlayerLocation.Surface;
        _currentState = PlayerState.SurfaceWater;
        UpdateUI();
        Invoke(nameof(UpdateObstacles), 0.01f);
    }

    public void FailQuestion()
    {
        _stats.UpdateHealth(-1);
    }

    public void AnswerQuestionCorrectly()
    {
        _stats.UpdateHealth(1);
    }

    public void ResumeMovement()
    {
        _moveScript.SetIsMoving(true);
    }

    public void TriggerChange(ObstacleType type)
    {

        var oldState = _currentState;
        var oldLocation = _currentLocation;

        if (type == ObstacleType.QuestionBlocker) {
            _moveScript.SetIsMoving(false);
            UIManager.i.Question.DisplayQuestion();
            return;
        }
        else if (type == ObstacleType.Blocker) {
            _stats.UpdateHealth(-1);
            return;
        }

        if (type == ObstacleType.Sunlight && _currentState == PlayerState.SurfaceWater) TransitionToVapor();
        else if (type == ObstacleType.Sunlight && _currentState == PlayerState.OceanWater) TransitionToVapor();
        else if (type == ObstacleType.Sunlight && _currentState == PlayerState.Iceberg) TransitionToOceanWater();
        else if (type == ObstacleType.Sunlight && _currentState == PlayerState.Hail) TransitionToRain();

        else if (type == ObstacleType.Frost && _currentState == PlayerState.OceanWater) TransitionToIceBerg();
        else if (type == ObstacleType.Frost && _currentState == PlayerState.Rain) TransitionToHail();

        else if (type == ObstacleType.Pressure && _currentState == PlayerState.Vapor) TransitionToRain();

        else if (type == ObstacleType.Ground && _currentState == PlayerState.Rain) TransitionToSurfaceWater();
        else if (type == ObstacleType.Ground && _currentState == PlayerState.Hail) TransitionToSurfaceWater();
        else if (type == ObstacleType.Ground && _currentState == PlayerState.SurfaceWater) TransitionToUndergroundWater();

        else if (type == ObstacleType.Ocean && _currentState == PlayerState.SurfaceWater) TransitionToOceanWater();
        else if (type == ObstacleType.Ocean && _currentState == PlayerState.UndergroundWater) TransitionToOceanWater();
        else if (type == ObstacleType.Ocean && _currentState == PlayerState.Rain) TransitionToOceanWater();
        else if (type == ObstacleType.Ocean && _currentState == PlayerState.Hail) TransitionToIceBerg();

        _stats.SetState(_currentState);
        UpdateUI();
        UpdateObstacles();
        _locationController.SwitchLocations(_currentLocation);

        UIManager.i.NewInfo.Transition(oldState, _currentState, oldLocation, _currentLocation);
        UpdateGoals(oldState, oldLocation);
    }

    private void UpdateGoals(PlayerState oldState, PlayerLocation oldLocation)
    {
        GoalsRemaining = 0;
        OnStateChanged.Invoke();
        OnLocationChanged.Invoke();
        if (GoalsRemaining == 0) {
            EndGame();
            //_moveScript.SetIsMoving(false);
        }
    }

    private void UpdateObstacles()
    {
        foreach (var o in Obstacles) o.UpdateType(_currentState);
    }

    private void UpdateUI()
    {
        UIManager.i.SetStateText(_currentState);
        UIManager.i.SetLocationText(_currentLocation);
    }

    private void TransitionToUndergroundWater()
    {
        _currentState = PlayerState.UndergroundWater;
        _currentLocation = PlayerLocation.Underground;
    }


    private void TransitionToOceanWater()
    {
        _currentState = PlayerState.OceanWater;
        _currentLocation = PlayerLocation.Ocean;
    }

    private void TransitionToSurfaceWater()
    {
        _currentState = PlayerState.SurfaceWater;
        _currentLocation = PlayerLocation.Surface;
    }

    private void TransitionToHail()
    {
        _currentState = PlayerState.Hail;
    }

    private void TransitionToVapor()
    {
        _currentState = PlayerState.Vapor;
        _currentLocation = PlayerLocation.Clouds;
    }

    private void TransitionToRain()
    {
        _currentState = PlayerState.Rain;
        _currentLocation = PlayerLocation.Falling;
    }

    private void TransitionToIceBerg()
    {
        _currentState = PlayerState.Iceberg;
        _currentLocation = PlayerLocation.Ocean;
    }


    void TogglePause()
    {
        if (Time.timeScale == 0) Resume();
        else Pause();
    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioManager.i.Resume();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        AudioManager.i.Pause();
    }

    [ButtonMethod]
    public void LoadMenu()
    {
        Resume();
        StartCoroutine(FadeThenLoadScene(0));
    }

    [ButtonMethod]
    public void EndGame()
    {
        Resume();
        StartCoroutine(FadeThenLoadScene(2));
    }

    private IEnumerator FadeThenLoadScene(int num)
    {
        fade.Appear(); 
        music.FadeOutCurrent(fade.fadeTime);
        yield return new WaitForSeconds(fade.fadeTime + 0.5f);
        Destroy(AudioManager.i.gameObject);
        SceneManager.LoadScene(num);
    }

}
