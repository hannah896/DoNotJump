using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemIndicator : MonoBehaviour
{
    UIManager uiManager;

    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;

    private void OnValidate()
    {
        ItemName = Utility.FindComponent<TextMeshProUGUI>(this.gameObject, "ItemName");
        ItemDescription = Utility.FindComponent<TextMeshProUGUI>(this.gameObject, "ItemDescription");
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
        uiManager.ItemIndicator = this;
    }

    public void SetItemUI(ItemInfo item)
    {
        uiManager.ItemName.text = item.name;
        uiManager.ItemDescription.text = item.description;
    }
}
