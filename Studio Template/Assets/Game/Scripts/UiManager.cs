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
    [SerializeField] RectTransform mainMenuUI;
    [SerializeField] RectTransform winPanel;
    [SerializeField]
    PlayerPickUpSystem playerGifts;
    [SerializeField]
    float tweeningTime;
    [SerializeField]
    Ease easeType;
    private void Start()
    {
        levelProgressionSlider.value = 0;
        countText.text = "x" + playerGifts.curGifts.ToString();
        GameManager.instance.OnGameStarted += StartGameUI;
    }

    private void StartGameUI()
    {
        mainMenuUI.DOScale(0, tweeningTime).SetEase(easeType);
        levelProgressionSlider.transform.parent.DOScaleX(1, tweeningTime);
        countText.transform.parent.DOScale(new Vector2(0.43f, 0.49f), tweeningTime);
    }

    public override void Init()
    {
        TapText(false);
      
    }


    public void ShowHideLevelFailedUi(bool value)
    {
        panelFailed.SetActiveAll(value);
    }
    public void ShowHideLevelWinUi(bool value)
    {
        winPanel.gameObject.SetActive(value);
        winPanel.DOScale(1, tweeningTime).SetEase(easeType);
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
        //ShowHideLevelCompleteUi(false);
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
        levelProgressionSlider.transform.parent.DOScaleX(0, 0.25f);
        countText.transform.parent.DOScaleX(0, 0.25f).OnComplete(() =>
        {
            GiftHandler.instance.DistributeGifts();
        });
    }

    int avatarIndex;
    [SerializeField]
    Sprite[] avatars;
    public void UpdateAvatar(Image avatarImage)
    {
        avatarIndex++;
        avatarImage.transform.GetChild(0).GetComponent<Image>().sprite = avatars[avatarIndex];
    }

    



}
