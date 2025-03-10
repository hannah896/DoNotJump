using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatIndicator : MonoBehaviour
{
    private UIManager _uiManager;

    public Image Bar;
    public TextMeshProUGUI Txt;

    private void OnValidate()
    {
        Bar = Utility.FindComponent<Image>(gameObject, "Bar");
        Txt = Utility.FindComponent<TextMeshProUGUI>(gameObject, "Txt");
    }

    //자식 오브젝트들 연결시키기
    private void Start()
    {
        UIManager.Instance.StatIndicator = this;
        _uiManager = UIManager.Instance;
    }
    

    public void SetPlayerUI(Stat stat)
    {
        switch (stat)
        {
            case Stat.HP:
                _uiManager.HPbar.fillAmount = _uiManager.manager.Player.HP / _uiManager.manager.Player.MaxHP;
                _uiManager.HPTxt.text = $"{(int)_uiManager.manager.Player.HP}/{_uiManager.manager.Player.MaxHP}";

                break;
            case Stat.Dash:
                _uiManager.DashBar.fillAmount = _uiManager.manager.Player.Dash / _uiManager.manager.Player.MaxDash;
                _uiManager.DashTxt.text = $"{(int)_uiManager.manager.Player.Dash}/{_uiManager.manager.Player.MaxDash}";
                break;
        }
    }
}
