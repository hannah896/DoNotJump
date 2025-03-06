using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat 
{
    HP,
    Dash
}
public class Player : MonoBehaviour
{
    public float maxHP;
    public float HP;
    public float maxDash;
    public float Dash;

    private void Awake()
    {
        maxHP = HP = 100;
        maxDash = Dash = 100;
    }

    private void Start()
    {
        CharacterManager.Instance.Player = this;
    }

    void Heal(Stat stat, int value)
    {
        switch (stat) 
        {
            case Stat.HP:
                HP += value;
                HP = Mathf.Min(HP, maxHP);
                break;

            case Stat.Dash:
                Dash += value;
                Dash = Mathf.Min(Dash, maxDash);
                break;
        }
    }

    void Damage(Stat stat, int value)
    {
        switch (stat)
        {
            case Stat.HP:
                HP -= value;
                HP = Mathf.Max(HP, 0);
                break;
            case Stat.Dash:
                Dash -= value;
                Dash = Mathf.Max(Dash, 0);
                break;
        }
    }
}
