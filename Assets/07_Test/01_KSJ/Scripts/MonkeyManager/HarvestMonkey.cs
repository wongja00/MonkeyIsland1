using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;

public class HarvestMonkey : MonkeyParent,IHarvestUnit
{
    private Vector3 originPos;

    private Vector3[] waypoints;
    
    private Vector3[] logWaypoints;
    
    public bool isHarvest;
    
    private RollingCycle rollingCycle;
    
    public Action OnMove;
    public Action OnHarvest;
    
    public List<ParticleSystem> harvestEffect;
    
    [SerializeField]
    private List<Animator> Effectsanimators;
    
    // Start is called before the first frame update
    void Start()
    {
        kind = MonkeyKind.Harvester;
        
        state = MonkeyState.Moving;
        
        FindBananaTree();
        
        isRolling = false;
        
        isHarvest = false;
        
        originPos = transform.position;
        
        // 반의 반원을 정의하기 위해 경로 배열을 생성
        waypoints = new Vector3[]
        {
            originPos, // 현재 위치에서 시작
            new Vector3((originPos.x + targetTree.transform.position.x) / 2, originPos.y, originPos.z), // 중간 지점
            new Vector3(targetTree.transform.position.x - 0.1f, originPos.y + 0.7f, targetTree.transform.position.z), // 나무
            new Vector3((originPos.x + targetTree.transform.position.x) / 2, originPos.y + 1.2f, originPos.z), // 다시 중간 지점
            originPos // 원래 위치로 돌아옴
        };
        
        logWaypoints = new Vector3[]
        {
            originPos, // 현재 위치에서 시작
            new Vector3((3 * originPos.x + targetTree.transform.position.x) / 4, originPos.y, originPos.z), // 중간 지점
            new Vector3(targetTree.transform.position.x - 0.1f, originPos.y, targetTree.transform.position.z), // 나무
            new Vector3((originPos.x + targetTree.transform.position.x) / 2, originPos.y, originPos.z), // 다시 중간 지점
            originPos // 원래 위치로 돌아옴
        };

        if (MonCode == 1 || MonCode == 2 || MonCode == 3)
        {
            rollingCycle = GetComponent<RollingCycle>();
            
            rollingCycle.Init(this);
        }
        
        else if(MonCode == 4 || MonCode == 5 || MonCode == 6)
        {
            waypoints = logWaypoints;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case MonkeyState.Moving:
                // 바나나 나무로 구르기!
                if(!isRolling)
                {
                    if (MonCode == 1 || MonCode == 2 || MonCode == 3)
                    {
                        rollingCycle.RollingCycleMove(Speed);
                    }
                    else if(MonCode == 4 || MonCode == 5 || MonCode == 6)
                    {
                        RollToTree();
                    }
                    
                    isRolling = true;
                }
                
                OnMove?.Invoke();
                break;
                
            case MonkeyState.Harvesting:
                // 바나나 수확
                HarvestBananas();
                
                OnHarvest?.Invoke();
                break;
        }
    }

    void RollToTree()
    {
        Tween t = transform.DOPath(waypoints, Speed/5, PathType.CatmullRom).SetEase(Ease.OutSine);
        
        t.OnWaypointChange(waypointIndex =>
        {
            if (waypointIndex == 1)
            {
                animator.speed = 1;
            }
            
            if (waypointIndex == 2)
            {
                if (!isHarvest)
                {
                    isHarvest = true;
                    state = MonkeyState.Harvesting;
                    
                    harvestEffect[MonCode-4].Play();
                    
                }
            }
            
            if (waypointIndex == 3)
            {
            }

            if (waypointIndex == 4 )
            {
                isRolling = false;
                isHarvest = false;
            }
        });
    }
    
    void HarvestBananas()
    {
        state = MonkeyState.Moving;
        targetTree.HarvestBananas(0);
    }
}
