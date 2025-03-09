using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //아이템 슬롯 프리펩
    public ItemSlot itemSlot;
    //인벤토리 슬롯 배열
    public ItemSlot[] inventorySlots;
    //선택한 아이템 슬롯
    public ItemSlot SelectSlot;

    private void OnValidate()
    {
        
        itemSlot = (ItemSlot)AssetDatabase.LoadAssetAtPath("Assets/03.Prefabs/Inventory/Slot.prefab", typeof(ItemSlot));
        Transform parent = transform.Find("Slots");
        ItemSlot slot;

        // TODO : 아이템슬롯 예외처리 해주기
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
        //비활성화시키기
        gameObject.SetActive(false);
    }

    public void SelectedSlot()
    {
        
    }
}
