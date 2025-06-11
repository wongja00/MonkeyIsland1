using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

//���ڰ� �Ѹ��Ǹ鼭 �ö󰡴� �ִϸ��̼�
public class NumberRolling : MonoBehaviour
{
    public static float spinDuration = 0.5f; // ��ü ȸ�� ���� �ð�
    
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
    
    
    // ������ ���� K, M, B, T ������ ��ȯ�ϴ� �Լ�
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
            // 1000T �̻��� a, b, c, ... z�� ǥ��
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