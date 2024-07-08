using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall.Monster;
using UnityEngine;
using UnityEngine.UI;

public class UIPresenter : MonoBehaviour
{
    [Header("Model")] 
    [SerializeField] private BossMonster _bossModel;
    
    [Header("View")] 
    [SerializeField] private Slider _bossHpBar;

    private void OnValidate()
    {
        _bossModel = FindFirstObjectByType<BossMonster>();
        _bossHpBar = GameObject.Find("BossHpBar").GetComponent<Slider>();
    }
    
    private void Start()
    {
        _bossModel.HealthChanged += OnBossHpChanged;
    }
    
    private void OnBossHpChanged(float value)
    {
        _bossHpBar.value = value;
    }
}
