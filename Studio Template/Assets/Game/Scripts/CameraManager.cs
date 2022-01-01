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
    [SerializeField] Transform finishTarget;

     public void StartGameCamera()
    {
        camMain.m_Priority = 15;
        startCam.m_Priority = 10;
    }
    public void EnableEndCam()
    {
        camMain.m_Priority = 10;
        camFinish.m_Priority = 15;
    }
    
}
