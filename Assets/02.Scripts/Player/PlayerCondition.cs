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
            Debug.Log("�ڷ�ƾ ����!");
            curCoroutin = StartCoroutine(Restore());
        }
        else
        {
            if (curCoroutin != null)
            {
                Debug.Log("�ڷ�ƾ ��!");
                StopCoroutine(curCoroutin);
                curCoroutin = null;
            }
            Debug.Log("�ڷ�ƾ ����");
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
}
