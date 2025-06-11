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

    //�׽�Ʈ ����
    
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
        // ������ ����
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("���� �ҷ�����.");

        // ���� �ҷ�����
        var adRequest = new AdRequest();

        // ���� ��û
        RewardedAd.Load(AndId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                //������ �ְų� ���� ���� ���
                if (error != null || ad == null)
                {
                    Debug.LogError("���� �ҷ����� ����" +
                                   "���� ����: " + error);
                    return;
                }

                Debug.Log("����: "
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
            "���� ����. ����: {0}, ����: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: ���� ó��
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                
            
                OnAdView?.Invoke();
                adCount++;
            });
        }
    }
    
    private void RegisterEventHandlers(RewardedAd ad)
    {
        //�����
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("����� {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            
        };
        
        // ���� ǥ�õǾ��� ��
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("���� ��.");
        };
        
        // ���� Ŭ���Ǿ��� ��
        ad.OnAdClicked += () =>
        {
            Debug.Log("���� Ŭ��");
            
        };
        
        // ���� ��ü ȭ�� 
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Ǯ��ũ�� ����");
        };
        
        // ���� ��ü ȭ�� �������� �ݾ��� ��
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("��ü ȭ�� ���� ����");
            rewardedAd.Destroy();
        };
        
        // ���� ��ü ȭ�� �������� ���� ������ ��
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError(" ���� ���� " + "���� : " + error);
            rewardedAd.Destroy();
        };
    }
    
}
