using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ChallengeCard : MonoBehaviour
{
    [HideInInspector] public Challenge challenge;
    public Image Thumbnail;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI DiaReward;
    public TextMeshProUGUI Progress;
    public TextMeshProUGUI ButtonText;
    public Button RewardButton;
    
    public ButtonColorChange buttonColorChange;
    
    public void SetCarde(bool ActiveReward, bool FinishCheck, string TitleString, string _diaReward)
    {
        Thumbnail.preserveAspect = true;
        
        RewardButton.interactable = ActiveReward;
        if(ActiveReward)
            transform.SetAsFirstSibling();
        
        if (FinishCheck)
        {
            ButtonText.text = "¿Ï·á";
            RewardButton.interactable = false;
            transform.SetAsLastSibling();
        }
        DiaReward.text = _diaReward;
        Title.text = TitleString;
        
        if(ActiveReward)
            buttonColorChange.ChangeFirstColor();
        else
            buttonColorChange.ChangeSecondColor();
        
    }
    
    
    public void GetReward()
    {
        challenge.GetReward();
    }
 }
