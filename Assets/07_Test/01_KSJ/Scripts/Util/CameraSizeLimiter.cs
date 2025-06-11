using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeLimiter : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject shader;
    private float maxWidth = 10f; // Set your desired maximum width here

    void Start()
    {
        maxWidth = shader.transform.localScale.x;
        LimitCameraWidth();
    }

    void Update()
    {
        LimitCameraWidth();
    }

    void LimitCameraWidth()
    {
        float currentWidth = GetCameraWidth(mainCamera);
        if (currentWidth > maxWidth || currentWidth < maxWidth)
        {
            float newOrthographicSize = maxWidth / (2f * mainCamera.aspect);
            mainCamera.orthographicSize = newOrthographicSize;
        }
        
    }

    float GetCameraWidth(Camera camera)
    {
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;
        return width;
    }
}
