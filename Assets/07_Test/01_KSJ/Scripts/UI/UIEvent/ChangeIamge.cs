using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeIamge : MonoBehaviour
{
    public Image iCconImage;
    
    public Sprite chatImage;
    public Sprite rewardImage;
    
    public EventMessagePopUp eventMessagePopUp;

    private void Start()
    {
        eventMessagePopUp.popUpAction += () =>
        {
            ChangeIamgeByEventType((eventMessagePopUp.suddenEventObject as MonkeyIdleEvent)._eventType);
        };
    }

    public void ChangeIamgeByEventType(EventType type)
    {
        switch (type)
        {
            case EventType.Talk:
                iCconImage.sprite = chatImage;
                break;
            case EventType.Reward:
                iCconImage.sprite = rewardImage;
                break;
        }
    }
    
}
