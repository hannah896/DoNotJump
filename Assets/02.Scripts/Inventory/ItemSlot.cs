using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //갯수
    public GameObject amountBG;
    public TextMeshProUGUI amountTxt;
    //오브젝트
    public ItemObject itemObject;
    //마우스 올린 오브젝트
    public Outline outLine;
    //클릭
    public Button button;
    public Image Icon;
    public int SlotIndex;
    public int amount;


    private void OnValidate()
    {
        outLine = GetComponent<Outline>();
        Icon = Utility.FindComponent<Image>(gameObject, "Icon");
        amountBG = transform.Find("amountBG").gameObject;
        amountTxt = amountBG.GetComponent<TextMeshProUGUI>();
        outLine.enabled = false;
        amount = 0;

        if (!TryGetComponent<Button>(out button))
        {
            button = gameObject.AddComponent<Button>();
        }
    }

    public void Awake()
    {
        amountBG.SetActive(false);
        button.onClick.AddListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outLine.enabled = true;
        InventoryManager.Instance.ShowInfo(SlotIndex);
        InventoryManager.Instance.selectedItemName.text = itemObject.itemInfo._name;
        InventoryManager.Instance.selectedItemDescription.text = string.Empty;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outLine.enabled = false;
        InventoryManager.Instance.CloseInfo();
    }

    public void OnClick()
    {
        if (itemObject == null) return;
        UseItem();
    }
    public bool Input(ItemObject obj)
    {
        if (itemObject != null)
        {
            if (itemObject.itemInfo._name.Equals(obj.itemInfo.name))
            {
                amount++;
                return true;
            }
            else
            {
                return false;
            }
        }

        itemObject = obj;
        Icon.sprite = itemObject.itemInfo.Icon;
        amount += 1;
        amountTxt.text = amount.ToString();
        return true;
    }

    public void UseItem()
    {
        if (itemObject == null) return;
        amount--;
        if (amount == 0)
        CharacterManager.Instance.Controller.Use();
    }
}
