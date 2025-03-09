using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public enum Stat 
{
    HP,
    Dash
}
public class Player : MonoBehaviour
{
    [ShowInInspector, ReadOnly]
    public float MaxHP { get; private set; }
    
    [ShowInInspector, ReadOnly]
    public float HP { get; private set; }
    
    [ShowInInspector, ReadOnly]
    public float MaxDash { get; private set; }
    
    [ShowInInspector, ReadOnly]
    public float Dash { get; private set; }

    private void Awake()
    {
        MaxHP = HP = 100;
        MaxDash = Dash = 100;
    }

    public void Heal(Stat stat, float value)
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

    public void Damage(Stat stat, float value)
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
