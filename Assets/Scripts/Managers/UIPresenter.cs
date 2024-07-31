using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall.Monster;
using INeverFall.Player;
using UnityEngine;
using UnityEngine.UI;

public class UIPresenter : MonoBehaviour
{
    [Header("Model")] 
    [SerializeField] private BossMonster _bossModel;
    [SerializeField] private PlayerCharacter _playerModel;

    [Header("View")] 
    [SerializeField] private Slider _bossHpBar;
    [SerializeField] private Image _playerHpBar;
    [SerializeField] private Image _playerIcon;

    private void OnValidate()
    {
        _bossModel = FindFirstObjectByType<BossMonster>();
        _bossHpBar = GameObject.Find("BossHpBar").GetComponent<Slider>();

        _playerModel = FindFirstObjectByType<PlayerCharacter>();
        _playerHpBar = GameObject.Find("PlayerHpBar").GetComponent<Image>();
    }

    private void Start()
    {
        if (_bossModel is not null)
            _bossModel.HealthChanged += OnBossHpChanged;

        if (_playerModel is not null)
            _playerModel.HpChanged += OnPlayerHpChanged;
    }

    private void OnBossHpChanged(float value)
    {
        _bossHpBar.value = value;
    }

    private void OnPlayerHpChanged(float value)
    {
        _playerHpBar.fillAmount = value;
        StartCoroutine(nameof(_cDamageColor));
    }
    
    private IEnumerator _cDamageColor()
    {
        var mat = _playerIcon;
        mat.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        mat.color = Color.white;
    }
}