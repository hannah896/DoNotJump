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
    //����
    public GameObject amountBG;
    public TextMeshProUGUI amountTxt;
    //������Ʈ
    public ItemInfo item;
    //������ ����ũ
    [ShowInInspector]
    Sprite mask;
    //���콺 �ø� ������Ʈ
    public Outline outLine;
    //Ŭ��
    public Button button;
    public Image Icon;
    public int SlotIndex;
    public int amount;


    private void OnValidate()
    {
        outLine = GetComponent<Outline>();
        Icon = Utility.FindComponent<Image>(gameObject, "Icon");
        amountBG = transform.Find("amountBG").gameObject;
        amountTxt = amountBG.GetComponentInChildren<TextMeshProUGUI>();
        outLine.enabled = false;
        mask = Icon.sprite;
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

    void ResetSlot()
    {
        amountBG.SetActive(false);
        amountTxt.text = string.Empty;
        item = null;
        outLine.enabled = false;
        Icon.sprite = mask;
        amount = 0;
    }

    public void UseItem()
    {
        if (item == null) return;
        amount--;
        if (amount == 0)
        {
            ResetSlot();
        }
    }

    private void OnDisable()
    {
        outLine.enabled = false;
    }
}
