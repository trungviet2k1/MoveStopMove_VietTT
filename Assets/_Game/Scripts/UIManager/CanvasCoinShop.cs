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

    private Vector2 totalCoinPos;
    private Vector2 shopBackgroundPos;

    private void OnEnable()
    {
        CacheInitialPositions();
        MoveElementsIntoView();
    }

    public override void OnInit()
    {
        UpdateCoinAmountUI();
    }

    private void CacheInitialPositions()
    {
        totalCoinPos = totalCoin.anchoredPosition;
        shopBackgroundPos = shopBackground.anchoredPosition;
    }

    private void MoveElementsIntoView()
    {
        totalCoin.anchoredPosition = new Vector2(Screen.width, totalCoinPos.y);
        shopBackground.anchoredPosition = new Vector2(shopBackgroundPos.x, -Screen.height);

        totalCoin.DOAnchorPos(totalCoinPos, duration).SetEase(Ease.OutExpo);
        shopBackground.DOAnchorPos(shopBackgroundPos, duration).SetEase(Ease.OutExpo);
    }

    private void UpdateCoinAmountUI()
    {
        coinAmount.text = UIManager.Ins.coinAmount.ToString();
    }

    public void XButton()
    {
        PlayClickSound();
        UIManager.Ins.OpenUI(UIName.MainMenu);
    }

    public void BuyCoin()
    {
        PlayClickSound();
        UpdateCoinAmount(1000);
    }

    public void NoAds()
    {
        PlayClickSound();
        UpdateCoinAmount(-UIManager.Ins.coinAmount);
    }

    public void ResetItem()
    {
        PlayClickSound();
        ResetShopItems();
        EquipDefaultWeapon();
        EquipDefaultClothes();
    }

    private void PlayClickSound()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
    }

    private void UpdateCoinAmount(int amount)
    {
        UIManager.Ins.coinAmount += amount;
        PlayerPrefs.SetInt("Score", UIManager.Ins.coinAmount);
        PlayerPrefs.Save();
        UpdateCoinAmountUI();
    }

    private void ResetShopItems()
    {
        for (int i = 0; i < 25; i++)
        {
            PlayerPrefs.SetInt(Constants.GetSkinShopKey((SkinController.ClothesType)i), 1);
            PlayerPrefs.SetInt(Constants.GetLockState((SkinController.ClothesType)i), 0);
            PlayerPrefs.SetInt(Constants.GetWeaponShopKey((SkinController.WeaponType)i), 1);
        }
        PlayerPrefs.Save();
    }

    private void EquipDefaultWeapon()
    {
        var defaultWeaponMaterial = PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Hammer_1];
        PlayerController.instance.WeaponSwitching(SkinController.WeaponType.Hammer, defaultWeaponMaterial);
        PlayerPrefs.SetInt(Constants.GetWeaponShopKey(SkinController.WeaponType.Hammer), (int)WeaponState.Equipped);
    }

    private void EquipDefaultClothes()
    {
        PlayerController.instance.characterSkin.ResetClothes();
    }

    private void OnDisable()
    {
        totalCoin.anchoredPosition = totalCoinPos;
        shopBackground.anchoredPosition = shopBackgroundPos;
    }
}