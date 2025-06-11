using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LogMonkey : MonkeyParent
{
    enum LogState
    {
        Walk,
        Roll,
        Jump,
        Run,
        TakeOut,
    }
    
    [SerializeField] 
    private Transform wayPoint;
    
    [SerializeField] 
    private List<Vector3> wayPoints = new List<Vector3>();
    
    private int currentWaypointIndex = 0;

    [SerializeField] 
    private GameObject rollingObject;
    private Vector3 rollingObjectPosition;

    private SpriteRenderer rollingSpriteRenderer;
    
    [SerializeField]
    private List<ParticleSystem> effects = new List<ParticleSystem>();
    private ParticleSystem curEffect;
    
    private float speed;
    
    LogState Lstate = LogState.Walk;

    private int WalkID;
    private int RollID;
    private int JumpID;
    private int RunID;
    private int TakeOutID;
    
    private void Start()
    {
        
        for(int i =0; i< wayPoint.childCount ; i++)
        {
            wayPoints.Add(wayPoint.GetChild(i).position);
        }
        
        transform.position = wayPoints[0];
        
        rollingSpriteRenderer = rollingObject.GetComponent<SpriteRenderer>();
        
        rollingSpriteRenderer.enabled = false;
        
        rollingObjectPosition = rollingObject.transform.localPosition;
        
        Lstate = LogState.Walk;
        
        WalkID = Animator.StringToHash("isWalk");
        RollID = Animator.StringToHash("isRoll");
        RunID = Animator.StringToHash("isRun");
        TakeOutID = Animator.StringToHash("TakeOut");
        JumpID = Animator.StringToHash("isJump");
        
        speed = 1f;
        animator.SetBool(WalkID, true);
        
        curEffect = effects[MonCode-4];

        FindBananaTree();
    }

    void Update()
    {
        switch(Lstate)
        {
            case LogState.Walk://걷기
                speed = 1f;
                transform.position = Vector3.MoveTowards(transform.position, wayPoints[1], speed * Time.deltaTime);

                if (closeToWaypoint(1))
                {
                    rollingObject.transform.parent = transform;
                    rollingObject.transform.localPosition = rollingObjectPosition;
                    animator.SetBool(WalkID, false);
                    animator.SetBool(RollID, true);
                    Lstate = LogState.Roll;
                }
                
                break;
            case LogState.Roll: //날리기!

                //애니메이션이 끝나면 다음 상태로 변경
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("B")) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool(RollID, false);
                    animator.SetBool(RunID, true);
                    rollingObject.transform.parent = null;
                    transform.position = wayPoints[4];
                    Lstate = LogState.Run;
                }

                break;
            case LogState.Run:
                
                speed = 2f;
                spriteRenderer.flipX = transform.position.x < wayPoints[5].x;
                transform.position = Vector3.MoveTowards(transform.position, wayPoints[5], speed * Time.deltaTime);
                
                if(closeToWaypoint(5))
                {
                    spriteRenderer.flipX = true;
                    animator.SetBool(RunID, false);
                    animator.SetBool(TakeOutID, true);
                    Lstate = LogState.TakeOut;
                    
                }
                
                break;
            
            case LogState.TakeOut:

                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("E")) &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f)
                {
                    transform.position = wayPoints[2];
                }
                
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("E")) &&  animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool(TakeOutID, false);
                    animator.SetBool(JumpID, true);
                    Lstate = LogState.Jump;
                }
                break;
            
            case LogState.Jump:
                
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("F")) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool(JumpID, false);
                    animator.SetBool(WalkID, true);
                    Lstate = LogState.Walk;
                }
                
                break;
        }
    }
    
    private bool closeToWaypoint(int index)
    {
        return Vector3.Distance(transform.position, wayPoints[index]) < 0.01f;
    }

    public void shot()
    {
        rollingSpriteRenderer.enabled = true;
        
        StartCoroutine(RollingObjectMove());
    }
    
    private IEnumerator RollingObjectMove()
    {
        while (Vector3.Distance(rollingObject.transform.position, wayPoints[3]) > 0.01f)
        {
            rollingObject.transform.position = Vector3.MoveTowards(rollingObject.transform.position, wayPoints[3], 2.0f * Time.deltaTime);
            yield return null;
        }
        rollingSpriteRenderer.enabled = false;
        curEffect.Play();
        targetTree.HarvestBananas();
    }
    
}
