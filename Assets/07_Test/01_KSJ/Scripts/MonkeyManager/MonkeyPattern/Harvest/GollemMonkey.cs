using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GollemMonkey : MonkeyParent
{
    enum GollemState
    {
        Ready,
        Walk,
        UptoRaeady,
        Attack,
        TakeArm,
        GotoArm
    }

    [SerializeField] private Transform Floatpoint;
    [SerializeField] private Transform Punchpoint;
    [SerializeField] private Transform Hitpoint;
    [SerializeField] private Transform goUpPoint;
    [SerializeField] private Transform backPoint;
    [SerializeField] private Transform armPoint;
    
    private Vector3 floatPosition;
    private Vector3 punchPosition;
    private Vector3 hitPosition;
    private Vector3 originPosition;
    private Vector3 armPosition;
    
    GollemState gollemState;

    void Start()
    {
        FindBananaTree();
        
        originPosition = transform.position;
        floatPosition = Floatpoint.position;
        punchPosition = Punchpoint.position;
        armPosition = armPoint.position;
        
        gollemState = GollemState.Ready;
        transform.position = new Vector3(transform.position.x, Floatpoint.position.y, 0);
    }

    void Update()
    {
        switch (gollemState)
        {
            case GollemState.Ready:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("A") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    gollemState = GollemState.Walk;
                    animator.Play("B");
                    transform.position = new Vector3(transform.position.x, originPosition.y, 0);
                }
                break;
            case GollemState.Walk:
                 transform.position = new Vector3(transform.position.x + Time.deltaTime * 2, transform.position.y, transform.position.z);
                
                if (transform.position.x >= punchPosition.x)
                {
                    gollemState = GollemState.UptoRaeady;
                    animator.Play("F");
                    transform.position = new Vector3(transform.position.x, Floatpoint.position.y, 0);
                }
                break;
            case GollemState.UptoRaeady:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("F") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    gollemState = GollemState.Attack;
                    animator.Play("C");
                    transform.position = new Vector3(Hitpoint.position.x, transform.position.y, transform.position.z);
                }
                break;
            case GollemState.Attack:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("D") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    gollemState = GollemState.TakeArm;
                    animator.Play("E");
                    MoveBack();
                }
                break;
            case GollemState.TakeArm:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("E") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    gollemState = GollemState.GotoArm;
                    animator.Play("B");
                    transform.position = new Vector3(transform.position.x, originPosition.y, 0);
                    spriteRenderer.flipX = false;
                }
                break;
            case GollemState.GotoArm:
                transform.position = new Vector3(transform.position.x - Time.deltaTime * 2, transform.position.y, transform.position.z);
                
                if (transform.position.x <= armPosition.x)
                {
                    gollemState = GollemState.Walk;
                    spriteRenderer.flipX = true;
                }
                break;
        }
    }

    public void MoveFloatPoint()
    {
        transform.position = new Vector3(transform.position.x, Floatpoint.position.y, 0);
    }

    public void Land()
    {
        transform.position = new Vector3(transform.position.x, originPosition.y, transform.position.z);
    }

    public void MoveBack()
    {
        transform.position = new Vector3(backPoint.position.x, transform.position.y, transform.position.z);
    }

    public void Harvest()
    {
        targetTree.HarvestBananas();
    }
    
}
