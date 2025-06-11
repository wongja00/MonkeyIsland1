using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum MiniGameType
{
    CrashBox,
    Adventure,
    Stock,
}

public enum MiniGameRewardType
{
    Gold,
    Banana,
    Diamond,
}

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;

    //게임 시작 버튼
    [SerializeField]
    private Button miniGameButton;
    
    //입장권
    private int ticket; 
    
    //공격 게이지
    [SerializeField]
    private BoxAttackGauge boxAttackGauge;
    
    public Action OnMiniGameStart;
    public Action<MiniGameRewardType, int> OnMiniGameEnd;
    
    //일반 UI
    [SerializeField]
    private GameObject UI;
    
    [SerializeField]
    private GameObject crashBoxUI;
    
    [SerializeField]
    private GameObject miniGame;
    
    [SerializeField]
    private CameraMove cameraMove;
    
    private Camera mainCamera;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        mainCamera = Camera.main;
    }
    
    private void Start()
    {
        ticket = 99;
    }
    
    public void EnterMiniGame()
    {
        if (ticket <= 0)
        {
            Debug.Log("입장권이 부족합니다.");
            return;
        }
        ticket--;
        
        OnMiniGameStart?.Invoke();
    }

    public void MiniGameStart(bool uiActive = false)
    {
        UI.SetActive(uiActive);
        cameraMove.enabled = false;
    }
    
    public void MiniGameEnd()
    {
        UI.SetActive(true);
        crashBoxUI.SetActive(false);
        miniGame.SetActive(false);
        mainCamera.transform.position = new Vector3(0, 0, -5);
        cameraMove.enabled = true;
    }
}
