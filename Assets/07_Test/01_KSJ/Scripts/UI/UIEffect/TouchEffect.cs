using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem touchEffect;

    [SerializeField]
    private Camera mainCamera;
    
    void Update()
    {
#if UNITY_ANDROID
        // 안드로이드 터치 감지
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    ActivateEffect(touch.position);
                }
            }
        }
#elif UNITY_EDITOR || UNITY_STANDALONE || PLATFORM_STANDALONE_WIN
        // 윈도우 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            ActivateEffect(Input.mousePosition);
        }
#endif
    }

    private void ActivateEffect(Vector3 position)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, mainCamera.nearClipPlane));
        touchEffect.transform.position = worldPosition;
        touchEffect.Play();
    }
}
