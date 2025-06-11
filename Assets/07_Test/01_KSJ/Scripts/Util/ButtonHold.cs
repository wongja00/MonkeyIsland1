using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//버튼 꾹 누르고 있을때
public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool interactable = true;

    public bool isHold = false;
    public bool isHoldEnd = false;
    public bool isHoldStart = false;
    
    //홀드 기준시간
    public float holdTime = 0.2f;
    //누르고 있는 시간
    public float holdTimer = 0f;
    
    //한번 눌렀을때 실행할 함수
    public UnityEvent OnClick;
    
    //홀드시작시 실행할 함수
    public UnityEvent OnHoldStart;
    
    public UnityEvent OnClickStart;
    
    private Coroutine holdUpdateCheck;
    
    [NonSerialized]
    public float holdValue = 0.1f;
    
    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        AddEventTrigger(trigger, EventTriggerType.PointerDown, (data) => OnPointerDown((PointerEventData)data));
        AddEventTrigger(trigger, EventTriggerType.PointerUp, (data) => OnPointerUp((PointerEventData)data));
        AddEventTrigger(trigger, EventTriggerType.PointerExit, (data) => OnPointerExit((PointerEventData)data));
    }

    void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
    
    private IEnumerator buttonHoldUpdateUpdateCheck()
    {
        while(isHold)
        {
            yield return new WaitForSeconds(holdValue);
            
            holdTimer += 0.1f;
            
            isHoldEnd = false; 
            
            OnHoldStart.Invoke();
            
            if(interactable == false)
                yield return new WaitUntil(()=>interactable);
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if(interactable == false)
            return;
        
        if (!isHoldStart)
        {
            OnClick.Invoke();
        }
        
        isHoldStart = true;
        isHold = true;
        
        if(holdUpdateCheck == null)
            holdUpdateCheck = StartCoroutine(buttonHoldUpdateUpdateCheck());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopHold();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        StopHold();
    }

    private void StopHold()
    {
        isHold = false;
        isHoldEnd = true;
        isHoldStart = false;
        holdTimer = 0.0f;

        if (holdUpdateCheck != null)
        {
            StopCoroutine(holdUpdateCheck);
            holdUpdateCheck = null;
        }
    }
}
