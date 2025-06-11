using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class CarryingMonkey : MonkeyParent
{
    // Start is called before the first frame update
    void Start()
    {
        kind = MonkeyKind.Carry;
        
        state = MonkeyState.Idle;
        
        isCarryingBanana = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case MonkeyState.Idle:
                //애니메이션 정지
                animator.speed = 0;
                
                //이벤트 마크가 이벤트라면 리턴
                if (eventMark.isEvent)
                {
                    return;
                }
                
                
                
                animator.speed = 1;
                state = MonkeyState.Moving;
                
               //타켓에 바나나가 1개라도 있으면 이동
                if (!isCarryingBanana)
                {
                    MoveTowardsTargetStorage(targetBananaPile);
                }
                else
                {
                    MoveTowardsTargetStorage(targetStorage);
                }
                
                break;
            case MonkeyState.Moving:
                
                 StorageChest _target = isCarryingBanana ? targetStorage : targetBananaPile;
                 MonkeyState nextState = isCarryingBanana ? MonkeyState.Transporting : MonkeyState.Collecting;
                 if (IsAtTargetStorage(_target))
                 {
                     state = nextState;
                 }
                 break;
            case MonkeyState.Collecting:
                    isCarryingBanana = true;
                
                animator.SetBool(isCarryHash, isCarryingBanana);    
                
                state = MonkeyState.Idle;
                break;
            case MonkeyState.Transporting:
                    isCarryingBanana = false;
                
                animator.SetBool(isCarryHash, isCarryingBanana);
                
                state = MonkeyState.Idle;
                break;
        }
    }
}
