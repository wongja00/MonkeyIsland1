using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null) {
                T[] assets = Resources.LoadAll<T>("");
                if(assets == null || assets.Length < 1)
                    throw new System.Exception("컨테이너가 없습니다.");
                else if (assets.Length >1)
                    throw new System.Exception("컨테이너가 2개 이상입니다.");
                instance = assets[0];
            }
            return instance;
        }
    }
}
