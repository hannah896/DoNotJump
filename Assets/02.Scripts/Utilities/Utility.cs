using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class Utility
{
    //Ž���Ϸ��� ������Ʈ, ã�����ϴ� �� �̸�
    public static T FindComponent<T>(GameObject gameObject , string name)  where T : Component
    {
        name ??= typeof(T).Name; //������Ʈ(T) �̸� �ޱ�

        foreach (var component in gameObject.GetComponentsInChildren<T>(true))
        {
            if (component.name.Equals(name))
            {
                return component;
            }
        }

        Debug.Log("�̰� �����");
        return null;
    }
}
