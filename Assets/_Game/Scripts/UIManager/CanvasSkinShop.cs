using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum ShopType { HatShop, PantShop, ShieldShop, SetShop }

public enum ClothState { CantBuy, CanBuy, Select, Equipped }

public enum ClothLockState { Lock, UnLock }

public class CanvasSkinShop : UICanvas
{
    [Header("Price")]
    [SerializeField] int[] ClothesPrice = { 100, 120, 150, 160, 180, 200, 220, 250, 280, 250, 290, 300, 330, 350, 380, 400, 450, 550, 700, 850, 1500, 2000, 2500, 3000, 3500 };

    [Header("Top Root")]
    [SerializeField] protected RectTransform totalCoin;
    [SerializeField] protected TextMeshProUGUI coinAmount;

    [Header("SkinShop UI")]
    [SerializeField] protected RectTransform shopBackground;
    [SerializeField] protected RectTransform listItem;
    [SerializeField] protected RectTransform[] shopIcon;
    [SerializeField] protected RectTransform[] shopScrollView;
    [SerializeField] protected Image[] _Frame;
    [SerializeField] protected Image[] _Lock;

    [Header("Buttons")]
    [SerializeField] protected Button[] shopButton;
    [SerializeField] protected GameObject[] stateButton;
    [SerializeField] protected TextMeshProUGUI cantBuyText;
    [SerializeField] protected TextMeshProUGUI canBuyText;

    [Header("Duration Animation")]
    [SerializeField] protected float duration;

    private int clothesID = 0;
    private Vector2 totalCoinPos;
    private Vector2 shopBackgroundPos;

    readonly Dictionary<SkinController.ClothesType, ClothState> clothesShopInfo = new();
    readonly Dictionary<SkinController.ClothesType, ClothLockState> clothesLockInfo = new();

    private void Awake()
    {
        totalCoinPos = totalCoin.anchoredPosition;
        shopBackgroundPos = shopBackground.anchoredPosition;

        for (int i = 0; i < ClothesPrice.Length; i++)    //Tạo List để quản lý trạng thái của từng sản phẩm
        {
            clothesShopInfo.Add((SkinController.ClothesType)i, ClothState.CantBuy);
            clothesLockInfo.Add((SkinController.ClothesType)i, ClothLockState.Lock);
        }
    }

    private void OnEnable()
    {
        MoveElementsIntoView();

        for (int i = 0; i < ClothesPrice.Length; i++)
        {
            int clothState = PlayerPrefs.GetInt(Constants.GetSkinShopKey((SkinController.ClothesType)i));
            int lockState = PlayerPrefs.GetInt(Constants.GetLockState((SkinController.ClothesType)i));

            // Clothes state
            if (clothState == 1) // Cannot buy
                clothesShopInfo[(SkinController.ClothesType)i] = ClothState.CantBuy;
            else if (clothState == 3) // Selected
                clothesShopInfo[(SkinController.ClothesType)i] = ClothState.Select;
            else if (clothState == 4) // Equipped
                clothesShopInfo[(SkinController.ClothesType)i] = ClothState.Equipped;

            // Lock state
            if (lockState == 1)  // Unlock
            {
                clothesLockInfo[(SkinController.ClothesType)i] = ClothLockState.UnLock;
            }
            else
            {
                clothesLockInfo[(SkinController.ClothesType)i] = ClothLockState.Lock;
            }
        }

        UpdateUIState();
    }

    private void MoveElementsIntoView()
    {
        totalCoin.anchoredPosition = new Vector2(Screen.width, totalCoinPos.y);
        shopBackground.anchoredPosition = new Vector2(shopBackgroundPos.x, -Screen.height);

        totalCoin.DOAnchorPos(totalCoinPos, duration).SetEase(Ease.OutExpo);
        shopBackground.DOAnchorPos(shopBackgroundPos, duration).SetEase(Ease.OutExpo);
    }

    private void OnDisable()
    {
        totalCoin.anchoredPosition = totalCoinPos;
        shopBackground.anchoredPosition = shopBackgroundPos;

        GameManagement.Ins.mainCamera.gameObject.SetActive(true);
        GameManagement.Ins.shopCamera.gameObject.SetActive(false);
        GameObject.FindObjectOfType<PlayerController>().OnIdle();
    }

    public override void OnInit()
    {
        coinAmount.text = UIManager.Ins.coinAmount.ToString();
        OnShopButtonClick(0);
        GetClothesID(8);

        GameManagement.Ins.mainCamera.gameObject.SetActive(false);
        GameManagement.Ins.shopCamera.gameObject.SetActive(true);
        GameObject.FindObjectOfType<PlayerController>().OnDance();
    }

