using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        CharacterManager.Instance.Condition = this;
        player = CharacterManager.Instance.Player;
    }
    public void FixedUpdate()
    {
        //if (player.Dash <= player.MaxDash)
        //{
        //    Restore();
        //}
        //else
        //{
        //    StopCoroutine();
        //}
    }

    public void SetStat(Stat stat, int value)
    {
        if (value < 0)
        {
            player.Damage(stat, Mathf.Abs(value));
        }
        else
        {
            player.Heal(stat, value);
        }
    }

    public void UseItem(ItemInfo item)
    {
        SetStat(item.stat, item.value);
        Debug.Log("³Ä¹Ö");
    }

    //IEnumerator Restore()
    //{
    //    yield return new WaitForSeconds(3f);
    //    player.Dash += -
    //}
}
