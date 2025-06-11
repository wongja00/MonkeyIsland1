using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeControll : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    
    [SerializeField]
    private TextMeshProUGUI timeSpeedText;
    
    //�����̵忡 ���� �ð��� �帣�� �ӵ�
    public float timeSpeed = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        
        UpdateTimeSpeed(1);
    }
    
    private void OnSliderValueChanged(float value)
    {
        UpdateTimeSpeed(value);
    }
    
    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeSpeed;
    }
    
    public void SetTimeSpeed(float speed)
    {
        UpdateTimeSpeed(speed);
    }

    public void Pause()
    {
        UpdateTimeSpeed(0);
    }

    public void Play()
    {
        UpdateTimeSpeed(1);
    }

    private void UpdateTimeSpeed(float speed)
    {
        timeSpeed = speed;
        slider.value = speed;
        timeSpeedText.text = timeSpeed.ToString("F0") + "���";
    }
}
