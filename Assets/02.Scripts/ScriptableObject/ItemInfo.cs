using UnityEngine;

[CreateAssetMenu (fileName = "ItemInfo")]
public class ItemInfo : ScriptableObject
{
    //이름과 설명, 아이콘, 갯수
    public string _name;
    public string description;
    public Sprite Icon;

    //효과
    public Stat stat;

    //초당 회복량 
    public float value;
    public float time;
}
