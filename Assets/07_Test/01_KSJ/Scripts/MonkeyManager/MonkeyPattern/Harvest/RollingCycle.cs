using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RollingCycle : MonoBehaviour
{
    [SerializeField]
    List<Vector3> rollingCycleWayPoint = new List<Vector3>();
    
    [SerializeField]
    private Animator Ready;
    
    [SerializeField]
    private Animator Rolling;
    
    [SerializeField]
    private Animator Jump;
    
    [SerializeField]
    private Animator Hit;
    
    [SerializeField]
    private Animator KnockBack;
    
    [SerializeField]
    private Animator Land;



    //9개의 웨이포인트를 순서대로 저장
    public List<Vector3> RollingCycleWayPoint
    {
        get => rollingCycleWayPoint;
        set => rollingCycleWayPoint = value;
    }

    [SerializeField]
    private GameObject rollingCycleWaypointGameObject;
    
    //웨이포인트의 개수
    public int WayPointCount
    {
        get => rollingCycleWayPoint.Count;
    }
    
    //웨이포인트의 위치를 반환
    public Vector3 GetWayPointPosition(int index)
    {
        return rollingCycleWayPoint[index];
    }
    
    HarvestMonkey harvest;
    private int currentWaypointIndex = 0;
    private float speed = 1f;
    
    private float jumpDelay = 0.2f;
    private float curJumpDelay = 0f;
    
    public void Init(HarvestMonkey _harvest)
    {
        harvest = _harvest;
        
        //자식 오브젝트에 있는 웨이포인트를 리스트에 저장
        for (int i = 0; i < rollingCycleWaypointGameObject.transform.childCount; i++)
        {
            rollingCycleWayPoint.Add(rollingCycleWaypointGameObject.transform.GetChild(i).position);
        }
    }
    
    //dotween을 이용하여 웨이포인트를 순서대로 이동
    public void RollingCycleMove(float speed)
    {
        this.speed = (speed/2) * harvest.MonCode;
        currentWaypointIndex = 0;
        StartCoroutine(MoveToNextWaypoint());
    }
    
    private IEnumerator MoveToNextWaypoint()
    {
        while (currentWaypointIndex <= rollingCycleWayPoint.Count)
        {
            if (currentWaypointIndex == rollingCycleWayPoint.Count)
            {
                OnWaypointChange(currentWaypointIndex);
                break;
            }
            
            Vector3 targetPosition = rollingCycleWayPoint[currentWaypointIndex];
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            OnWaypointChange(currentWaypointIndex);
            currentWaypointIndex++;
        }
        
        OnWaypointChange(currentWaypointIndex);
        currentWaypointIndex = 0;
        //StartCoroutine(MoveToNextWaypoint());
    }
    
    void OnWaypointChange(int waypointIndex)
    {
        if (waypointIndex < 0) return;
        
        if (waypointIndex == 0)
        {
            Ready.gameObject.SetActive(true);
            speed = 1;
            harvest.isRolling = true;
            harvest.animator.SetBool("IsRoll", true);
        }
        
        if (waypointIndex == 1)
        {
            Ready.gameObject.SetActive(false);
            Rolling.gameObject.SetActive(true);
            speed = 4;
            harvest.animator.SetFloat("Speed", 2);
        }
    
        if (waypointIndex == 2)
        {
            Rolling.gameObject.SetActive(false);
            Jump.gameObject.SetActive(true);
            speed = 1;
            
        }
        
        if (waypointIndex == 3)
        {
            harvest.animator.SetBool("IsJump", true);
            harvest.animator.SetBool("IsRoll", false);
            harvest.animator.SetFloat("Speed", 0);
            speed = 2;
        }

        if (waypointIndex == 4 )
        {
            Jump.gameObject.SetActive(false);
            Hit.gameObject.SetActive(true);
            harvest.animator.SetBool("IsHit", true);
            harvest.animator.SetBool("IsJump", false);
        }
    
        if (waypointIndex == 5 )
        {
            Hit.gameObject.SetActive(false);
            KnockBack.gameObject.SetActive(true);
            harvest.animator.SetBool("IsFly", true);
            harvest.animator.SetBool("IsHit", false);
        }
    
        if (waypointIndex == 6 )
        {
            KnockBack.gameObject.SetActive(false);
            Land.gameObject.SetActive(true);
            harvest.animator.SetBool("IsDown", true);
            harvest.animator.SetBool("IsFly", false);
        }
    
        if (waypointIndex == 7 )
        {
            Land.gameObject.SetActive(false);
            speed = 0.5f;
            harvest.animator.SetBool("IsStun", true);
            harvest.animator.SetBool("IsDown", false);

        }
    
        if (waypointIndex == 8 )
        {
            harvest.isRolling = false;
            harvest.isHarvest = false;
            harvest.animator.SetBool("IsRoll", true);
            harvest.animator.SetBool("IsStun", false);
        }
    }
    
}
