using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
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

}
