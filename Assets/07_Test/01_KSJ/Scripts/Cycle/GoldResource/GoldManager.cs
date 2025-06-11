using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//싱글톤으로 골드를 관리하는 클래스

public class GoldManager : MonoBehaviour
{
    private static GoldManager instance;

    public static GoldManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GoldManager>();
                
                if (instance == null)
                {
                    GameObject container = new GameObject("GoldManager");
                    instance = container.AddComponent<GoldManager>();
                }
            }
            
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }
    
    private void OnDestroy()
    {
        instance = null;
    }
    
    private void OnApplicationQuit()
    {
        instance = null;
    }
}
