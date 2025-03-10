using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    //인벤토리 슬롯 배열
    public ItemSlot[] inventorySlots;

    //아이템 슬롯 프리펩
    public ItemSlot itemSlot;

    //선택한 아이템 슬롯
    public ItemSlot SelectSlot;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;

    Transform Slots;

    private void OnValidate()
    {
        itemSlot = (ItemSlot)AssetDatabase.LoadAssetAtPath("Assets/03.Prefabs/Inventory/Slot.prefab", typeof(ItemSlot));
        Slots = transform.Find("Slots");
    }

    private void Awake()
    {
        inventorySlots = new ItemSlot[20];

        for (int i = 0; i < 20; i++)
        {
            inventorySlots[i] = Instantiate(itemSlot, Slots);
            inventorySlots[i].SlotIndex = i;
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CloseInfo();
    }

    public void ShowInfo(int index)
    {
        SelectSlot = inventorySlots[index];
    }

    public void CloseInfo()
    {
        SelectSlot = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
    }

    //인벤토리에 아이템 넣기
    public void Input(ItemObject item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.Input(item))
            {
                return;
            }
            else
            {
                continue;
            }
        }
    }
}
