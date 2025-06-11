using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds;
using UnityEngine;
using UnityEngine.Events;

public class ADManager: MonoBehaviour
{
    public static ADManager instance;
    
    private RewardedAd rewardedAd;

    //테스트 광고
    
    private string AndId = "ca-app-pub-3940256099942544/5224354917";

    //private string IphoneID = "ca-app-pub-3940256099942544/1712485313";

    private AdRequest _AdRequest;

    public Action OnAdView;
    
    public int adCount = 0;
    
    public UnityEvent OnAdComplete;
    
    void Awake()
    {
        instance = this;
        
        MobileAds.Initialize(initStatus => { });
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void LoadRewardedAd()
    {
        // 있으며 지움
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("광고 불러오기.");

        // 광고 불러오기
        var adRequest = new AdRequest();

        // 광고 요청
        RewardedAd.Load(AndId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                //에러가 있거나 광고가 없을 경우
                if (error != null || ad == null)
                {
                    Debug.LogError("광고 불러오기 실패" +
                                   "실패 사유: " + error);
                    return;
                }

                Debug.Log("광고: "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                
                RegisterEventHandlers(rewardedAd);
                
                ShowRewardedAd();
                adCount++;
            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "광고 보상. 종류: {0}, 수량: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: 보상 처리
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                
            
                OnAdView?.Invoke();
                adCount++;
            });
        }
    }
    
    private void RegisterEventHandlers(RewardedAd ad)
    {
        //광고료
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("광고비 {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            
        };
        
        // 광고가 표시되었을 때
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("광고 뜸.");
        };
        
        // 광고가 클릭되었을 때
        ad.OnAdClicked += () =>
        {
            Debug.Log("광고 클릭");
            
        };
        
        // 광고가 전체 화면 
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("풀스크린 광고");
        };
        
        // 광고가 전체 화면 콘텐츠를 닫았을 때
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("전체 화면 광고 닫음");
            rewardedAd.Destroy();
        };
        
        // 광고가 전체 화면 콘텐츠를 열지 못했을 때
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError(" 광고 실패 " + "에러 : " + error);
            rewardedAd.Destroy();
        };
    }
    
}
