using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class PlayerPickUpSystem : MonoSingleton<PlayerPickUpSystem>
{
    Animator Anim;

 
    [SerializeField] GameObject treeDescription;
    [SerializeField] Image giftTimerImage;
    
    public int curGifts;
    [SerializeField] float stopTimer;
    private void Start()
    {
        Anim = GetComponent<Animator>();
        curGifts = 0;
        UiManager.instance.ChangeGiftValue();
        giftTimerImage.fillAmount = 0;
    }


    public void SetGiftCount(int value) => curGifts = value;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<GiftBox>(out var gift))
        {
            if(!gift.hasPickedBefore())
            {
                gift.transform.DOScale(0, 0.25f).SetEase(Ease.InOutSine);
                GiftHandler.instance.AddNextBox(gift);
                curGifts++;
                UiManager.instance.ChangeGiftValue();
                UiManager.instance.TweenProgression(true);
                gift.hasPicked(true);
            }
        }
        if (other.TryGetComponent<Tree>(out var tree))
        {
            GetComponent<PlayerMovement>().StopPlayer();
            StartCoroutine(nameof(StopforChangingCloths),tree);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Tree>(out var tree))
        {
            tree.gameObject.transform.DOScaleY(0.1f, 0.25f).SetEase(Ease.InBounce);
        }
    }

    IEnumerator StopforChangingCloths(Tree tree)
    {
        CameraManager.instance.EnableCheckPointCamera(true);
        giftTimerImage.DOFillAmount(1, stopTimer);
        tree.ClosetheDoor();
        yield return new WaitForSeconds(stopTimer/2);
        tree.CheckGiftCount();
        if (curGifts >= tree.treeGifts())
        {
            GiftHandler.instance.RemoveItem_CheckPoint(tree.treeGifts(), tree.getGiftSpot(), tree.getGiftstoEnable());
            curGifts -= tree.treeGifts();
            Debug.Log("AT CHECKPOINT");
        }
        else
        {
            GiftHandler.instance.ClearTheList();
            curGifts = 0;
        }
        yield return new WaitForSeconds(stopTimer);


        CameraManager.instance.EnableCheckPointCamera(false);
        UiManager.instance.ChangeGiftValue();
        giftTimerImage.fillAmount = 0;
        UiManager.instance.UpdateAvatar(giftTimerImage);
        ClothSystem.instance.UpdateClothing();
        GetComponent<PlayerMovement>().StartPlayer(false);
       
    }
}
