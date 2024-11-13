using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasVictory : UICanvas
{
    [Header("Effects")]
    [SerializeField] private Animator SliderAnim;

    [Header("UI")]
    [SerializeField] private GameObject Congratulation;
    [SerializeField] private GameObject Button;

    [Header("Presents")]
    [SerializeField] private Image[] Weapon;
    [SerializeField] private Image[] Clothes;

    private readonly List<SkinController.ClothesType> ClothesType = new();   //List Clothes chưa mua
    private readonly List<SkinController.WeaponType> WeaponType = new();  //List weapon chưa mua
    private bool WeaponOrClothes;   //Nếu là true thì Present là weapon, nếu là false thì present là Clothes
    private SkinController.ClothesType ClothesPresent;
    private SkinController.WeaponType WeaponPresent;
    private int PresentLoad;
    private PlayerController player;

    private void Awake()
    {
        PresentLoad = 0;
    }

    private void OnEnable()
    {
        Congratulation.SetActive(false);
    }

    public override void OnInit()
    {
        ClothesType.Clear();   //Tạo List mới chứa các item mà Player chưa có
        WeaponType.Clear();
        player = FindObjectOfType<PlayerController>();

        if (PlayerPrefs.GetInt("weaponOrClothes") == 1) WeaponOrClothes = true;
        else WeaponOrClothes = false;

        for (int i = 0; i < 12; i++)
        {
            if (PlayerPrefs.GetInt("WeaponShop" + (SkinController.WeaponType)i, 99) != 99)
            {
                if (PlayerPrefs.GetInt("WeaponShop" + (SkinController.WeaponType)i) != 3 && PlayerPrefs.GetInt("WeaponShop" + (SkinController.WeaponType)i) != 4)
                {
                    WeaponType.Add((SkinController.WeaponType)i);
                }
            }
        }
        for (int i = 0; i < 25; i++)
        {
            if (PlayerPrefs.GetInt("ClothesShop" + (SkinController.ClothesType)i, 99) != 99)
            {
                if (PlayerPrefs.GetInt("ClothesShop" + (SkinController.ClothesType)i) != 3 && PlayerPrefs.GetInt("ClothesShop" + (SkinController.ClothesType)i) != 4)
                {
                    ClothesType.Add((SkinController.ClothesType)i);
                }
            }
        }

        if (PlayerPrefs.GetInt("presentLoad", 99) != 99) PresentLoad = PlayerPrefs.GetInt("presentLoad");
        if (PlayerPrefs.GetInt("clothesPresent", 99) == 99 && PlayerPrefs.GetInt("weaponPresent", 99) == 99) RandomPresent();
        if (PlayerPrefs.GetInt("clothesPresent", 99) != 99)
        {
            ClothesPresent = (SkinController.ClothesType)PlayerPrefs.GetInt("clothesPresent");
        }
        if (PlayerPrefs.GetInt("weaponPresent", 99) != 99)
        {
            WeaponPresent = (SkinController.WeaponType)PlayerPrefs.GetInt("weaponPresent");
        }

        PresentLoad += 25;
        UIManager.Ins.coinAmount += player.Level;
        PlayerPrefs.SetInt("Score", UIManager.Ins.coinAmount);
        PlayerPrefs.SetInt("presentLoad", PresentLoad);
        PlayerPrefs.Save();

        StartCoroutine(PlayAnimation());
        ShowPresentImage();
    }

    // Update is called once per frame
    void Update()
    {
        if (PresentLoad > 100) GetPresent();
    }

    void GetPresent()   //Nếu đủ 100 điểm thì sẽ tự động nhận được present
    {
        if (!WeaponOrClothes)
        {
            PresentLoad = 0;
            PlayerPrefs.SetInt("presentLoad", PresentLoad);
            PlayerPrefs.SetInt("ClothesShop" + (SkinController.ClothesType)GetItemID((int)ClothesPresent), 3);
            PlayerPrefs.Save();
        }
        else if (WeaponOrClothes)
        {
            PresentLoad = 0;
            PlayerPrefs.SetInt("presentLoad", PresentLoad);
            PlayerPrefs.SetInt("WeaponShop" + WeaponPresent, 3);
            PlayerPrefs.Save();
        }
        RandomPresent();    //Sau khi nhận present thì lại Random chọn present mới
    }

    void RandomPresent()
    {
        if (Random.Range(0, 100) >= 50)
        {
            WeaponOrClothes = true;
            PlayerPrefs.SetInt("weaponOrClothes", 1);
            PlayerPrefs.Save();
        }
        else
        {
            WeaponOrClothes = false;
            PlayerPrefs.SetInt("weaponOrClothes", 0);
            PlayerPrefs.Save();
        }

        if (!WeaponOrClothes)
        {
            ClothesPresent = (SkinController.ClothesType)(Random.Range(0, ClothesType.Count));
            PlayerPrefs.SetInt("clothesPresent", (int)ClothesPresent);
            PlayerPrefs.Save();
        }
        else
        {
            WeaponPresent = (SkinController.WeaponType)(Random.Range(0, WeaponType.Count));
            PlayerPrefs.SetInt("weaponPresent", (int)WeaponPresent);
            PlayerPrefs.Save();
        }

    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(1);
        SliderAnim.SetTrigger("" + PresentLoad + "%");
        if (PresentLoad == 100)
        {
            GetPresent();
            Congratulation.SetActive(true);
        }
        StartCoroutine(ShowButton());
    }

    IEnumerator ShowButton()
    {
        yield return new WaitForSeconds(2);
        Button.SetActive(true);
    }

    void ShowPresentImage()
    {
        if (WeaponOrClothes)
        {
            for (int i = 0; i < Weapon.Length; i++)
            {
                if (i == (int)WeaponPresent) Weapon[i].gameObject.SetActive(true);
                else Weapon[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < Clothes.Length; i++)
            {
                Clothes[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < Clothes.Length; i++)
            {
                if (i == (int)ClothesPresent) Clothes[i].gameObject.SetActive(true);
                else Clothes[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < Weapon.Length; i++)
            {
                Weapon[i].gameObject.SetActive(false);
            }
        }
    }

    public void HomeButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManagement.Ins.LoadMap();
        UIManager.Ins.OpenUI(UIName.GamePlay);
    }

    #region Get ID of Item
    int GetItemID(int _id)
    {
        return _id switch
        {
            0 => 8,
            1 => 2,
            2 => 3,
            3 => 4,
            4 => 5,
            5 => 6,
            6 => 0,
            7 => 1,
            8 => 7,
            9 => 12,
            10 => 13,
            11 => 14,
            12 => 15,
            13 => 17,
            14 => 19,
            15 => 11,
            16 => 16,
            17 => 18,
            18 => 10,
            19 => 9,
            20 => 20,
            21 => 21,
            22 => 22,
            23 => 23,
            _ => 24,
        };
    }
    #endregion
}