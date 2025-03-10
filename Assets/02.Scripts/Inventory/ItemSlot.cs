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
    public ItemInfo item;
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
        if (Icon == null)
        {
            Debug.LogError("Icon 이미지가 할당되지 않음: " + gameObject.name);
        }
        amountBG = transform.Find("amountBG").gameObject;
        amountTxt = amountBG.GetComponentInChildren<TextMeshProUGUI>();
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
        if (item == null) return;
        InventoryManager.Instance.ShowInfo(SlotIndex);
        InventoryManager.Instance.selectedItemName.text = item._name;
        InventoryManager.Instance.selectedItemDescription.text = item.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outLine.enabled = false;
        if (item == null) return;
        InventoryManager.Instance.CloseInfo();
    }

    public void OnClick()
    {
        if (item == null) return;
        UseItem();
    }
    public bool Input(ItemObject obj)
    {
        if (item != null)
        {
            if (item._name.Equals(obj.itemInfo._name))
            {
                amount++;
                return true;
            }
            else
            {
                return false;
            }
        }

        item = obj.itemInfo;
        Icon.sprite = item.Icon;

        amount += 1;
        amountTxt.text = amount.ToString();

        amountBG.SetActive(true);
        return true;              
    }

    void Reset()
    {
        amountBG.SetActive(false);
        amountTxt.text = string.Empty;
        item = null;
        outLine.enabled = false;
        Icon = null;
        amount = 0;
    }

    public void UseItem()
    {
        if (item == null) return;
        amount--;
        if (amount == 0)
        {
            Reset();
        }
    }

    private void OnDisable()
    {
        outLine.enabled = false;
    }
}
