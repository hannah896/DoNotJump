using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemIndicator : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.ItemName = Utility.FindComponent<TextMeshProUGUI>(this.gameObject, "ItemName");
        UIManager.Instance.ItemDescription = Utility.FindComponent<TextMeshProUGUI>(this.gameObject, "ItemDescription");
        UIManager.Instance.ItemIndicator = this;
    }

    public void SetItemUI(ItemInfo item)
    {
        UIManager.Instance.ItemName.text = item.name;
        UIManager.Instance.ItemDescription.text = item.description;
    }
}
