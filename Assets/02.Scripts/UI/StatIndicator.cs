using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatIndicator : MonoBehaviour
{
    public Stat stat;
    private UIManager _uiManager;

    //자식 오브젝트들 연결시키기
    private void Start()
    {
        UIManager.Instance.StatIndicator = this;
        _uiManager = UIManager.Instance;

        switch(stat)
        {
            case Stat.HP:
                _uiManager.HPbar = GetComponentInChildren<Image>();
                _uiManager.HPTxt = GetComponentInChildren<TextMeshProUGUI>();
                break;
            case Stat.Dash:
                _uiManager.DashBar = GetComponentInChildren<Image>();
                _uiManager.DashTxt = GetComponentInChildren<TextMeshProUGUI>();
                break;
        }
    }
    

    public void SetPlayerUI(Stat stat)
    {
        switch (stat)
        {
            case Stat.HP:
                _uiManager.HPbar.fillAmount = _uiManager.charManager.Player.HP / _uiManager.charManager.Player.maxHP;
                _uiManager.HPTxt.text = $"{_uiManager.charManager.Player.HP}/{_uiManager.charManager.Player.maxHP}";
                break;
            case Stat.Dash:
                _uiManager.DashBar.fillAmount = _uiManager.charManager.Player.Dash / _uiManager.charManager.Player.MaxDash;
                _uiManager.DashTxt.text = $"{_uiManager.charManager.Player.Dash}/{_uiManager.charManager.Player.MaxDash}";
                break;
        }
    }
}
