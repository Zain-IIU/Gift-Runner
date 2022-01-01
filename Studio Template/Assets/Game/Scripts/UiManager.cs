using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UiManager : MonoSingleton<UiManager>
{
    [SerializeField] GameObject gameTitle;
    [SerializeField] GameObject[] panelLevelComplete;
    [SerializeField] GameObject[] panelFailed;

    [SerializeField] Slider levelProgressionSlider;
    [SerializeField] TextMeshPro countText;

    [SerializeField]
    PlayerPickUpSystem playerGifts;
    private void Start()
    {
        levelProgressionSlider.value = 0;
        countText.text = "x" + playerGifts.curGifts.ToString();
    }
    public override void Init()
    {
        TapText(false);
      
    }


    public void ShowHideLevelFailedUi(bool value)
    {
        panelFailed.SetActiveAll(value);
    }
    public void ShowHideLevelCompleteUi(bool value)
    {
        panelLevelComplete.SetActiveAll(value);
    }
    public void TapText(bool isShow)
    {
       
    }

    public void TweenProgression(bool toIncrement)
    {
        if (toIncrement)
            levelProgressionSlider.DOValue(levelProgressionSlider.value + 0.2f, 0.25f);
        else
            levelProgressionSlider.DOValue(levelProgressionSlider.value - 0.2f, 0.25f);
    }

    public void ChangeGiftValue()
    {
        countText.text = "x" + playerGifts.curGifts.ToString();
    }
    public void ChangeGiftValue(int value)
    {
        playerGifts.SetGiftCount(value);
        countText.text = "x" + playerGifts.curGifts.ToString();
    }

    // Ui Buttons
    public void PlayButton(GameObject playBtn)
    {       
        playBtn.SetActive(false);
        gameTitle.SetActive(false);
        GameManager.instance.RestartLevel();
    }

    public void BtnNextLevel()
    {
        ShowHideLevelCompleteUi(false);
        Init();
        GameManager.instance.StartNewLevel();
    }

    public void BtnRetryLevel()
    {
        ShowHideLevelFailedUi(false);
        Init();
        GameManager.instance.RestartLevel();
    }
    public void HideUI()
    {
        levelProgressionSlider.transform.DOScaleX(0, 0.25f);

    }


    



}
