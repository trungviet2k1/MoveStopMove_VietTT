using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum WeaponState { CantBuy, CanBuy, Select, Equipped }

public class CanvasWeaponShop : UICanvas
{
    [Header("Price")]
    [SerializeField] int[] weaponPrices = { 2500, 800, 1500, 400, 100, 200, 1000, 600, 300, 200, 3000, 2000 };

    [Header("Top Root")]
    [SerializeField] RectTransform totalCoin;
    [SerializeField] TextMeshProUGUI coinAmount;

    [Header("Weapon Shop UI")]
    [SerializeField] RectTransform shopBackground;
    [SerializeField] TextMeshProUGUI weaponName;
    [SerializeField] Image imageCurrentWeapon;
    [SerializeField] RectTransform listWeapon;

    [Header("Buttons")]
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] RectTransform stateButton;
    [SerializeField] TextMeshProUGUI cantBuyText;
    [SerializeField] TextMeshProUGUI canBuyText;

    [Header("Animation Duration")]
    [SerializeField] float duration;

    private int shopWeaponID = 0;
    private Vector2 totalCoinPos;
    private Vector2 shopBackgroundPos;
    private readonly Dictionary<SkinController.WeaponType, WeaponState> weaponShopInfo = new();

    private void Awake()
    {
        totalCoinPos = totalCoin.anchoredPosition;
        shopBackgroundPos = shopBackground.anchoredPosition;
        InitializeWeaponShop();
        UpdateButtonVisibility();
    }

    private void OnEnable()
    {
        AnimateElementsIntoView();
        LoadWeaponShopState();
    }

    private void InitializeWeaponShop()
    {
        for (int i = 0; i < weaponPrices.Length; i++)
        {
            weaponShopInfo.Add((SkinController.WeaponType)i, WeaponState.CantBuy);
        }

        SetDefaultWeapon();
    }

    private void SetDefaultWeapon()
    {
        bool hasEquippedWeapon = false;

        for (int i = 0; i < weaponPrices.Length; i++)
        {
            if (PlayerPrefs.GetInt(Constants.GetWeaponShopKey((SkinController.WeaponType)i)) == (int)WeaponState.Equipped)
            {
                EquipWeapon((SkinController.WeaponType)i);
                hasEquippedWeapon = true;
                break;
            }
        }

        if (!hasEquippedWeapon)
        {
            EquipWeapon(SkinController.WeaponType.Hammer);
        }
    }

    private void EquipWeapon(SkinController.WeaponType weaponType)
    {
        PlayerPrefs.SetInt(Constants.GetWeaponShopKey(weaponType), (int)WeaponState.Equipped);
        PlayerPrefs.Save();

        Material[] weaponMaterial = weaponType switch
        {
            SkinController.WeaponType.Arrow => PlayerController.instance.characterSkin._weapon.ArrowDefaultMaterials,
            SkinController.WeaponType.RedAxe => PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Axe2_2],
            SkinController.WeaponType.BlueAxe => PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Axe1_2],
            SkinController.WeaponType.Boomerang => PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Boomerang_1],
            SkinController.WeaponType.Candy001 => PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Candy4_2],
            SkinController.WeaponType.Candy002 => PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Candy2_2],
            SkinController.WeaponType.Candy003 => PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.candy1_1],
            SkinController.WeaponType.Candy004 => PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Candy0_2],
            SkinController.WeaponType.Hammer => PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Hammer_1],
            SkinController.WeaponType.Knife => PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Knife_2],
            SkinController.WeaponType.Uzi => PlayerController.instance.characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Uzi_2],
            SkinController.WeaponType.Z => PlayerController.instance.characterSkin._weapon.ZDefaultMaterials,
            _ => null
        };

        if (weaponMaterial != null)
        {
            PlayerController.instance.WeaponSwitching(weaponType, weaponMaterial);
        }
    }

    private void AnimateElementsIntoView()
    {
        totalCoin.anchoredPosition = new Vector2(Screen.width, totalCoinPos.y);
        shopBackground.anchoredPosition = new Vector2(shopBackgroundPos.x, -Screen.height);

        totalCoin.DOAnchorPos(totalCoinPos, duration).SetEase(Ease.OutExpo);
        shopBackground.DOAnchorPos(shopBackgroundPos, duration).SetEase(Ease.OutExpo);
    }

    private void LoadWeaponShopState()
    {
        bool equippedWeaponFound = false;

        for (int i = 0; i < weaponPrices.Length; i++)
        {
            SkinController.WeaponType weaponType = (SkinController.WeaponType)i;
            WeaponState weaponState = (WeaponState)PlayerPrefs.GetInt(Constants.GetWeaponShopKey(weaponType), (int)WeaponState.CantBuy);
            weaponShopInfo[weaponType] = weaponState;

            if (weaponState == WeaponState.Equipped)
            {
                shopWeaponID = i;
                equippedWeaponFound = true;
            }
        }

        if (!equippedWeaponFound)
        {
            SetDefaultWeapon();
        }

        UpdateUIForCurrentWeapon();
    }

    public void OpenMainMenu()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        UIManager.Ins.OpenUI(UIName.MainMenu);
    }

    private void OnDisable()
    {
        ResetUIPositions();
        GameManagement.Ins.mainCamera.gameObject.SetActive(true);
        GameManagement.Ins.shopCamera.gameObject.SetActive(false);
        GameObject.FindObjectOfType<PlayerController>().OnIdle();
    }

    private void ResetUIPositions()
    {
        totalCoin.anchoredPosition = totalCoinPos;
        shopBackground.anchoredPosition = shopBackgroundPos;
    }

    public override void OnInit()
    {
        UpdateCoinAmount();
        UpdateWeaponShopState();
        UpdateUIForCurrentWeapon();

        GameManagement.Ins.mainCamera.gameObject.SetActive(false);
        GameManagement.Ins.shopCamera.gameObject.SetActive(true);
        GameObject.FindObjectOfType<PlayerController>().OnDance();
    }

    public void NextButton()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        shopWeaponID = Mathf.Min(shopWeaponID + 1, weaponPrices.Length - 1);
        UpdateUIForCurrentWeapon();
    }

    public void BackButton()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        shopWeaponID = Mathf.Max(shopWeaponID - 1, 0);
        UpdateUIForCurrentWeapon();
    }

    private void UpdateUIForCurrentWeapon()
    {
        ShowWeaponImage((SkinController.WeaponType)shopWeaponID);
        SetPriceText(weaponPrices[shopWeaponID]);
        ShowState();
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        backButton.SetActive(shopWeaponID > 0);
        nextButton.SetActive(shopWeaponID < weaponPrices.Length - 1);
    }

    public void ShowWeaponImage(SkinController.WeaponType weaponType)
    {
        for (int i = 0; i < listWeapon.childCount; i++)
        {
            Transform weapon = listWeapon.GetChild(i);
            weapon.gameObject.SetActive(i == (int)weaponType);
            if (i == (int)weaponType)
            {
                weaponName.text = weapon.name;
                AnimateWeaponImage(weapon);
            }
        }
    }

    private void AnimateWeaponImage(Transform weapon)
    {
        weapon.DOScale(Vector3.one * 1.15f, 0.1f).SetEase(Ease.OutBack).OnKill(() =>
        {
            weapon.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBack);
        });
    }

    public void BuyWeapon()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);

        SkinController.WeaponType currentWeapon = (SkinController.WeaponType)shopWeaponID;
        int currentPrice = weaponPrices[(int)currentWeapon];

        if (UIManager.Ins.coinAmount >= currentPrice)
        {
            UIManager.Ins.coinAmount -= currentPrice;
            weaponShopInfo[currentWeapon] = WeaponState.Select;

            PlayerPrefs.SetInt(Constants.GetWeaponShopKey(currentWeapon), (int)WeaponState.Select);
            PlayerPrefs.Save();

            UpdateCoinAmount();
            UpdateWeaponShopState();
            ShowState();
        }
    }

    public void UpdateWeaponShopState()
    {
        List<SkinController.WeaponType> weaponKeys = new(weaponShopInfo.Keys);

        foreach (var weapon in weaponKeys)
        {
            if (UIManager.Ins.coinAmount < weaponPrices[(int)weapon] && weaponShopInfo[weapon] == WeaponState.CanBuy)
            {
                weaponShopInfo[weapon] = WeaponState.CantBuy;
            }
            else if (UIManager.Ins.coinAmount >= weaponPrices[(int)weapon] && weaponShopInfo[weapon] == WeaponState.CantBuy)
            {
                weaponShopInfo[weapon] = WeaponState.CanBuy;
            }
        }
    }

    private void ShowState()
    {
        foreach (Transform child in stateButton)
        {
            child.gameObject.SetActive((WeaponState)child.GetSiblingIndex() == weaponShopInfo[(SkinController.WeaponType)shopWeaponID]);
        }
    }

    public void SelectWeapon()
    {
        foreach (var weapon in new List<SkinController.WeaponType>(weaponShopInfo.Keys))
        {
            if (weaponShopInfo[weapon] == WeaponState.Equipped)
            {
                weaponShopInfo[weapon] = WeaponState.Select;
                PlayerPrefs.SetInt(Constants.GetWeaponShopKey(weapon), (int)WeaponState.Select);
            }
        }

        weaponShopInfo[(SkinController.WeaponType)shopWeaponID] = WeaponState.Equipped;
        PlayerPrefs.SetInt(Constants.GetWeaponShopKey((SkinController.WeaponType)shopWeaponID), (int)WeaponState.Equipped);
        PlayerPrefs.Save();

        EquipWeapon((SkinController.WeaponType)shopWeaponID);
        ShowState();
    }

    private void SetPriceText(int price)
    {
        canBuyText.text = price.ToString();
        cantBuyText.text = price.ToString();
    }

    private void UpdateCoinAmount()
    {
        coinAmount.text = UIManager.Ins.coinAmount.ToString();
        PlayerPrefs.SetInt("Score", UIManager.Ins.coinAmount);
        PlayerPrefs.Save();
    }
}