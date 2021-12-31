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
        {
            giftPacks[curBoxIndex].FollowNext(giftPacks[curBoxIndex - 1].transform);
        }
        curBoxIndex++;
    }
    public void RemoveItem_CheckPoint(int amount)
    {
        int totalGifts = giftPacks.Count;
        
        for(int i=1; i<=amount;i++)
        {
            giftPacks[totalGifts - i].FollowNext(null);
            giftPacks.RemoveAt(totalGifts - i);
        }
        curBoxIndex = giftPacks.Count;

    }

    public void RemoveItems_Obstacle(Collider newCollider)
    {
        int location = 3;

        for (int i = location; i < giftPacks.Count; i++)
        {
            giftPacks[i].FollowNext(null);
            giftPacks.RemoveAt(i);
        }
        curBoxIndex = location;
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

