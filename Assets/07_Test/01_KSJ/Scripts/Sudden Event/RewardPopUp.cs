using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class RewardPopUp : MonoBehaviour
{
    public Button rewardPopUpButton;
    
    public  TextMeshProUGUI rewardPopUpButtonText;
    
    public  TextMeshProUGUI desc;

    public EventMessagePopUp eventMessagePopUp;

    public string message;
    
    public string successMessage;
    
    public UnityEvent OnGetReward;
    
    private void Start()
    {
        rewardPopUpButton.onClick.AddListener(GetReward);
        
        message = "두근.......\n두근.......\n두근.......";
    }

    public void Init(EventMessagePopUp _eventMessagePopUp)
    {
        eventMessagePopUp = _eventMessagePopUp;
        
        ShowRewardPopUp();
    }
    
    public void ShowRewardPopUp()
    {
        gameObject.SetActive(true);
    }
    
    public void SetRewardPopUpText(string message)
    {
        desc.text = message;
    }

    public void GetReward()
    {
        gameObject.SetActive(false);
        
        eventMessagePopUp.GetReward();
        OnGetReward?.Invoke();
    }
    
    public IEnumerator TextByTime()
    {
        rewardPopUpButton.gameObject.SetActive(false);
        
        desc.text = "";
        
        for(int i = 0; i< message.Length; i++)
        {
            desc.text += message[i];
            yield return new WaitForSeconds(0.1f);
        }
        
        desc.text = "";
        desc.text = successMessage;
        rewardPopUpButton.gameObject.SetActive(true);
    }
}
