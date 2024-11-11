using System.Collections;
using UnityEngine;

public class PlayerController : Character, IInitializeVariables, IHit
{
    public static PlayerController instance;

    [SerializeField] private GameObject _Cycle;
    [SerializeField] private GameObject Reticle;
    [SerializeField] private Material[] CupMaterial;

    [HideInInspector] public FloatingJoystick _Joystick;
    [HideInInspector] public CharacterName KillerName;
    [HideInInspector] public int Level;
    private Vector3 positionToAttack;
    private IStatePlayer currentState;
    
    void Start()
    {
        InitializeVariables();
        GameManagement.Ins.characterList.Add(this);    //Thêm player vào trong list Character để quản lý
    }

    void Update()
    {
        ShowReticle();
        ObstacleFading();
        if (!IsDeath && GameManagement.Ins.gameState == GameManagement.GameState.gameStarted)
        {
            currentState?.OnExecute(this);
        }
        else if (GameManagement.Ins.gameState == GameManagement.GameState.gameWin) OnWin();
        _Cycle.transform.position = transform.position;
        if (GameManagement.Ins.isAliveAmount == 1 && !IsDeath) StartCoroutine(CheckGameVictory());
    }

    public override void Move()
    {
        if (GameManagement.Ins.gameState == GameManagement.GameState.gameStarted)
        {
            if (_Joystick.Horizontal != 0 || _Joystick.Vertical != 0) // Nếu vị trí joystick được di chuyển thì Move Player
            {
                Vector3 temp = transform.position;
                temp.x -= _Joystick.Vertical * Time.deltaTime * MoveSpeed;
                temp.z += _Joystick.Horizontal * Time.deltaTime * MoveSpeed;

                Vector3 moveDirection = new(temp.x - transform.position.x, 0, temp.z - transform.position.z);

                if (moveDirection != Vector3.zero)
                {
                    moveDirection.Normalize();
                    Quaternion toRotate = Quaternion.LookRotation(moveDirection);
                    transform.SetPositionAndRotation(temp, Quaternion.RotateTowards(transform.rotation, toRotate, 720 * Time.deltaTime));
                }

                enableToAttackFlag = true;
            }
        }
    }

    public void CheckIdleToPatrol()
    {
        if ((_Joystick.Horizontal != 0 || _Joystick.Vertical != 0) && !IsDeath) ChangeState(new StatePlayerPatrol());
    }

    public void CheckPatrolToIdle()
    {
        if ((_Joystick.Horizontal == 0 && _Joystick.Vertical == 0) && !IsDeath) ChangeState(new StatePlayerIdle());
    }

    public void CheckIdletoAttack()
    {

        if (enableToAttackFlag && FindNearistEnemy(AttackRange) != Vector3.zero && !IsDeath)
        {
            ChangeState(new StatePlayerAttack());
        }
    }

    public override void Attack()
    {
        transform.LookAt(positionToAttack);
        enableToAttackFlag = false;
        attackScript.SetID(gameObject.GetInstanceID(), opponentID);
        StartCoroutine(TurntoIdle());
    }

    IEnumerator TurntoIdle()
    {
        yield return new WaitForSeconds(0.5f);
        if (GameManagement.Ins.gameState == GameManagement.GameState.gameStarted && _Joystick.Horizontal == 0 && _Joystick.Vertical == 0 && !IsDeath) ChangeState(new StatePlayerIdle());
    }

    void ChangeAttackRange(float attackRange)
    {
        AttackRange = attackRange;
        _Cycle.transform.localScale = new Vector3(AttackRange, 1f, AttackRange);
    }

    #region Reticle
    void ShowReticle() //Hiện mục tiêu của Player
    {
        positionToAttack = FindNearistEnemy(AttackRange);
        if (positionToAttack != Vector3.zero)
        {
            Reticle.transform.position = new Vector3(positionToAttack.x, 0.1f, positionToAttack.z);
            Reticle.SetActive(true);
        }
        else
        {
            Reticle.SetActive(false);
        }
    }
    #endregion 
    
