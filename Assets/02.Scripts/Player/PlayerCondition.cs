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
        Debug.Log("≥ƒπ÷");
    }

    IEnumerator Restore()
    {
        Debug.Log("»˙«ÿ¡‡!!!");
        SetStat(Stat.Dash, 0.1f);
        if (player.Dash >= player.MaxDash)
        {
            Debug.Log("«ÿµÂ∑«Ω¿¥œ¥Ÿ");
            yield break;
        }
        else
        {
            while (true)
            {
                Debug.Log("¡ˆ±› «ÿ¡÷¥¬¡ﬂ~");
                yield return new WaitForSeconds(1f);
                Debug.Log("»˙«ÿ¡‡!!!2∆Æ");
                SetStat(Stat.Dash, 0.5f);
            }
        }
    }
}
