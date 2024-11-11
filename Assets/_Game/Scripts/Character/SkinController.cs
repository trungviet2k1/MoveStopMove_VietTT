using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    public enum WeaponType { Arrow, RedAxe, BlueAxe, Boomerang, Candy001, Candy002, Candy003, Candy004, Hammer, Knife, Uzi, Z }

    public enum WeaponMaterialsType
    {
        Arrow,
        Axe1, Axe1_2,
        Axe2, Axe2_2,
        Boomerang_1, Boomerang_2,
        Candy0_1, Candy0_2,
        candy1_1, candy1_2, candy1_3, candy1_4,
        Candy2_1, Candy2_2,
        Candy4_1, Candy4_2,
        Hammer_1, Hammer_2,
        Knife_1, Knife_2,
        Uzi_1, Uzi_2,
        Z,
        Azure, Black,
        Blue, Chartreuse,
        Cyan, Green,
        Magenta, Orange,
        Red, Rose,
        SpringGreen, Violet,
        White, Yellow
    }

    public enum ClothesType
    {
        Arrow, Cowboy, Crown, Ear, Hat, Hat_Cap, Hat_Yellow, HeadPhone, Rau, Khien, Shield,
        Batman, Chambi, comy, dabao, onion, pokemon, rainbow, Skull, Vantim,
        Devil, Angel, Witch, Deadpool, Thor
    }

    [HideInInspector]
    public enum SetFullOrNormal { SetFull, Normal }

    [Header("Character Skins")]
    public SetFullOrNormal lastClothes;
    public ClothesInfo CharacterClothes;
    public ClothesPower clothesPower;
    public EnemyRandomSkin _enemySkin;

    [Header("Weapon Skins")]
    public WeaponInfo _weapon;

    [Header("Body Settings")]
    public Transform ShieldPosition;
    public Transform LeftHandPosition;
    public Transform HeadPosition;
    public Transform TailPosition;
    public Transform BackPosition;
    public Renderer PantsPositionRenderer;
    public Renderer SkinPositionRenderer;
    public Transform weaponPosition; //GameObject chứa weapon trên tay Character.
    public GameObject[] weaponInHand = new GameObject[12]; //Mảng dùng để quản lý weapon trên tay Character

    [HideInInspector] public float AttackRange;
    [HideInInspector] public float AttackSpeed;
    [HideInInspector] public int EnemySkinID;

    public Dictionary<WeaponMaterialsType, Material[]> ListWeaponMaterial = new();

    #region Create List of Weapon Materials
    public void CreateListOfWeaponMaterial()
    {
        ListWeaponMaterial.Add(WeaponMaterialsType.Arrow, _weapon.ArrowDefaultMaterials);

        ListWeaponMaterial.Add(WeaponMaterialsType.Axe1, new Material[] { _weapon.Axe2DefaultMaterials[0], _weapon.Axe2DefaultMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Axe1_2, new Material[] { _weapon.Axe2DefaultMaterials[1], _weapon.Axe2DefaultMaterials[1] });

        ListWeaponMaterial.Add(WeaponMaterialsType.Axe2, new Material[] { _weapon.Axe1DefaultMaterials[0], _weapon.Axe1DefaultMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Axe2_2, new Material[] { _weapon.Axe1DefaultMaterials[1], _weapon.Axe1DefaultMaterials[1] });

        ListWeaponMaterial.Add(WeaponMaterialsType.Boomerang_1, new Material[] { _weapon.BoomerangDefaultMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Boomerang_2, new Material[] { _weapon.BoomerangDefaultMaterials[1] });

        ListWeaponMaterial.Add(WeaponMaterialsType.Candy0_1, new Material[] { _weapon.Candy004DefaultMaterials[0], _weapon.Candy004DefaultMaterials[0], _weapon.Candy004DefaultMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Candy0_2, new Material[] { _weapon.Candy004DefaultMaterials[1], _weapon.Candy004DefaultMaterials[1], _weapon.Candy004DefaultMaterials[1] });

        ListWeaponMaterial.Add(WeaponMaterialsType.candy1_1, new Material[] { _weapon.Candy003DefaultMaterials[0], _weapon.Candy003DefaultMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.candy1_2, new Material[] { _weapon.Candy003DefaultMaterials[1], _weapon.Candy003DefaultMaterials[1] });
        ListWeaponMaterial.Add(WeaponMaterialsType.candy1_3, new Material[] { _weapon.Candy003DefaultMaterials[2], _weapon.Candy003DefaultMaterials[2] });
        ListWeaponMaterial.Add(WeaponMaterialsType.candy1_4, new Material[] { _weapon.Candy003DefaultMaterials[3], _weapon.Candy003DefaultMaterials[3] });

        ListWeaponMaterial.Add(WeaponMaterialsType.Candy2_1, new Material[] { _weapon.Candy002DefaultMaterials[0], _weapon.Candy002DefaultMaterials[0], _weapon.Candy002DefaultMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Candy2_2, new Material[] { _weapon.Candy002DefaultMaterials[1], _weapon.Candy002DefaultMaterials[1], _weapon.Candy002DefaultMaterials[1] });

        ListWeaponMaterial.Add(WeaponMaterialsType.Candy4_1, new Material[] { _weapon.Candy001DefaultMaterials[0], _weapon.Candy001DefaultMaterials[0], _weapon.Candy001DefaultMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Candy4_2, new Material[] { _weapon.Candy001DefaultMaterials[1], _weapon.Candy001DefaultMaterials[1], _weapon.Candy001DefaultMaterials[1] });

        ListWeaponMaterial.Add(WeaponMaterialsType.Hammer_1, new Material[] { _weapon.HammerDefaultMaterials[0], _weapon.HammerDefaultMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Hammer_2, new Material[] { _weapon.HammerDefaultMaterials[1], _weapon.HammerDefaultMaterials[1] });

        ListWeaponMaterial.Add(WeaponMaterialsType.Knife_1, new Material[] { _weapon.KnifeDefaultMaterials[0], _weapon.KnifeDefaultMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Knife_2, new Material[] { _weapon.KnifeDefaultMaterials[1], _weapon.KnifeDefaultMaterials[1] });

        ListWeaponMaterial.Add(WeaponMaterialsType.Uzi_1, new Material[] { _weapon.UziDefaultMaterials[0], _weapon.UziDefaultMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Uzi_2, new Material[] { _weapon.UziDefaultMaterials[1], _weapon.UziDefaultMaterials[1] });

        ListWeaponMaterial.Add(WeaponMaterialsType.Z, _weapon.ZDefaultMaterials);

        ListWeaponMaterial.Add(WeaponMaterialsType.Azure, new Material[] { _weapon.CustomMaterials[0] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Black, new Material[] { _weapon.CustomMaterials[1] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Blue, new Material[] { _weapon.CustomMaterials[2] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Chartreuse, new Material[] { _weapon.CustomMaterials[3] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Cyan, new Material[] { _weapon.CustomMaterials[4] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Green, new Material[] { _weapon.CustomMaterials[5] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Magenta, new Material[] { _weapon.CustomMaterials[6] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Orange, new Material[] { _weapon.CustomMaterials[7] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Red, new Material[] { _weapon.CustomMaterials[8] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Rose, new Material[] { _weapon.CustomMaterials[9] });
        ListWeaponMaterial.Add(WeaponMaterialsType.SpringGreen, new Material[] { _weapon.CustomMaterials[10] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Violet, new Material[] { _weapon.CustomMaterials[11] });
        ListWeaponMaterial.Add(WeaponMaterialsType.White, new Material[] { _weapon.CustomMaterials[12] });
        ListWeaponMaterial.Add(WeaponMaterialsType.Yellow, new Material[] { _weapon.CustomMaterials[13] });
    }
    #endregion

    #region ChangeClothes
    public void ChangeClothes(ClothesType _ClothesType)
    {
        SetFullOrNormal _setfullOrNormal;
        _setfullOrNormal = ((int)_ClothesType > 19) ? (SetFullOrNormal.SetFull) : (SetFullOrNormal.Normal); //Xét xem ClothesType có phải Set full hay không
        if (_setfullOrNormal != lastClothes || _setfullOrNormal == SetFullOrNormal.SetFull) ResetClothes(); //Nếu là setfull mà bộ trước không phải setfull thì xóa hết để thay setfull vào
        switch (_ClothesType)
        {
            case ClothesType.Arrow:
                {
                    ResetHeadPosition();
                    Instantiate(CharacterClothes.HeadPosition[0], HeadPosition);
                    break;
                }
            case ClothesType.Cowboy:
                {
                    ResetHeadPosition();
                    Instantiate(CharacterClothes.HeadPosition[1], HeadPosition);
                    break;
                }
            case ClothesType.Crown:
                {
                    ResetHeadPosition();
                    Instantiate(CharacterClothes.HeadPosition[2], HeadPosition);
                    break;
                }
            case ClothesType.Ear:
                {
                    ResetHeadPosition();
                    Instantiate(CharacterClothes.HeadPosition[3], HeadPosition);
                    break;
                }
            case ClothesType.Hat:
                {
                    ResetHeadPosition();
                    Instantiate(CharacterClothes.HeadPosition[4], HeadPosition);
                    break;
                }
            case ClothesType.Hat_Cap:
                {
                    ResetHeadPosition();
                    Instantiate(CharacterClothes.HeadPosition[5], HeadPosition);
                    break;
                }
            case ClothesType.Hat_Yellow:
                {
                    ResetHeadPosition();
                    Instantiate(CharacterClothes.HeadPosition[6], HeadPosition);
                    break;
                }
            case ClothesType.HeadPhone:
                {
                    ResetHeadPosition();
                    Instantiate(CharacterClothes.HeadPosition[7], HeadPosition);
                    break;
                }
            case ClothesType.Rau:
                {
                    ResetHeadPosition();
                    Instantiate(CharacterClothes.HeadPosition[8], HeadPosition);
                    break;
                }
            case ClothesType.Khien:
                {
                    ResetShieldPosition();
                    Instantiate(CharacterClothes.LeftHandPosition[2], ShieldPosition);
                    break;
                }
            case ClothesType.Shield:
                {
                    ResetShieldPosition();
                    Instantiate(CharacterClothes.LeftHandPosition[3], ShieldPosition);
                    break;
                }
            case ClothesType.Batman:
                {
                    GetDefaultClothes();
                    PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[4];
                    break;
                }
            case ClothesType.Chambi:
                {
                    GetDefaultClothes();
                    PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[5];
                    break;
                }
            case ClothesType.comy:
                {
                    GetDefaultClothes();
                    PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[6];
                    break;
                }
            case ClothesType.dabao:
                {
                    GetDefaultClothes();
                    PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[7];
                    break;
                }
            case ClothesType.onion:
                {
                    GetDefaultClothes();
                    PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[8];
                    break;
                }
            case ClothesType.pokemon:
                {
                    GetDefaultClothes();
                    PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[9];
                    break;
                }
            case ClothesType.rainbow:
                {
                    GetDefaultClothes();
                    PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[10];
                    break;
                }
            case ClothesType.Skull:
                {
                    GetDefaultClothes();
                    PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[11];
                    break;
                }
            case ClothesType.Vantim:
                {
                    GetDefaultClothes();
                    PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[12];
                    break;
                }
            case ClothesType.Devil:
                {
                    Instantiate(CharacterClothes.HeadPosition[10], HeadPosition);
                    Instantiate(CharacterClothes.BackPosition[2], BackPosition);
                    Instantiate(CharacterClothes.TailPosition[0], TailPosition);
                    SkinPositionRenderer.sharedMaterial = CharacterClothes.SkinMaterials[3];
                    break;
                }
            case ClothesType.Angel:
                {
                    Instantiate(CharacterClothes.HeadPosition[9], HeadPosition);
                    Instantiate(CharacterClothes.BackPosition[0], BackPosition);
                    Instantiate(CharacterClothes.LeftHandPosition[0], LeftHandPosition);
                    SkinPositionRenderer.sharedMaterial = CharacterClothes.SkinMaterials[0];
                    break;
                }
            case ClothesType.Witch:
                {
                    Instantiate(CharacterClothes.HeadPosition[12], HeadPosition);
                    Instantiate(CharacterClothes.LeftHandPosition[1], LeftHandPosition);
                    SkinPositionRenderer.sharedMaterial = CharacterClothes.SkinMaterials[6];
                    break;
                }
            case ClothesType.Deadpool:
                {
                    Instantiate(CharacterClothes.BackPosition[1], BackPosition);
                    SkinPositionRenderer.sharedMaterial = CharacterClothes.SkinMaterials[1];
                    break;
                }
            case ClothesType.Thor:
                {
                    Instantiate(CharacterClothes.HeadPosition[11], HeadPosition);
                    SkinPositionRenderer.sharedMaterial = CharacterClothes.SkinMaterials[5];
                    break;
                }
        }
        lastClothes = _setfullOrNormal;
    }

    public void ResetClothes()
    {
        ResetShieldPosition();
        ResetLeftHandPosition();
        ResetHeadPosition();
        ResetBackPosition();
        ResetTailPosition();
        GetDefaultClothes();
    }

    public void GetDefaultClothes() //Thay đổi quần và màu skin về default
    {
        PantsPositionRenderer.sharedMaterial = CharacterClothes.PantsMaterials[3];
        if (gameObject.CompareTag(Constants.PLAYER)) SkinPositionRenderer.sharedMaterial = CharacterClothes.SkinMaterials[8]; //Nếu là Player thì cho màu vàng là default
        else //Nếu là Enemy thì chọn màu random
        {
            EnemySkinID = Random.Range(0, 21);
            SkinPositionRenderer.sharedMaterial = _enemySkin.EnemyColor[EnemySkinID];
        }
    }

    public void ResetShieldPosition()
    {
        foreach (Transform item in ShieldPosition)
        {
            Destroy(item.gameObject);
        }
    }

    public void ResetLeftHandPosition()
    {
        foreach (Transform item in LeftHandPosition)
        {
            Destroy(item.gameObject);
        }
    }

    public void ResetHeadPosition()
    {
        foreach (Transform item in HeadPosition)
        {
            Destroy(item.gameObject);
        }
    }

    public void ResetBackPosition()
    {
        foreach (Transform item in BackPosition)
        {
            Destroy(item.gameObject);
        }
    }

    public void ResetTailPosition()
    {
        foreach (Transform item in TailPosition)
        {
            Destroy(item.gameObject);
        }
    }
    #endregion
}