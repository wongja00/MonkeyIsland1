using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

//보물상자 부수기 매니저
public class CrashBoxManager : MonoBehaviour
{
    public static CrashBoxManager instance;

    private float time = 60f;
    
    [SerializeField] 
    private CrashBox crashBox;
    
    //미니게임을 할동안 꺼버릴 월드 오브젝트
    [SerializeField]
    private Transform worldObjects;
    
    [SerializeField]
    private TextMeshProUGUI timeText;
    
    [SerializeField]
    private AttackUnit attackUnit;
    
    private Coroutine timeCoroutine;
    
    [SerializeField]
    private GameObject rewardUI;
    
    [SerializeField]
    private TextMeshProUGUI bananaRewardText;
    
    [SerializeField]
    private TextMeshProUGUI goldRewardText;
    
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //StartMiniGame();
    }

    private void InitMiniGame()
    {
        crashBox.Init();
        attackUnit.Init();
        MiniGameManager.instance.MiniGameStart();
    }
    
    public void GetReward(double _damage)
    {
        //보상
        ShopManager.UsePaymentCount(PaymentType.Banana, BananaManager.instance.BananaperSec * 600 * _damage);
        ShopManager.UsePaymentCount(PaymentType.Gold, MonkeyDollorManager.instance.MonkeyDollorperSec * 600 * _damage);
        
        rewardUI.SetActive(true);
        bananaRewardText.text = (BananaManager.instance.BananaperSec * 600 * _damage).ToString();
        goldRewardText.text = (MonkeyDollorManager.instance.MonkeyDollorperSec * 600 * _damage).ToString();
        
        EndMiniGame();
    }
    
    public void StartMiniGame()
    {
        InitMiniGame();
        Camera.main.transform.position = new Vector3(21, Camera.main.transform.position.y, Camera.main.transform.position.z);
        timeCoroutine = StartCoroutine(TimeCoroutine());
        
    }
    
    public void EndMiniGame()
    {
        //worldObjects.gameObject.SetActive(true);
        MiniGameManager.instance.MiniGameEnd();
    }
    
    private IEnumerator TimeCoroutine()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1f);
            time -= 1;
            timeText.text = time.ToString();
        }
        TimeOut();
    }

    public void TimeOut()
    {
        GetReward(crashBox.GetCurHPbyPercent());
    }
    
    public void CrashBoxClick()
    {
        StopCoroutine(timeCoroutine);
    }
    
    
}
