using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickUpMonkey : MonkeyParent
{
    public List<Vector3> BananaPoint;

    public List<Animator> EffectAnimators;
    
    // Start is called before the first frame update
    void Start()
    {
        kind = MonkeyKind.PickUp;
        
        state = MonkeyState.Idle;
        
        isCarryingBanana = false;
    

    }
    
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case MonkeyState.Idle:
                //이벤트 마크가 이벤트라면 리턴
                if (eventMark.isEvent)
                {
                    animator.SetBool("isIdle", true);
                    return;
                }
                
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
                animator.SetBool("isIdle", false);
                 // 타겟으로 이동
                 StorageChest _target = isCarryingBanana ? targetStorage : targetBananaPile;
                 MonkeyState nextState = isCarryingBanana ? MonkeyState.Transporting : MonkeyState.Collecting;
                
                 if (IsAtTargetStorage(_target))
                 {
                     state = nextState;
                 }
                 break;
            case MonkeyState.Collecting:
                // 바나나 더미에서 바나나 수집
                transform.DOKill();
                
                //애니메이션이 끝나면 이벤트 마크를 트루로 바꾸고 다시 아이들로 돌아가게 한다.
                if (!IsAnimationPlaying(EffectAnimators[0], "Banana_Ani"))
                {
                    StartCoroutine(AnimationCoroutine(EffectAnimators[0], "Banana_Ani"));
                }
                
                break;
            case MonkeyState.Transporting:
                // 임시저장고에바나나 추가
                
                    isCarryingBanana = false;
                
                animator.SetBool("IsCarry", isCarryingBanana);
                
                state = MonkeyState.Idle;
                break;
        }
    }
    
    bool IsAnimationPlaying(Animator animator, string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1.0f;
    }
    
    private IEnumerator AnimationCoroutine(Animator animator, string animationName)
    {
        this.animator.SetBool("isIdle", true);
        animator.gameObject.SetActive(true);
        animator.Play(animationName);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        animator.gameObject.SetActive(false);
        isCarryingBanana = true;
        this.animator.SetBool("isIdle", false);
        this.animator.SetBool("IsCarry", isCarryingBanana);    
        state = MonkeyState.Idle;
    }

}