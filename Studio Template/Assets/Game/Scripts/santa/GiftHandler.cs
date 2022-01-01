using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GiftHandler : MonoSingleton<GiftHandler>
{
    [SerializeField]
    List<GiftBox> giftPacks = new List<GiftBox>();

    [SerializeField]
    int curBoxIndex;

    [SerializeField]
    Transform Player;
   


    // Start is called before the first frame update
    void Start()
    {
        curBoxIndex = 0;
    }

    public void AddNextBox(GiftBox newBox)
    {
        giftPacks.Add(newBox);

        if (curBoxIndex == 0)
            giftPacks[curBoxIndex].FollowNext(Player);
        else
            giftPacks[curBoxIndex].FollowNext(giftPacks[curBoxIndex - 1].transform);
        
        curBoxIndex++;
    }
    public void RemoveItem_CheckPoint(int amount,Transform giftThrowingPos,Transform giftEnabling)
    {
        int totalGifts = giftPacks.Count;
        
        for(int i=1; i<=amount;i++)
        {
            giftPacks[totalGifts - i].FollowNext(null);
            giftPacks[totalGifts - i].transform.DOMove(giftThrowingPos.position, 0.5f);
            giftPacks.RemoveAt(totalGifts - i);
        }
        giftEnabling.DOScale(1, 0.25f).SetEase(Ease.InOutSine);
        curBoxIndex = giftPacks.Count;

        UiManager.instance.ChangeGiftValue();
    }

      public void RemoveItems_Obstacle(int boxID)
    {
        int deletingIndex = -1;
        int totalGifts = giftPacks.Count;

        //finding the box
        for (int i = 0; i < giftPacks.Count; i++)
        {
            if (giftPacks[i].getID() == boxID)
            {
                deletingIndex = i;
                break;
            }
        }
        //removing them
        for (int i = deletingIndex; i < totalGifts; i++)
        {

            giftPacks[deletingIndex].gameObject.transform.DOScale(0, 0.35f);
            giftPacks[deletingIndex].FollowNext(null);
            giftPacks.RemoveAt(deletingIndex);
        }
        curBoxIndex = giftPacks.Count;
        UiManager.instance.ChangeGiftValue(curBoxIndex);
    }
    public void ClearTheList()
    {
        for (int i =0; i <giftPacks.Count; i++)
        {
            giftPacks[i].FollowNext(null);
        }
        giftPacks.Clear();
        curBoxIndex = 0;
    }

}

