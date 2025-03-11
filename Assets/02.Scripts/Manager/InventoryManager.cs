using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    //�κ��丮 ���� �迭
    public ItemSlot[] inventorySlots;

    //������ ���� ������
    public ItemSlot itemSlot;

    //������ ������ ����
    public ItemSlot SelectSlot;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;

    Transform Slots;

    private void OnValidate()
    {
        if (itemSlot == null) itemSlot = (ItemSlot)AssetDatabase.LoadAssetAtPath("Assets/03.Prefabs/Inventory/Slot.prefab", typeof(ItemSlot));
        if (Slots == null) Slots = transform.Find("Slots");
        if (selectedItemName = null) selectedItemName = Utility.FindComponent<TextMeshProUGUI>(transform.Find("ItemInfo").gameObject, "name");
        if (selectedItemDescription == null) selectedItemDescription = Utility.FindComponent<TextMeshProUGUI>(transform.Find("ItemInfo").gameObject, "info");
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

    //�κ��丮�� ������ �ֱ�
    public void Input(ItemObject item)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.Input(item))
            {
                Debug.Log("�κ��丮�� ������ ��");
                return;
            }
            else
            {
                continue;
            }
        }
    }
}
