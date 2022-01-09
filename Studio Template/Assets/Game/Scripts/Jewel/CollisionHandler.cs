using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    string tag_Obstacle;

    GiftBox box;

    private void Start()
    {
        box = GetComponent<GiftBox>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(tag_Obstacle))
        {
            GiftHandler.instance.RemoveItems_Obstacle(box.getID());
            Destroy(other);
        }
    }
   
}
