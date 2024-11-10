using UnityEngine;
using TMPro;
using DG.Tweening;

public class CanvasCoinShop : UICanvas
{
    [Header("Top Root")]
    [SerializeField] protected RectTransform totalCoin;
    [SerializeField] protected TextMeshProUGUI coinAmount;

    [Header("Shop UI")]
    [SerializeField] protected RectTransform shopBackground;

    [Header("Duration Animation")]
    [SerializeField] protected float duration;

    private Vector2 totalCoinOriginalAnchoredPosition;
    private Vector2 shopBackgroundOriginalAnchoredPosition;

    private void OnEnable()
    {
        totalCoinOriginalAnchoredPosition = totalCoin.anchoredPosition;
        shopBackgroundOriginalAnchoredPosition = shopBackground.anchoredPosition;

        MoveElementsIntoView();
    }

    public override void OnInit()
    {
        coinAmount.text = UIManager.Ins.coinAmount.ToString();
    }

    private void MoveElementsIntoView()
    {
        totalCoin.anchoredPosition = new Vector2(Screen.width, totalCoinOriginalAnchoredPosition.y);
        shopBackground.anchoredPosition = new Vector2(shopBackgroundOriginalAnchoredPosition.x, -Screen.height);

        totalCoin.DOAnchorPos(totalCoinOriginalAnchoredPosition, duration).SetEase(Ease.OutExpo);
        shopBackground.DOAnchorPos(shopBackgroundOriginalAnchoredPosition, duration).SetEase(Ease.OutExpo);
    }

    public void XButton()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        UIManager.Ins.OpenUI(UIName.MainMenu);
    }

    public void BuyCoin()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        UIManager.Ins.coinAmount += 1000;
        PlayerPrefs.SetInt("Score", UIManager.Ins.coinAmount);
        PlayerPrefs.Save();
        coinAmount.text = UIManager.Ins.coinAmount.ToString();
    }

    public void NoAds()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        UIManager.Ins.coinAmount = 0;
        PlayerPrefs.SetInt("Score", UIManager.Ins.coinAmount);
        PlayerPrefs.Save();
        coinAmount.text = UIManager.Ins.coinAmount.ToString();
    }

    public void ResetItem()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        for (int i = 0; i < 25; i++)
        {
            PlayerPrefs.SetInt(Constants.GetSkinShopKey((SkinController.ClothesType)i), 1);
            PlayerPrefs.SetInt(Constants.GetLockState((SkinController.ClothesType)i), 0);
            PlayerPrefs.SetInt(Constants.GetWeaponShopKey((SkinController.WeaponType)i), 1);
        }
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        totalCoin.anchoredPosition = totalCoinOriginalAnchoredPosition;
        shopBackground.anchoredPosition = shopBackgroundOriginalAnchoredPosition;
    }
}