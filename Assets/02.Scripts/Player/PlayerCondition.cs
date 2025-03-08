using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    private void Start()
    {
        CharacterManager.Instance.Condition = this;
    }
    public void SetStat(Stat stat, int value)
    {
        if (value < 0)
        {
            CharacterManager.Instance.Player.Damage(stat, value);
        }
        else
        {
            CharacterManager.Instance.Player.Heal(stat, value);
        }
    }

    public void UseItem(ItemInfo item)
    {
        SetStat(item.stat, item.value);
        Debug.Log("³Ä¹Ö");
    }
}
