using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Image HPbar;
    private Image DashBar;
    private TextMeshProUGUI HP;
    private TextMeshProUGUI Dash;

    private CharacterManager charManager;

    private void Start()
    {
        charManager = CharacterManager.Instance;
        HPbar = transform.Find("HPBar").GetComponentInChildren<Image>();
        DashBar = transform.Find("DashBar").GetComponentInChildren<Image>();
        HP = transform.Find("HPBar").GetComponentInChildren<TextMeshProUGUI>();
        Dash = transform.Find("DashBar").GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        SetUI(Stat.HP);
        SetUI(Stat.Dash);
    }

    void SetUI(Stat stat)
    {
        switch(stat)
        {
            case Stat.HP:
                HPbar.fillAmount = charManager.Player.HP / charManager.Player.maxHP;
                HP.text = $"{charManager.Player.HP}/{charManager.Player.maxHP}";
                break;
            case Stat.Dash:
                DashBar.fillAmount = charManager.Player.Dash / charManager.Player.maxDash;
                Dash.text = $"{charManager.Player.Dash}/{charManager.Player.maxDash}";
                break;
        }
    }
}
