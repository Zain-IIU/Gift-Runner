using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float rotationSPeed;
    [SerializeField] float moveSpeed;

    private float curSpeed;

    float yRot;
    

    Rigidbody RB;
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        curSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.forward * curSpeed * Time.deltaTime);
        transform.DORotateQuaternion(Quaternion.Euler(0, yRot, 0), 0.25f);

        if (Input.GetMouseButtonDown(0))
            return;
  

        if (Input.GetMouseButton(0))
        {
            yRot += Input.GetAxis("Mouse X") * rotationSPeed;
            yRot = Mathf.Clamp(yRot, -20f, 20f);

        }
        else
            yRot = 0f;
        
    }

    public void StopPlayer()
    {
        curSpeed = 0f;
    }
    public void StartPlayer()
    {
        curSpeed = moveSpeed;
    }
}
