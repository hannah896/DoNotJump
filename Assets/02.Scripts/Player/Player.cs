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
    public float MaxHP { get {  return MaxHP; } private set { value = MaxHP; } }
    public float HP { get { return HP; } private set { HP = value; } }
    public float MaxDash { get { return MaxDash; } private set { MaxDash = value; } }
    public float Dash { get { return Dash; } private set { Dash = value; } }

    private void Awake()
    {
        MaxHP = HP = 100;
        MaxDash = Dash = 100;
    }

    private void Start()
    {
        CharacterManager.Instance.Player = this;
    }

    public void Heal(Stat stat, int value)
    {
        switch (stat) 
        {
            case Stat.HP:
                HP += value;
                HP = Mathf.Min(HP, MaxHP);
                break;

            case Stat.Dash:
                Dash += value;
                Dash = Mathf.Min(Dash, MaxDash);
                break;
        }
    }

    public void Damage(Stat stat, int value)
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
