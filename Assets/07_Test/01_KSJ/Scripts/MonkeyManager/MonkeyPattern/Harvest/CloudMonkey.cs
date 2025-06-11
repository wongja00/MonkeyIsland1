using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CloudMonkey : MonkeyParent
{
    enum CloudState
    {
        Whistle,
        Fly,
        Move,
        Shot,
        Land
    }
    
    [SerializeField]
    private GameObject cloud;
    
    [SerializeField]
    private Transform cloudPos;
    
    [SerializeField]
    private Transform FlyPoint;
    [SerializeField]
    private Transform ShotPoint;
    [SerializeField]
    private Transform LandPoint;
    [SerializeField]
    private Transform PolePoint;
    
    [SerializeField] 
    private GameObject Pole;
    private SpriteRenderer PoleSprite;
    
    Vector3 cloudPosition;
    
    Vector3 FlyPosition;
    
    Vector3 ShotPosition;
    
    Vector3 LandPosition;
    
    Vector3 PolePosition;
    
    Coroutine cloudCoroutine;
    
    Coroutine poleCoroutine;
    
    [SerializeField]
    private CloudState cloudState;

    [SerializeField] private float poleScaleSpeed = 20;
    
    void Start()
    {
        cloudPosition = cloudPos.position;
        FlyPosition = FlyPoint.position;
        ShotPosition = ShotPoint.position;
        LandPosition = LandPoint.position;
        PolePosition = PolePoint.position;
        
        cloud.transform.position = new Vector3(FlyPosition.x -3, FlyPosition.y, FlyPosition.z);
        
        PoleSprite = Pole.GetComponent<SpriteRenderer>();
        
        cloud.transform.SetParent(null);
        Pole.transform.SetParent(null);
        
        Pole.transform.localScale = Vector3.one;
        Pole.SetActive(false);
        cloud.SetActive(false);
        
        cloudState = CloudState.Whistle;
        
        FindBananaTree();
    }
    
    void Update()
    {
        switch (cloudState)
        {
            case CloudState.Whistle:
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("A")) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.Play("B");
                    cloudState = CloudState.Fly;
                    if(cloudCoroutine == null)
                        cloudCoroutine = StartCoroutine(CloudCome());
                }
                break;
            case CloudState.Fly:
                
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("B")) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0d)
                {
                    animator.Play("D");
                    transform.position = FlyPosition;
                    cloudState = CloudState.Move;
                }
                break;
            case CloudState.Move:
                transform.position = Vector3.MoveTowards(transform.position, ShotPosition, 1 * Time.deltaTime);
    
                if (Vector3.Distance(transform.position, ShotPosition) < 0.1f)
                {
                    animator.Play("E");
                    
                    cloudState = CloudState.Shot;
                }
                break;
            case CloudState.Shot:
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("E")) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    if(poleCoroutine == null)
                        poleCoroutine = StartCoroutine(PoleShot());
                }
                break;
            
            case CloudState.Land:
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("F")) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.Play("A");
                    FlyPosition = FlyPoint.position;
                    cloudState = CloudState.Whistle;
                }
                
                break;
        }
    }

    private IEnumerator CloudCome()
    {
        cloud.SetActive(true);
        while (transform.transform.position.x > cloud.transform.position.x)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            cloud.transform.position = new Vector3(cloud.transform.position.x + 5 * Time.deltaTime, cloud.transform.position.y, cloud.transform.position.z);
        }
        cloud.transform.position = new Vector3(FlyPosition.x -3, FlyPosition.y, FlyPosition.z);
        cloud.SetActive(false);
        
        cloudCoroutine = null;
    }

    private IEnumerator PoleShot()
    {
        Pole.SetActive(true);
        
        Pole.transform.position = PolePosition;
        
        while (PoleSprite.bounds.center.x + PoleSprite.bounds.extents.x < targetTree.transform.position.x)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            Pole.transform.localScale = new Vector3(Pole.transform.localScale.x + poleScaleSpeed * Time.deltaTime, Pole.transform.localScale.y + poleScaleSpeed * Time.deltaTime, Pole.transform.localScale.z);
        }
        
        targetTree.HarvestBananas();
        
        Pole.SetActive(false);
        Pole.transform.position = FlyPosition;
        Pole.transform.localScale = Vector3.one;
        animator.Play("F");
        transform.position = LandPosition;
        cloudState = CloudState.Land;
        poleCoroutine = null;
    }

}
