using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class PlayerPickUpSystem : MonoSingleton<PlayerPickUpSystem>
{
    Animator Anim;

    [SerializeField] TextMeshPro countText;
    [SerializeField] GameObject treeDescription;
    [SerializeField] Image giftTimerImage;
    
    public int curGifts;
    private void Start()
    {
        Anim = GetComponent<Animator>();
        curGifts = 0;
        countText.text = "x" + curGifts.ToString();
        giftTimerImage.fillAmount = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Gift"))
        {
            
            GiftHandler.instance.AddNextBox(other.gameObject.GetComponent<GiftBox>());
            curGifts++;
            countText.text = "x" + curGifts.ToString();
            //Destroy(other);
        }
        if (other.gameObject.CompareTag("Tree"))
        {
            Anim.SetTrigger("Stop");
            GetComponent<PlayerMovement>().StopPlayer();
            StartCoroutine(nameof(StopForDeliveringGifts),other);
        }

    }

    IEnumerator StopForDeliveringGifts(Collider other)
    {
        
        giftTimerImage.DOFillAmount(1, 2);
        yield return new WaitForSeconds(2f);
        other.gameObject.GetComponent<Tree>().CheckGiftCount();
        if (curGifts >= other.gameObject.GetComponent<Tree>().treeGifts())
        {
            GiftHandler.instance.RemoveItem_CheckPoint(other.gameObject.GetComponent<Tree>().treeGifts());
            curGifts -= other.gameObject.GetComponent<Tree>().treeGifts();
        }
        else
        {
            GiftHandler.instance.ClearTheList();
            curGifts = 0;
        }
        countText.text = "x" + curGifts.ToString();
        giftTimerImage.fillAmount = 0;
        Anim.SetTrigger("Move");
        GetComponent<PlayerMovement>().StartPlayer();
    }
}
