using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlantMonkey : MonkeyParent
{
    enum PlantState
    {
        Walk,
        Sommon,
        Shot,
        Disappear
    }
    
    PlantState plantState;
    
    [SerializeField]
    private Animator effectAnimation;

    [SerializeField]
    private Animator projectileAni;
    
    [SerializeField]
    private ParticleSystem effectParticle;

    [SerializeField] 
    private Transform sommonPoint;
    
    [SerializeField] 
    private Transform ridePoint;
    
    [SerializeField] 
    private Transform landPoint;
    
    [SerializeField] 
    private Transform shotPoint;
    
    Vector3 sommonPosition;
    
    Vector3 ridePosition;
    
    Vector3 landPosition;
    
    Vector3 shotPosition;
    
    private bool isEvent = false;

    private float speed = 5;

    private void Start()
    {
        FindBananaTree();
        
        plantState = PlantState.Walk;
        
        sommonPosition = sommonPoint.position;
        ridePosition = ridePoint.position;
        landPosition = landPoint.position;
        shotPosition = shotPoint.localPosition;
        speed = 1f;
        
        projectileAni.gameObject.SetActive(false);
        effectAnimation.gameObject.SetActive(false);
        projectileAni.transform.position = shotPoint.position;

    }

    void Update()
    {
        switch (plantState)
        {
            case PlantState.Walk:
                
                transform.position = Vector3.MoveTowards(transform.position, sommonPosition, speed * Time.deltaTime);
                spriteRenderer.flipX = transform.position.x > sommonPosition.x;               
                
                if(Vector3.Distance(transform.position, sommonPosition) < 0.1f)
                {
                    animator.SetBool("isWalk", false);
                    animator.SetBool("isSommon", true);
                    effectParticle.Play();
                    plantState = PlantState.Sommon;
                }
                
                break;
            
            case PlantState.Sommon:
                spriteRenderer.flipX = false;
                
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("D")) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("isSommon", false);
                    animator.SetBool("isShot", true);
                    
                    transform.position = ridePosition;
                    
                    plantState = PlantState.Shot;
                }
                break;
            
            case PlantState.Shot:
                
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("F")) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("isShot", false);
                    animator.SetBool("isDisappear", true);
                    plantState = PlantState.Disappear;
                }
                break;
            
            case PlantState.Disappear:
                
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("G")) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("isDisappear", false);
                    animator.SetBool("isWalk", true);
                    transform.position = landPosition;
                    plantState = PlantState.Walk;
                }
                break;
        }
    }
    
    private IEnumerator Spit()
    {
        projectileAni.gameObject.SetActive(true);
        
        while(targetTree.transform.position.x > projectileAni.transform.position.x)
        {
            projectileAni.transform.position = new Vector3(projectileAni.transform.position.x + 5 * Time.deltaTime, projectileAni.transform.position.y, projectileAni.transform.position.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        targetTree.HarvestBananas();
        projectileAni.gameObject.SetActive(false);
        
        effectAnimation.transform.position = projectileAni.transform.position;
        effectAnimation.gameObject.SetActive(true);
        StartCoroutine(Splash());
        
        projectileAni.transform.position = shotPoint.position;
    }
    
    public void Shot()
    {
        StartCoroutine(Spit());
    }

    public void splashEnd()
    {
        
    }

    public IEnumerator Splash()
    {
        while(effectAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        effectAnimation.gameObject.SetActive(false);
    }

}
