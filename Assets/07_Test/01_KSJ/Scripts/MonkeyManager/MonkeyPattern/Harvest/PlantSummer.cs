using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlantSummer : MonkeyParent
{
    public List<Harvester> plantPrefab;
    
    public Transform plantPos;
    
    public List<ParticleSystem> plantParticles = new List<ParticleSystem>();
 
    public List<Sprite> sommer = new List<Sprite>();
    
    private void Start()
    {
        //AdjustPivotToBottomCenter();
        plantParticles[MonCode-7].Play();
        spriteRenderer.sprite = sommer[MonCode-7];
        StartCoroutine(CheckParticleSystemEnd(plantParticles[MonCode-7]));
    }

    private IEnumerator CheckParticleSystemEnd(ParticleSystem particleSystem)
    {
        while (!particleSystem.isStopped)
        {
            yield return null;
        }
        OnSummoned();
    }
    
    //식물 소환
    public void OnSummoned()
    {
        Harvester plant = Instantiate(plantPrefab[MonCode-7], plantPos.position, Quaternion.identity);
        plant.animator.speed = 0;
        
        Material plantMaterial = plant.spriteRenderer.material;
        Color color = plantMaterial.color;
        color.a = 0;
        plantMaterial.color = color;
        
        Material myMaterial = spriteRenderer.material;
        
        plantMaterial.DOFade(1, 1.5f).OnComplete(() =>
        {
            plant.animator.speed = 1;
            myMaterial.DOFade(0, 1.5f);
        });
            

       
    }
}
