using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    string tag_Obstacle;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(tag_Obstacle))
        {
            GiftHandler.instance.RemoveItems_Obstacle(other);
        }
    }
   
}
