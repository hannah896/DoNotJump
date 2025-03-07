using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Stat Info")]
    private Image HPbar;
    private Image DashBar;
    private TextMeshProUGUI HP;
    private TextMeshProUGUI Dash;

    [Header("Item Info")]
    private Text ItemName;
    private Text ItemDescription;
    private CharacterManager charManager;

    private void Start()
    {
        charManager = CharacterManager.Instance;

        HPbar = Utility.FindComponent<Image>(transform.Find("PlayerUI").gameObject, "HP");
        DashBar = Utility.FindComponent<Image>(transform.Find("PlayerUI").gameObject, "Dash");
        HP = Utility.FindComponent<TextMeshProUGUI>(transform.Find("PlayerUI").gameObject, "HPTxt");
        Dash = Utility.FindComponent<TextMeshProUGUI>(transform.Find("PlayerUI").gameObject, "DashTxt");

        ItemName = Utility.FindComponent<Text>(transform.Find("ItemUI").gameObject, "ItemName");
        ItemDescription = Utility.FindComponent<Text>(transform.Find("ItemUI").gameObject, "ItemDescription");

    }

    private void Update()
    {
        SetPlayerUI(Stat.HP);
        SetPlayerUI(Stat.Dash);
    }

    void SetPlayerUI(Stat stat)
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

    void SetItemUI()
    {

    }
}
