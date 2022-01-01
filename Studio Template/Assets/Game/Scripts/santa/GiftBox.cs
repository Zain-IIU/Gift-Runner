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
    [SerializeField]
    List<GameObject> boxes = new List<GameObject>();

    int boxLevel;

  
    private void Start()
    {
        boxLevel = 0;
        boxes[boxLevel].SetActive(true);
        
    }
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
    public void UpgradeBox()
    {
        transform.DOScale(Vector3.one * 1.25f, 0.25f).OnComplete(() =>
        {
            transform.DOScale(1, 0.15f);
        });
        boxLevel++;
        for(int i=0;i<boxes.Count;i++)
        {
            if (i == boxLevel)
                boxes[boxLevel].SetActive(true);
            else
                boxes[i].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Multiplier"))
        {
            UpgradeBox();
        }
    }
}

