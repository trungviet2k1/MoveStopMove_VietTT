using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class CanvasMainMenu : UICanvas
{
    [Header("Level and Ranking")]
    [SerializeField] protected RectTransform ranksTransform;
    [SerializeField] protected Slider playerEXP;
    [SerializeField] protected GameObject woodRank;
    [SerializeField] protected GameObject silverRank;
    [SerializeField] protected GameObject goldRank;

    [Header("Money")]
    [SerializeField] protected RectTransform totalCoin;
    [SerializeField] protected TextMeshProUGUI coinAmount;

    [Header("Settings")]
    [SerializeField] protected RectTransform settings;
    [SerializeField] protected GameObject openSound;
    [SerializeField] protected GameObject vibration;
    [SerializeField] protected GameObject noSound;
    [SerializeField] protected GameObject noVibration;

    [Header("Events")]
    [SerializeField] protected RectTransform eventRoot;
    [SerializeField] protected TextMeshProUGUI countdownTime;

    [Header("Bottom Root")]
    [SerializeField] protected RectTransform itemButton;
    [SerializeField] protected RectTransform playButton;

    [Header("Duration Animation")]
    [SerializeField] protected float duration;

    private Vector2 rankStartPos;
    private Vector2 itemButtonStartPos;
    private Vector2 totalCoinStartPos;
    private Vector2 settingsStartPos;
    private Vector2 eventRootStartPos;
    private Vector2 playButtonStartPos;

    private DateTime targetTime;
    private const string targetTimeKey = "CountdownTargetTime";


    private void Awake()
    {
        CacheStartPositions();

        if (PlayerPrefs.HasKey(targetTimeKey))
        {
            long savedTime = long.Parse(PlayerPrefs.GetString(targetTimeKey));
            targetTime = new DateTime(savedTime);
        }
        else
        {
            SetCountdownTimer(23);
        }

        UIManager.Ins.soundState = PlayerPrefs.GetInt("SoundState", 1) == 1;
        openSound.SetActive(UIManager.Ins.soundState);
        noSound.SetActive(!UIManager.Ins.soundState);
    }

    private void CacheStartPositions()
    {
        rankStartPos = ranksTransform.anchoredPosition;
        itemButtonStartPos = itemButton.anchoredPosition;
        totalCoinStartPos = totalCoin.anchoredPosition;
        settingsStartPos = settings.anchoredPosition;
        eventRootStartPos = eventRoot.anchoredPosition;
        playButtonStartPos = playButton.anchoredPosition;
    }

    private void OnEnable()
    {
        MoveElementsIntoView();
    }

    private void MoveElementsIntoView()
    {
        SetElementsOffScreen();
        AnimateUI();
    }

    private void SetElementsOffScreen()
    {
        ranksTransform.anchoredPosition = new Vector2(-Screen.width, rankStartPos.y);
        itemButton.anchoredPosition = new Vector2(-Screen.width, itemButtonStartPos.y);
        totalCoin.anchoredPosition = new Vector2(Screen.width, totalCoinStartPos.y);
        settings.anchoredPosition = new Vector2(Screen.width, settingsStartPos.y);
        eventRoot.anchoredPosition = new Vector2(Screen.width, eventRootStartPos.y);
        playButton.anchoredPosition = new Vector2(Screen.width, playButtonStartPos.y);
    }

    private void AnimateUI()
    {
        ranksTransform.DOAnchorPos(rankStartPos, duration).SetEase(Ease.InOutBack);
        itemButton.DOAnchorPos(itemButtonStartPos, duration).SetEase(Ease.InOutBack);
        totalCoin.DOAnchorPos(totalCoinStartPos, duration).SetEase(Ease.InOutBack);
        settings.DOAnchorPos(settingsStartPos, duration).SetEase(Ease.InOutBack);
        eventRoot.DOAnchorPos(eventRootStartPos, duration).SetEase(Ease.InOutBack);
        playButton.DOAnchorPos(playButtonStartPos, duration).SetEase(Ease.InOutBack);
    }

    private void OnDisable()
    {
        ResetElementsPosition();
    }

    private void ResetElementsPosition()
    {
        ranksTransform.anchoredPosition = rankStartPos;
        itemButton.anchoredPosition = itemButtonStartPos;
        totalCoin.anchoredPosition = totalCoinStartPos;
        settings.anchoredPosition = settingsStartPos;
        eventRoot.anchoredPosition = eventRootStartPos;
        playButton.anchoredPosition = playButtonStartPos;
    }

    private void SetCountdownTimer(int hours)
    {
        DateTime now = DateTime.Now;
        DateTime startTime = new(now.Year, now.Month, now.Day, hours, 0, 0);

        if (now >= startTime)
        {
            startTime = startTime.AddDays(1);
        }

        targetTime = startTime.AddHours(24);
        SaveTargetTime();
    }

    private void SaveTargetTime()
    {
        PlayerPrefs.SetString(targetTimeKey, targetTime.Ticks.ToString());
        PlayerPrefs.Save();
    }

    void Update()
    {
        UpdateCountdownTimer();
    }

    private void UpdateCountdownTimer()
    {
        TimeSpan remainingTime = targetTime - DateTime.Now;

        if (remainingTime.TotalSeconds <= 0)
        {
            SetCountdownTimer(23);
            remainingTime = TimeSpan.FromHours(24);
        }

        countdownTime.text = $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
    }

    public override void OnInit()
    {
        coinAmount.text = UIManager.Ins.coinAmount.ToString();
    }

    public void PlayGame()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        UIManager.Ins.OpenUI(UIName.GamePlay);
        SoundManagement.Ins.ToggleMusic(false);
    }

    public UIName? ConvertStringToUIName(string shopName)
    {
        if (Enum.TryParse(shopName, out UIName result))
        {
            return result;
        }
        return null;
    }

    public void OpenShop(string shopName)
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        UIName? uiName = ConvertStringToUIName(shopName);
        if (uiName.HasValue)
        {
            UIManager.Ins.OpenUI(uiName.Value);
        }
    }

    public void ToggleSound()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        UIManager.Ins.soundState = !UIManager.Ins.soundState;
        SoundManagement.Ins.SoundOn(UIManager.Ins.soundState);

        openSound.SetActive(UIManager.Ins.soundState);
        noSound.SetActive(!UIManager.Ins.soundState);
    }

    public void ToggleVibration()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        UIManager.Ins.vibrationState = !UIManager.Ins.vibrationState;
        vibration.SetActive(UIManager.Ins.vibrationState);
        noVibration.SetActive(!UIManager.Ins.vibrationState);
    }

    public void UpdatePlayerRank(int EXP)
    {
        UIManager.Ins.playerEXP = EXP;
        UpdateExperienceSlider();

        if (UIManager.Ins.playerEXP >= 100)
        {
            UIManager.Ins.playerEXP -= 100;
            PromotePlayerRank();
        }
    }

    private void UpdateExperienceSlider()
    {
        playerEXP.value = (float)UIManager.Ins.playerEXP / 100;
    }

    private void PromotePlayerRank()
    {
        if (UIManager.Ins.playerRank == PlayerRank.Wood)
        {
            UIManager.Ins.playerRank = PlayerRank.Silver;
            woodRank.SetActive(false);
            silverRank.SetActive(true);
        }
        else if (UIManager.Ins.playerRank == PlayerRank.Silver)
        {
            UIManager.Ins.playerRank = PlayerRank.Gold;
            silverRank.SetActive(false);
            goldRank.SetActive(true);
        }
    }

    private void OnApplicationQuit()
    {
        SaveTargetTime();
    }
}