    #region Singleton
    void IInitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public void InitializeVariables()
    {
        AttackRange = 5f;
        AttackSpeed = 10;
        MoveSpeed = 6f;
        WeaponListCreate();                 //Khởi tạo danh sách vũ khí
        characterSkin.CreateListOfWeaponMaterial();       //Khởi tạo danh sách Material của vũ khí
        WeaponSwitching(SkinController.WeaponType.Hammer, characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Hammer_1]);
        UpdatePlayerItem();
        IInitializeSingleton();
        ChangeAttackRange(AttackRange);
        IsDeath = false;
        Level = 0;
        ChangeState(new StatePlayerIdle());
    }

    public void OnHit()
    {
        currentState.OnExit(this);
        OnDeath();
        Reticle.SetActive(false);
        IsDeath = true;
        GameManagement.Ins.killedAmount++;
        GameManagement.Ins.gameState = GameManagement.GameState.gameOver;
        if (SoundManagement.Ins.openSound) SoundManagement.Ins.PlaySFX(SoundManagement.Ins.dieAudio);
    }

    void ObstacleFading()
    {
        GameManagement.Ins.FindObstacles();
        foreach (Transform _obstacle in GameManagement.Ins.obstacle)
        {
            if (Vector3.Distance(transform.position, _obstacle.position) < 8f)
            {
                _obstacle.GetComponent<Renderer>().sharedMaterial = CupMaterial[1];
            }
            else
            {
                _obstacle.GetComponent<Renderer>().sharedMaterial = CupMaterial[0];
            }
        }
    }

    public override void AddLevel() //Mỗi lần bắn hạ đối thủ thì sẽ gọi hàm AddLevel
    {
        characterCanvasAnim.SetTrigger(Constants.ADD_LEVEL); //Chạy Anim +1 khi giết được 1 enemy
        Level++;
        transform.localScale = new Vector3(1f + 0.1f * Level, 1f + 0.1f * Level, 1f + 0.1f * Level);    //Khi tăng 1 level thì sẽ tăng Scale của Player thêm 10% so với kích thước khi Start game
        MoveSpeed = (1f + 0.05f * Level) * 5f;                                                          //Tốc độ di chuyển của Player tăng 5% so với khi Start game.
        ChangeAttackRange(1.05f * AttackRange);                                                         //Tăng 5% tầm bắn
        GameManagement.Ins.mainCamera.fieldOfView += 2;
        if (SoundManagement.Ins.openSound) SoundManagement.Ins.PlaySFX(SoundManagement.Ins.sizeUpAudio);
    }

    public void WeaponSwitching(SkinController.WeaponType _weaponType, Material[] _weaponMaterial)
    {
        AttackRange = 5f;
        AttackSpeed = 10;
        MoveSpeed = 5f;

        for (int i = 0; i < characterSkin.weaponInHand.Length; i++)
        {
            if (i == (int)_weaponType)
            {
                characterSkin.weaponInHand[i].GetComponent<Renderer>().sharedMaterials = _weaponMaterial;
                characterSkin.weaponInHand[i].SetActive(true);
            }
            else
            {
                characterSkin.weaponInHand[i].SetActive(false);
            }
        }
        AddWeaponPower();
    }

    public void ChangeState(IStatePlayer state)
    {
        if (state != currentState)
        {
            currentState?.OnExit(this);
            currentState = state;
            currentState?.OnEnter(this);
        }
    }

    public void UpdatePlayerItem() // Trang bị clothes và weapon cho Player khi tắt đi bật lại
    {
        bool isWeaponEquipped = false;

        for (int i = 0; i < 12; i++)
        {
            SkinController.WeaponType weaponType = (SkinController.WeaponType)i;

            if (PlayerPrefs.GetInt(Constants.GetWeaponShopKey(weaponType), 0) == (int)WeaponState.Equipped)
            {
                isWeaponEquipped = true;

                Material[] weaponMaterialType = weaponType switch
                {
                    SkinController.WeaponType.Arrow => characterSkin._weapon.ArrowDefaultMaterials,
                    SkinController.WeaponType.RedAxe => characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Axe2_2],
                    SkinController.WeaponType.BlueAxe => characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Axe1_2],
                    SkinController.WeaponType.Boomerang => characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Boomerang_1],
                    SkinController.WeaponType.Candy001 => characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Candy4_2],
                    SkinController.WeaponType.Candy002 => characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Candy2_2],
                    SkinController.WeaponType.Candy003 => characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.candy1_1],
                    SkinController.WeaponType.Candy004 => characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Candy0_2],
                    SkinController.WeaponType.Hammer => characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Hammer_1],
                    SkinController.WeaponType.Knife => characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Knife_2],
                    SkinController.WeaponType.Uzi => characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Uzi_2],
                    SkinController.WeaponType.Z => characterSkin._weapon.ZDefaultMaterials,
                    _ => new Material[] { }
                };

                WeaponSwitching(weaponType, weaponMaterialType);
                break;
            }
        }

        if (!isWeaponEquipped)
        {
            SkinController.WeaponType defaultWeapon = SkinController.WeaponType.Hammer;
            Material[] defaultWeaponMaterial = characterSkin.ListWeaponMaterial[SkinController.WeaponMaterialsType.Hammer_1];

            WeaponSwitching(defaultWeapon, defaultWeaponMaterial);
        }

        for (int i = 0; i < 25; i++)
        {
            if (PlayerPrefs.GetInt(Constants.GetSkinShopKey((SkinController.ClothesType)i)) == 4)
            {
                characterSkin.ChangeClothes((SkinController.ClothesType)i);
            }
        }
    }

    IEnumerator CheckGameVictory()
    {
        yield return new WaitForSeconds(1);
        if (GameManagement.Ins.isAliveAmount == 1 && !IsDeath)
        {
            GameManagement.Ins.gameState = GameManagement.GameState.gameWin; //Chỉ còn 1 character còn sống và Player vẫn sống thì Victory
            if (SoundManagement.Ins.openSound) SoundManagement.Ins.PlaySFX(SoundManagement.Ins.winAudio);
        }
        else GameManagement.Ins.gameState = GameManagement.GameState.gameOver;
    }
}