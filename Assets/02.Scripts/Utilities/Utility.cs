using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class Utility
{
    //탐색하려는 오브젝트, 찾고자하는 거 이름
    public static T FindComponent<T>(GameObject gameObject , string name)  where T : Component
    {
        name ??= typeof(T).Name; //컴포넌트(T) 이름 받기

        foreach (var component in gameObject.GetComponentsInChildren<T>(true))
        {
            if (component.name.Equals(name))
            {
                return component;
            }
        }

        Debug.Log("이건 곤란해");
        return null;
    }
}
