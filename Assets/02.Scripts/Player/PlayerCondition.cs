using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    private Player player;
    Coroutine curCoroutin;
    
    private void Start()
    {
        player = CharacterManager.Instance.Player;
    }
    public void FixedUpdate()
    {
        if (player.Dash < player.MaxDash)
        {
            if (curCoroutin != null) return;
            curCoroutin = StartCoroutine(Restore());
        }
        else
        {
            if (curCoroutin != null)
            {
                StopCoroutine(curCoroutin);
                curCoroutin = null;
            }
        }
    }

    public void SetStat(Stat stat, float value)
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
        Debug.Log("냐밍");
    }

    IEnumerator Restore()
    {
        Debug.Log("힐해줘!!!");
        SetStat(Stat.Dash, 0.1f);
        if (player.Dash >= player.MaxDash)
        {
            Debug.Log("해드렷습니다");
            yield break;
        }
        else
        {
            while (true)
            {
                Debug.Log("지금 해주는중~");
                yield return new WaitForSeconds(1f);
                Debug.Log("힐해줘!!!2트");
                SetStat(Stat.Dash, 0.5f);
            }
        }
    }
    public void Buff(ItemInfo itemInfo, ItemType type)
    {
        Debug.Log("아이템버프!!");
        StartCoroutine(HoldingCupCake(type, itemInfo.time));
        StartCoroutine(CharacterManager.Instance.Condition.Buf(itemInfo.stat, itemInfo.time, itemInfo.value));
    }

    public IEnumerator Buf(Stat stat, float time, float value)
    {
        Debug.Log("버프해줘!!!");
        SetStat(stat, value);
        while (true)
        {
            Debug.Log("지금 해주는중~");
            yield return new WaitForSeconds(1f);
            time -= 1f;
            Debug.Log("버프해줘!!!2트");
            SetStat(stat, value);

            if (time <= 0)
            {
                Debug.Log("해드렷습니다");
                yield break;
            }
        }
    }

    IEnumerator HoldingCupCake(ItemType type, float time)
    {
        Debug.Log("케이크소환!!");
        CharacterManager.Instance.Active(type);
        yield return new WaitForSeconds(time);
        CharacterManager.Instance.NoActive(type);
    }
}
