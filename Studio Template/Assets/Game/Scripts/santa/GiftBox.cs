using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GiftBox : MonoBehaviour
{
    [SerializeField]
    Transform targetToFollow;

    [SerializeField]
    float lerpTime;

    [SerializeField]
    Vector3 offset;
    [SerializeField]
    int customID;


    public int getID()
    {
        return customID;
    }
    // Update is called once per frame
    void Update()
    {
        if (targetToFollow)
            FollowTarget(); 
    }
    public void FollowNext(Transform newTarget)
    {
        targetToFollow = newTarget;
    }

    private void  FollowTarget()
    {
        transform.DOMove(targetToFollow.position+offset,lerpTime);
    }
}

