using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitParticleShapeScaler : MonoBehaviour
{
    public ParticleSystem sparkleEffect;  // 파티클 시스템 참조
    public SpriteRenderer unitSpriteRenderer;  // 유닛의 스프라이트 렌더러 참조

    void Start()
    {
        unitSpriteRenderer = GetComponentInParent<SpriteRenderer>();
        
        // 시작 시 파티클의 Shape 크기를 유닛 크기에 맞게 조정
        ScaleParticleShapeToUnit();
    }

    void ScaleParticleShapeToUnit()
    {
        var shape = sparkleEffect.shape;
        shape.sprite = unitSpriteRenderer.sprite;
    }
}
