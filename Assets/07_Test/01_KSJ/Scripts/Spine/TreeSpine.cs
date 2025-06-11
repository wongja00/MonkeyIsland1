using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class TreeSpine : MonoBehaviour
{
    const int TrackIndex = 1;

    public SkeletonAnimation TreeAnimation;

    [SpineAnimation]
    public string shakeAnimation;
    
    public void PlayAnimation(string animationName)
    {
        TreeAnimation.AnimationState.SetAnimation(TrackIndex, animationName, false);
    }
    
    public void TreeShake()
    {
        PlayAnimation(shakeAnimation);
    }
}
