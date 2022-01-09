using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothSystem : MonoSingleton<ClothSystem>
{
    [SerializeField] GameObject[] models;

    [SerializeField] Avatar[] avatars;

    Animator Anim;

    int index;
    private void Awake()
    {
        Anim = GetComponent<Animator>();
        index = 0;
        Anim.avatar = avatars[index];
    }
    private void Start()
    {
        models[index].SetActive(true);
    }
    public void UpdateClothing()
    {
        PlayerParticleSystem.instance.PlayCorrectVX();
        foreach (GameObject model in models)
            model.SetActive(false);
        index++;
        Anim.avatar = avatars[index];
        models[index].SetActive(true);
    }
    
}
