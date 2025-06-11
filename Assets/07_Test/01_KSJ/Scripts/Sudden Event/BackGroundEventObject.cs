using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundEventObject : MonoBehaviour, ClickableObject, SuddenEventObject
{
    public bool isEvent { get; set; }

    public string _name;
    
    public Animator animator;

    public string chatString;
    
    public BoxCollider2D boxCollider2D;
    
    public EnvironmentMaster WindowMaster;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Init(EnvironmentMaster _WindowMaster)
    {
        WindowMaster = _WindowMaster;
        
        //animator.Play(name);
        isEvent = true;
    }
    
    public void OnClick()
    {
        BackGroundEventManager.instance.OnEventInteract(this);
    }
    
    public void OnEvent()
    {
        gameObject.SetActive(true);
        isEvent = true;
    }
    
    public void EventOut()
    {
        gameObject.SetActive(false);
        isEvent = false;
    }
}
