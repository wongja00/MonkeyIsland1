using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        Application.runInBackground = true;
        Application.targetFrameRate = 60; 
        DataManager.Dataload();
    }
    private void OnDestroy()
    {
        //DataManager.DataSave();
    }
}

public class PermissionRequester : MonoBehaviour
{
    void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
#endif
    }
}