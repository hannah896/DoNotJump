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
            Debug.Log("코루틴 시작!");
            curCoroutin = StartCoroutine(Restore());
        }
        else
        {
            if (curCoroutin != null)
            {
                Debug.Log("코루틴 끝!");
                StopCoroutine(curCoroutin);
                curCoroutin = null;
            }
            Debug.Log("코루틴 없음");
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
}
