using UnityEngine;

[CreateAssetMenu]
public class ItemInfo : ScriptableObject
{
    public string _name;
    public string description;

    public Stat stat;
    public int value;
}
