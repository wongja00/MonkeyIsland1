using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ĺ� ��ġ ������Ʈ �������̽�
public interface DecorateObjectInterface
{
    //��Ĺ� ��ġ ���� 
    public bool isDecorate { get; set; }
    
    //��Ĺ� ��ġ
    public void Decorate();
    
    //��Ĺ� ����
    public void Remove();
    
    //��Ĺ� ��ġ ���� ��ġ ���� Ȯ��
    public bool CheckDecorate();
    
    public DecorateBuff decorateBuff { get; set; }
    
}
