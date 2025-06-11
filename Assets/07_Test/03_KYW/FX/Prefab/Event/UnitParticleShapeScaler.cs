using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitParticleShapeScaler : MonoBehaviour
{
    public ParticleSystem sparkleEffect;  // ��ƼŬ �ý��� ����
    public SpriteRenderer unitSpriteRenderer;  // ������ ��������Ʈ ������ ����

    void Start()
    {
        unitSpriteRenderer = GetComponentInParent<SpriteRenderer>();
        
        // ���� �� ��ƼŬ�� Shape ũ�⸦ ���� ũ�⿡ �°� ����
        ScaleParticleShapeToUnit();
    }

    void ScaleParticleShapeToUnit()
    {
        var shape = sparkleEffect.shape;
        shape.sprite = unitSpriteRenderer.sprite;
    }
}
