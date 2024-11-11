public class Constants
{
    #region ===== Weapon Tag =====
    public const string ARROW = "Arrow";
    public const string AXE_1 = "Axe_1";
    public const string AXE_2 = "Axe_2";
    public const string BOOMERANG = "boomerang";
    public const string CANDY_001 = "Candy 001";
    public const string CANDY_002 = "Candy 002";
    public const string CANDY_003 = "Candy 003";
    public const string CANDY_004 = "Candy 004";
    public const string HAMMER = "Hammer";
    public const string KNIFE = "knife";
    public const string UZI = "uzi";
    public const string Z = "Z";

    public const string PREFAB_PATH = "Prefabs/Weapon/ThrowWeapon/";

    #endregion

    #region ===== Characters Tag =====
    public const string PLAYER = "Player";
    public const string ENEMY = "Enemy";
    #endregion

    #region ===== Obstacle =====
    public const string OBSTACLE = "Obstacle";
    #endregion

    #region ===== Animation Tag =====
    public const string ATTACK = "Attack";
    public const string DANCE = "Dance";
    public const string IDLE = "Idle";
    public const string DEATH = "Death";
    public const string RUN = "Run";
    public const string WIN = "Win";
    public const string ULTI = "Ulti";
    public const string ADD_LEVEL = "AddLevel";
    #endregion

    #region ===== Sound =====
    public const string SoundStateKey = "SoundState";
    #endregion

    #region ===== Weapon Shop =====

    public static string GetWeaponShopKey(SkinController.WeaponType weaponType)
    {
        return "WeaponShop" + weaponType.ToString();
    }

    #endregion

    #region ===== Skin Shop =====

    public static string GetSkinShopKey(SkinController.ClothesType clothType)
    {
        return "ClothesShop" + clothType.ToString();
    }

    #endregion

    #region ===== Lock State =====

    public static string GetLockState(SkinController.ClothesType clothType)
    {
        return "ClothLockState" + clothType.ToString();
    }

    #endregion
}