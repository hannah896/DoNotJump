using UnityEngine;

[CreateAssetMenu (fileName = "ItemInfo")]
public class ItemInfo : ScriptableObject
{
    //�̸��� ����, ������, ����
    public string _name;
    public string description;
    public Sprite Icon;

    //ȿ��
    public Stat stat;
    public int value;
}
