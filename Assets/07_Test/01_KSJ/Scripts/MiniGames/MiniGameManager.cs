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

    //���� ���� ��ư
    [SerializeField]
    private Button miniGameButton;
    
    //�����
    private int ticket; 
    
    //���� ������
    [SerializeField]
    private BoxAttackGauge boxAttackGauge;
    
    public Action OnMiniGameStart;
    public Action<MiniGameRewardType, int> OnMiniGameEnd;
    
    //�Ϲ� UI
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
            Debug.Log("������� �����մϴ�.");
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
