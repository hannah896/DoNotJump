using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VInspector;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //갯수
    public GameObject amountBG;
    public TextMeshProUGUI amountTxt;
    //오브젝트
    public ItemObject item;
    //아이콘 마스크
    [ShowInInspector]
    Sprite mask;
    //마우스 올린 오브젝트
    public Outline outLine;
    //클릭
    public Button button;
    public Image Icon;
    public int SlotIndex;
    public int amount;

    private Color color;


    private void OnValidate()
    {
        outLine = GetComponent<Outline>();
        Icon = Utility.FindComponent<Image>(gameObject, "Icon");
        amountBG = transform.Find("amountBG").gameObject;
        amountTxt = amountBG.GetComponentInChildren<TextMeshProUGUI>();
        outLine.enabled = false;
        amount = 0;
        if (!TryGetComponent<Button>(out button))
        {
            button = gameObject.AddComponent<Button>();
        }
        color = Icon.color;
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
        InventoryManager.Instance.selectedItemName.text = item.itemInfo._name;
        InventoryManager.Instance.selectedItemDescription.text = item.itemInfo.description;
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
            if (item.itemInfo._name.Equals(obj.itemInfo._name))
            {
                amount++;
                return true;
            }
            else
            {
                return false;
            }
        }

        item = obj;
        Icon.sprite = item.itemInfo.Icon;
        color.a = 100;
        Icon.color = color;

        amount += 1;
        amountTxt.text = amount.ToString();

        amountBG.SetActive(true);
        return true;              
    }

    void ResetSlot()
    {
        amountBG.SetActive(false);
        amountTxt.text = string.Empty;
        item = null;
        outLine.enabled = false;
        color.a = 0;
        Icon.color = color;
        amount = 0;
    }

    public void UseItem()
    {
        if (item == null) return;
        amount--;
        CharacterManager.Instance.Condition.Buff(item.itemInfo, item.itemInfo.type);
        if (amount == 0)
        {
            ResetSlot();
        }
        else
        {
            amountTxt.text = amount.ToString();
        }
    }

    private void OnDisable()
    {
        outLine.enabled = false;
    }
}
