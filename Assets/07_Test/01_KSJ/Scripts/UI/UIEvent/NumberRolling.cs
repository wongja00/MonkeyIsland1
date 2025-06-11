using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

//숫자가 롤링되면서 올라가는 애니메이션
public class NumberRolling : MonoBehaviour
{
    public static float spinDuration = 0.5f; // 전체 회전 지속 시간
    
    public static void SpinningNumber(ref double startNumber, double endNumber, TextMeshProUGUI text, Action<double> updateOriginalValue)
    {
        double temp = startNumber;
        DOTween.To(() => temp, x => temp = x, endNumber, spinDuration).OnUpdate(() =>
        {
            text.text = ConvertNumberToText(Math.Floor(temp));
        }).OnComplete(() =>
        {
            updateOriginalValue(temp);
        });
        
    }
    
    
    // 단위에 따라서 K, M, B, T 등으로 변환하는 함수
    public static string ConvertNumberToText(double number)
    {
        if (number < 1000d)
        {
            return number.ToString();
        }
        else if (number < 1000000d)
        {
            return (number / 1000d).ToString("F1") + "K";
        }
        else if (number < 1000000000d)
        {
            return (number / 1000000d).ToString("F1") + "M";
        }
        else if (number < 1000000000000d)
        {
            return (number / 1000000000d).ToString("F1") + "B";
        }
        else if(number < 1000000000000000d)
        {
            return (number / 1000000000000d).ToString("F1") + "T";
        }
        else
        {
            // 1000T 이상은 a, b, c, ... z로 표현
            double baseValue = 1000000000000000d;
            int exponent = 0;
            while (number >= baseValue * 1000d)
            {
                baseValue *= 1000d;
                exponent++;
            }
            char suffix = (char)('a' + exponent);
            return (number / baseValue).ToString("F1") + suffix;
        }
    }
}