    public void XButton()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        UIManager.Ins.OpenUI(UIName.MainMenu);
    }

    #region Show Shop

    public void OnShopButtonClick(int shopIndex)
    {
        switch (shopIndex)
        {
            case 0:
                GetClothesID(8);
                OpenShop(ShopType.HatShop);
                break;
            case 1:
                GetClothesID(12);
                OpenShop(ShopType.PantShop);
                break;
            case 2:
                GetClothesID(10);
                OpenShop(ShopType.ShieldShop);
                break;
            case 3:
                GetClothesID(20);
                OpenShop(ShopType.SetShop);
                break;
        }
    }

    void OpenShop(ShopType _shopType)
    {
        for (int i = 0; i < listItem.childCount; i++)
        {
            if (i == (int)_shopType)
            {
                listItem.GetChild(i).gameObject.SetActive(true);
                shopIcon[i].gameObject.SetActive(false);
                shopScrollView[i].gameObject.SetActive(true);
            }
            else
            {
                shopIcon[i].gameObject.SetActive(true);
                shopScrollView[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    public void GetClothesID(int id)
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        clothesID = id;
        ChangePrice();
    }

    #region Get ID
    public void GetArrowID() { GetClothesID(0); }
    public void GetCowboyID() { GetClothesID(1); }
    public void GetCrownID() { GetClothesID(2); }
    public void GetEarID() { GetClothesID(3); }
    public void GetHatID() { GetClothesID(4); }
    public void GetHatCapID() { GetClothesID(5); }
    public void GetHatYellowID() { GetClothesID(6); }
    public void GetHeadphoneID() { GetClothesID(7); }
    public void GetRauID() { GetClothesID(8); }
    public void GetKhienID() { GetClothesID(9); }
    public void GetShieldID() { GetClothesID(10); }
    public void GetBatmanID() { GetClothesID(11); }
    public void GetChambiID() { GetClothesID(12); }
    public void GetComyID() { GetClothesID(13); }
    public void GetDabaoID() { GetClothesID(14); }
    public void GetOnionID() { GetClothesID(15); }
    public void GetPikachuID() { GetClothesID(16); }
    public void GetRainBowID() { GetClothesID(17); }
    public void GetSkullID() { GetClothesID(18); }
    public void GetVantimID() { GetClothesID(19); }
    public void GetDevilID() { GetClothesID(20); }
    public void GetAngelID() { GetClothesID(21); }
    public void GetWitchID() { GetClothesID(22); }
    public void GetDeadpoolID() { GetClothesID(23); }
    public void GetThorID() { GetClothesID(24); }

    #endregion

    void ChangePrice()
    {
        for (int i = 0; i < ClothesPrice.Length; i++)
        {
            if (i == clothesID)
            {
                cantBuyText.text = ClothesPrice[i].ToString();
                canBuyText.text = ClothesPrice[i].ToString();
                _Frame[i].gameObject.SetActive(true);
            }
            else
            {
                _Frame[i].gameObject.SetActive(false);
            }
        }
        UpdateButtonState();
    }

    public void BuyClothes()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);

        if (UIManager.Ins.coinAmount >= ClothesPrice[clothesID])
        {
            UIManager.Ins.coinAmount -= ClothesPrice[clothesID];
            clothesShopInfo[(SkinController.ClothesType)clothesID] = ClothState.Select;
            clothesLockInfo[(SkinController.ClothesType)clothesID] = ClothLockState.UnLock;

            PlayerPrefs.SetInt(Constants.GetSkinShopKey((SkinController.ClothesType)clothesID), 3);
            PlayerPrefs.SetInt(Constants.GetLockState((SkinController.ClothesType)clothesID), 1);
            UpdateUIState();
        }

    }

    public void Equip()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);

        var clothesKeys = new List<SkinController.ClothesType>(clothesShopInfo.Keys);

        foreach (var key in clothesKeys)
        {
            if (clothesShopInfo[key] == ClothState.Equipped)
            {
                clothesShopInfo[key] = ClothState.Select;
                PlayerPrefs.SetInt(Constants.GetSkinShopKey(key), 3);
            }
        }

        if (clothesShopInfo[(SkinController.ClothesType)clothesID] == ClothState.Select)
        {
            clothesShopInfo[(SkinController.ClothesType)clothesID] = ClothState.Equipped;
            GameObject.FindObjectOfType<PlayerController>().characterSkin.ChangeClothes((SkinController.ClothesType)clothesID);
            PlayerPrefs.SetInt(Constants.GetSkinShopKey((SkinController.ClothesType)clothesID), 4);
        }

        UpdateUIState();
    }

    void UpdateUIState()
    {
        UpdateButtonState();
        UpdateCoinAmount();
        UpdateUnLockState();
    }

    void UpdateButtonState()
    {
        for (int i = 0; i < ClothesPrice.Length; i++)
        {
            if (UIManager.Ins.coinAmount < ClothesPrice[i] && clothesShopInfo[(SkinController.ClothesType)i] == ClothState.CanBuy)
            {
                clothesShopInfo[(SkinController.ClothesType)i] = ClothState.CantBuy;
            }
            else if (UIManager.Ins.coinAmount >= ClothesPrice[i] && clothesShopInfo[(SkinController.ClothesType)i] == ClothState.CantBuy)
            {
                clothesShopInfo[(SkinController.ClothesType)i] = ClothState.CanBuy;
            }
        }

        for (int i = 0; i < stateButton.Length; i++)
        {
            stateButton[i].SetActive(clothesShopInfo[(SkinController.ClothesType)clothesID] == (ClothState)i);
        }
    }

    void UpdateUnLockState()
    {
        for (int i = 0; i < ClothesPrice.Length; i++)
        {
            if (clothesLockInfo[(SkinController.ClothesType)i] == ClothLockState.UnLock)
            {
                _Lock[i].gameObject.SetActive(false);
            }
            else
            {
                _Lock[i].gameObject.SetActive(true);
            }
        }
    }

    void UpdateCoinAmount()
    {
        coinAmount.text = UIManager.Ins.coinAmount.ToString();
        PlayerPrefs.SetInt("Score", UIManager.Ins.coinAmount);
        PlayerPrefs.Save();
    }
}