using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Outline outLine;
    public TextMeshProUGUI amountTxt;
    public ItemObject itemObject;
    public Image Icon;
    public int SlotIndex;
    public int amount;


    private void OnValidate()
    {
        outLine = GetComponent<Outline>();
        Icon = Utility.FindComponent<Image>(gameObject, "Icon");
        amountTxt = GetComponent<TextMeshProUGUI>();
        outLine.enabled = false;
        amount = 0;
    }
    public void Input(ItemObject obj)
    {
        itemObject = obj;
        Icon.sprite = itemObject.itemInfo.Icon;
        amount += 1;
        amountTxt.text = amount.ToString();
    }

    public void UseItem()
    {
        if (itemObject == null) return;
        CharacterManager.Instance.Controller.Use();
    }
}
