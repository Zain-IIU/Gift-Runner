using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleSystem : MonoSingleton<PlayerParticleSystem>
{

    [SerializeField]
    GameObject correctTreeVFX;

    [SerializeField]
    GameObject inCorrectTreeVFX;

    public void PlayCorrectVX()
    {
        correctTreeVFX.SetActive(true);
    }
    public void PlayInCorrectVX()
    {
        inCorrectTreeVFX.SetActive(true);
    }
}
