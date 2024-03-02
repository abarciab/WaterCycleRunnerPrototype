using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private GameObject _liquidModel;
    [SerializeField] private GameObject _iceModel;
    [SerializeField] private GameObject _gasModel;

    [SerializeField] private int _maxHealth = 4;
    [SerializeField] private int _health = 4;
    [SerializeField] private float _maxHealthSpeedIncrease = 50;
    [SerializeField] private Sound _hurtSound;

    private void Start()
    {
        _hurtSound = Instantiate(_hurtSound);
        _health = _maxHealth;
    }

    public void SetState(PlayerState state)
    {
        bool liquid = state == PlayerState.SurfaceWater || state == PlayerState.Rain || state == PlayerState.UndergroundWater || state == PlayerState.OceanWater;
        bool solid = state == PlayerState.Iceberg || state == PlayerState.Hail;

        _liquidModel.SetActive(liquid);
        _iceModel.SetActive(solid);
        _gasModel.SetActive(state == PlayerState.Vapor);
    }

    public void UpdateHealth(int delta)
    {
        if (delta < 0) _hurtSound.Play();

        _health += delta;
        if (_health > _maxHealth) {
            _health = _maxHealth;
            GetComponent<PlayerMovement>().AddSpeed(_maxHealthSpeedIncrease);
        }

        UIManager.i.SetHealth((float)_health / _maxHealth);
        if (_health <= 0) Die();
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
