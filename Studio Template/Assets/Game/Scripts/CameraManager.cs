using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] CinemachineVirtualCamera camMain;
    [SerializeField] CinemachineVirtualCamera camFinish;
    [SerializeField] CinemachineVirtualCamera startCam;
    [SerializeField] CinemachineVirtualCamera checkPointCamera;
    [SerializeField] Transform finishTarget;

     public void StartGameCamera()
    {
        camMain.m_Priority = 15;
        startCam.m_Priority = 10;
    }
    public void EnableEndCam()
    {
        camMain.gameObject.SetActive(false);
        camFinish.m_Priority = 15;
    }
    public void EnableCheckPointCamera(bool value)
    {
        camMain.gameObject.SetActive(!value);
        checkPointCamera.gameObject.SetActive(value);
    }
    
}
