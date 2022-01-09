using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Tree : MonoBehaviour
{
    [SerializeField]
    int giftToDistribute;
    [SerializeField]
    GameObject treeDescription;

    [SerializeField]
    Transform giftThrowingSpot;
    [SerializeField]
    Transform giftsToEnable;
    [SerializeField]
    TextMeshPro gifText;
    // Start is called before the first frame update
    [SerializeField]
    Transform door;

    void Start()
    {
        gifText.text ="x"+ giftToDistribute.ToString();
    }
    
    public void CheckGiftCount()
    {
        if(PlayerPickUpSystem.instance.curGifts>=giftToDistribute)
        {
            PlayerParticleSystem.instance.PlayCorrectVX();
            UiManager.instance.TweenProgression(true);
        }
        else
        {
            PlayerParticleSystem.instance.PlayInCorrectVX();
            UiManager.instance.TweenProgression(false);
        }
        treeDescription.transform.DOScaleY(0, 0.25f).SetEase(Ease.InOutSine);
    }
    public int treeGifts()
    {
        return giftToDistribute;
    }

    public Transform getGiftSpot()
    {
        return giftThrowingSpot;
    }
    
    public Transform getGiftstoEnable()
    {
        return giftsToEnable;
    }

    public void EnableGifts()
    {
        giftsToEnable.DOScale(1, 0.25f).SetEase(Ease.InOutSine);
    }
    public void ClosetheDoor()
    {
        door.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.5f).SetEase(Ease.InOutSine);
    }


}
