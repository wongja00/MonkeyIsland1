using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//장식물 설치 오브젝트 인터페이스
public interface DecorateObjectInterface
{
    //장식물 설치 여뷰 
    public bool isDecorate { get; set; }
    
    //장식물 설치
    public void Decorate();
    
    //장식물 제거
    public void Remove();
    
    //장식물 설치 가능 위치 여부 확인
    public bool CheckDecorate();
    
    public DecorateBuff decorateBuff { get; set; }
    
}
