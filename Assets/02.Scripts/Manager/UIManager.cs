using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Stat Info")]
    public Image HPbar;
    public Image DashBar;
    public TextMeshProUGUI HPTxt;
    public TextMeshProUGUI DashTxt;

    public StatIndicator StatIndicator;

    [Header("Item Info")]
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;
    public CharacterManager manager;
    public ItemIndicator ItemIndicator;

    private void OnValidate()
    {
        //플레이어 UI데이터 가져오기
        StatIndicator hpUI = Utility.FindComponent<StatIndicator>(gameObject, "HPBar");
        StatIndicator dashUI = Utility.FindComponent<StatIndicator>(gameObject, "DashBar");
        HPbar = hpUI.Bar;
        HPTxt = hpUI.Txt;
        DashBar = dashUI.Bar;
        DashTxt = dashUI.Txt;
        //아이템 데이터 가져오기
        ItemIndicator itemUI = Utility.FindComponent<ItemIndicator>(gameObject, "ItemUI");
        ItemName = itemUI.ItemName;
        ItemDescription = itemUI.ItemDescription;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        manager = CharacterManager.Instance;
    }

    private void Update()
    {
        if (manager.isInteract)
        {
            Display(manager.Controller.item.itemInfo);
        }
        else
        {
            Display();
        }
    }

    public void Display()
    {
        ItemIndicator.gameObject.SetActive(false);
        StatIndicator.SetPlayerUI(Stat.HP);
        StatIndicator.SetPlayerUI(Stat.Dash);
    }

    public void Display(ItemInfo item)
    {
        Display();
        ItemIndicator.gameObject.SetActive(true);
        ItemIndicator.SetItemUI(item);
    }
}
