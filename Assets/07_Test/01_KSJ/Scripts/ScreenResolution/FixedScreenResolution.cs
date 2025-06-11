using System;
using UnityEngine;
using System.Collections.Generic;
 
public class FixedScreenResolution : MonoBehaviour
{
    #region Pola
    //private int ScreenSizeX = 0;
    //private int ScreenSizeY = 0;
    private Camera _camera;
    #endregion
 
    #region method

    #region rescale camera
    private void RescaleCamera()
    {
        float targetAspect = 16.0f / 9.0f; // 원하는 가로세로 비율
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Rect rect = _camera.rect;

        if (scaleHeight < 1.0f)
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        _camera.rect = rect;
    }
    #endregion
 
    #endregion
 
    #region method unity
 
    void OnPreCull()
    {
        if (Application.isEditor) return;
        Rect wp = _camera.rect;
        Rect nr = new Rect(0, 0, 1, 1);
 
        _camera.rect = nr;
        GL.Clear(true, true, Color.black);
       
        _camera.rect = wp;
 
    }
 
    // Use this for initialization
    void Awake () {
        _camera = GetComponent<Camera>();
        RescaleCamera();
    }
   
    // Update is called once per frame
    void Update () {
        RescaleCamera();
    }
    #endregion
}