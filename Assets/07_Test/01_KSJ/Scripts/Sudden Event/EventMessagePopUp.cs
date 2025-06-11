using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventMessagePopUp : MonoBehaviour
{
    public TextMeshProUGUI description;
    
    public Image Thumbnail;
    
    public SuddenEventObject suddenEventObject;
    
    public GameObject Background;

    public TextMeshProUGUI buttonText;
    
    public Button button;

    public TextMeshProUGUI aDbuttonText;
    
    public Button aDButton;
    
    public RewardPopUp rewardPopUp;
    
    public Action popUpAction;

    public void ShowPopUp(SuddenEventObject _suddenEventObject)
    {
        suddenEventObject = _suddenEventObject;
        Background.SetActive(true);
        gameObject.SetActive(true);
        
        popUpAction?.Invoke();
    }
    
    public void SetButtonText(string message)
    {
        buttonText.text = message;
    }
    
    public void SetADButtonText(string message)
    {
        aDbuttonText.text = message;
    }

    public void SetImage(Sprite sprite)
    {
        Thumbnail.sprite = sprite;
    }
    
    public void SetDescription(string message, string percent = "")
    {
        description.text = message + percent;
    }
    
    public void ShowRewardPopUp()
    {
        rewardPopUp.Init(this);
        gameObject.SetActive(false);
    }
    
    public void GetReward()
    {
        Background.SetActive(false);
        gameObject.SetActive(false);
        
        suddenEventObject.EventOut();
    }
    
    public IEnumerator TextByTime(string message)
    {
        for(int i = 0; i< message.Length; i++)
        {
            description.text += message[i];
            yield return new WaitForSeconds(0.1f);
        }
    }
}
