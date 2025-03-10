using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Choco,
    Straw
}
public class ItemObject : MonoBehaviour
{
    public ItemInfo itemInfo;
    ItemType type;

    GameObject cake;

    public void Buff()
    {
        Debug.Log("아이템버프!!");
        HoldingCupCake(itemInfo.time);
        StartCoroutine(CharacterManager.Instance.Condition.Buff(itemInfo.stat, itemInfo.time, itemInfo.value));
    }

    IEnumerator HoldingCupCake(float time)
    {
        switch (type) 
        {
            case ItemType.Choco:
                cake = CharacterManager.Instance.Choco;
                break;
            case ItemType.Straw:
                cake = CharacterManager.Instance.Straw;
                break;
        }

        cake.SetActive(true);
        yield return new WaitForSeconds(time);
        cake.SetActive(false);
    }
}
