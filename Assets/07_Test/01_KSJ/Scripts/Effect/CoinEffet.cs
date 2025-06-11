using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CoinEffet : MonoBehaviour, ClickableObject
{
    [SerializeField]
    private ParticleSystem coinEffect;
    
    [SerializeField]
    private GameObject coinSprite;
    
    //코인 나오는 용
    [SerializeField]
    private CircleCollider2D _collider;
    
    [SerializeField]
    private Image CoinImage;
    
    [SerializeField]
    private SpriteRenderer CoinSpriteRenderer;
    
    List<GameObject> CoinList = new List<GameObject>();
    List<SpriteRenderer> CoinSpriteList = new List<SpriteRenderer>();

    private Vector3 coinImageOriginSize;
    
    private bool isCoinEffectDelay = false;
    
    [SerializeField]
    private Animator coinAnimator;
    
    private void Start()
    {
        CoinSpriteRenderer = coinSprite.GetComponent<SpriteRenderer>();
        CoinSpriteRenderer.color = new Color(1, 1, 1, 0);
        coinImageOriginSize = CoinImage.transform.localScale;
        
        for (int i = 0; i < 8; i++)
        {
            GameObject UpCoing = Instantiate(coinSprite, new Vector3(Random.Range(_collider.bounds.min.x, _collider.bounds.max.x), Random.Range(_collider.bounds.min.y, _collider.bounds.max.y), 0), Quaternion.identity);
                
            CoinList.Add(UpCoing);
            CoinSpriteList.Add(UpCoing.GetComponent<SpriteRenderer>());
        }
    }
    
    public void OnClick()
    {
        if(!isCoinEffectDelay)
            CoinEffectStart();
    }

    private void CoinEffectStart()
    {
        isCoinEffectDelay = true;
        
        for (int i = 0; i < CoinList.Count; i++)
        {
            int index = i;
                
            CoinList[index].transform.position = 
                (new Vector3(Random.Range(_collider.bounds.min.x, _collider.bounds.max.x),
                    Random.Range(_collider.bounds.min.y, _collider.bounds.max.y),0 ));
           
            CoinSpriteList[index].DOColor(Color.white, 0.3f).SetEase(Ease.Linear).onComplete = () =>
            {
                StartCoroutine(MoveToCoinUI(index));
            };
        }
    }

    private IEnumerator MoveToCoinUI(int index)
    {
        yield return new WaitForSeconds(0.2f);
        CoinList[index].transform.DOMove(CoinImage.transform.position, 0.5f).SetEase(Ease.Linear).onComplete = () =>
        {
            CoinSpriteList[index].color = new Color(1, 1, 1, 0);
        };
        yield return new WaitForSeconds(0.5f); 

        if(index == CoinList.Count - 1)
        {
            CoinImage.transform.DOScale(CoinImage.transform.localScale * 1.5f, 0.1f).onComplete = () =>
            {
                CoinImage.transform.DOScale(CoinImage.transform.localScale / 1.5f, 0.1f);
            };
            coinEffect.Play();
            coinAnimator.Play("CoinAnimation2", 0, 0f);
            
            CoinImage.transform.localScale = coinImageOriginSize;
            
            isCoinEffectDelay = false;
        }
    }
    
}
