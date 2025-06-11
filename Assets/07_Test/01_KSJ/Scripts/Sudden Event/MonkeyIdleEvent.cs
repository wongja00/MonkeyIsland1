using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonkeyIdleEvent : MonoBehaviour, ClickableObject, SuddenEventObject
{
    public bool isEvent { get; set; }

    public int eventID = 0;

    public EventType _eventType;

    public EventRewardType _eventRewardType;
    
    public Sprite eventSprite;

    public WindowMaster curWindowMaster;
    
    public StringMaster curStringMaster;
    
    public int rewardType = 0;
    public int rewardValue = 0;
    
    public SpriteRenderer monkeySpriteRenderer;
    
    public BoxCollider2D boxCollider2D;
    
    public void Init(EventType eventtype, EventRewardType _rewardType, RewardMaster rewardMaster)
    {
        isEvent = false;
        _eventType = eventtype;
        _eventRewardType = _rewardType;
        
        rewardType = rewardMaster.Type;
        rewardValue = rewardMaster.Value;
    }

    public void OnClick()
    {
        if (isEvent)
        {
            PopMonkeyEvent();
        }
    }
    
    public void OnEvent()
    {
        
    }

    //¸Ó¸®À§¿¡ ¶ä
    public void OnMonkeyEvent()
    {
        isEvent = true;
        gameObject.SetActive(true);

        Invoke("EventOut", 30f);
        

        Bounds eventBounds = boxCollider2D.bounds;
        eventBounds.center += (Vector3)transform.localPosition;

        Bounds currentSpriteBounds = monkeySpriteRenderer.sprite.bounds;
        currentSpriteBounds.center += (Vector3)monkeySpriteRenderer.transform.localPosition;
        
        Bounds intersectionBounds = currentSpriteBounds;
        intersectionBounds.Encapsulate(eventBounds.min);
        intersectionBounds.Encapsulate(eventBounds.max);

        boxCollider2D.size = intersectionBounds.size;


    }

    public void EventOut()
    {
        isEvent = false;
        gameObject.SetActive(false);
    }

    //Å¬¸¯½Ä ´ëÈ­ ÆË¾÷ ¶ç¿öÁü
    public void PopMonkeyEvent()
    {
        CancelInvoke("EventOut");
        
        MonkeyEventManager.instance.ShowPopUp(this, _eventType, _eventRewardType);
    }
}