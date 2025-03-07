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
    public CharacterManager charManager;

    public ItemIndicator ItemIndicator;


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
        charManager = CharacterManager.Instance;
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
