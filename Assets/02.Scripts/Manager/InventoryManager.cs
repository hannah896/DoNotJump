using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //������ ���� ������
    public ItemSlot itemSlot;
    //�κ��丮 ���� �迭
    public ItemSlot[] inventorySlots;
    //������ ������ ����
    public ItemSlot SelectSlot;

    private void OnValidate()
    {
        
        itemSlot = (ItemSlot)AssetDatabase.LoadAssetAtPath("Assets/03.Prefabs/Inventory/Slot.prefab", typeof(ItemSlot));
        Transform parent = transform.Find("Slots");
        ItemSlot slot;

        // TODO : �����۽��� ����ó�� ���ֱ�
        for (int i = 0; i < 20; i++)
        {
            slot = Instantiate(itemSlot, parent);
            slot.SlotIndex = i;
        }

        if (parent.childCount < 20)
        {
            while (parent.childCount == 20)
            {
                Instantiate(itemSlot, parent);
            }
        }
        else if (parent.childCount > 20)
        {
            while (parent.childCount == 20)
            {
                Destroy(slot);
            }
        }

    }

    private void Awake()
    {
        //��Ȱ��ȭ��Ű��
        gameObject.SetActive(false);
    }

    public void SelectedSlot()
    {
        
    }
}
