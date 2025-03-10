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
        Debug.Log("�Ĺ�");
    }

    IEnumerator Restore()
    {
        Debug.Log("������!!!");
        SetStat(Stat.Dash, 0.1f);
        if (player.Dash >= player.MaxDash)
        {
            Debug.Log("�ص�ǽ��ϴ�");
            yield break;
        }
        else
        {
            while (true)
            {
                Debug.Log("���� ���ִ���~");
                yield return new WaitForSeconds(1f);
                Debug.Log("������!!!2Ʈ");
                SetStat(Stat.Dash, 0.5f);
            }
        }
    }

    public IEnumerator Buff(Stat stat, float time, float value)
    {
        Debug.Log("��������!!!");
        SetStat(stat, value);
        if (time <= 0)
        {
            Debug.Log("�ص�ǽ��ϴ�");
            yield break;
        }
        else
        {
            while (true)
            {
                Debug.Log("���� ���ִ���~");
                yield return new WaitForSeconds(1f);
                time -= 1f;
                Debug.Log("��������!!!2Ʈ");
                SetStat(stat, value);
            }
        }
    }
}
