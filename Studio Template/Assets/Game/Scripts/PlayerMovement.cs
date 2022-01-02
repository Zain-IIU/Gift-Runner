using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float rotationSPeed;
    [SerializeField] float moveSpeed;
    [SerializeField] Animator Anim;
    private float curSpeed;

    float yRot;

    bool hasReachedEnd;

    Rigidbody RB;
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
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
        else if (Input.GetMouseButton(0) == false && !hasReachedEnd)
            yRot = 0f;
        
    }

    public void StopPlayer()
    {
        curSpeed = 0f;
        Anim.SetTrigger("Stop");
    }
    public void StartPlayer()
    {
        Anim.SetTrigger("Move");
        curSpeed = moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EndPoint"))
        {
            Destroy(other);
            UiManager.instance.HideUI();
            CameraManager.instance.EnableEndCam();
            StopPlayer();
            hasReachedEnd = true;
        }
    }
}
