using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Character, IInitializeVariables, IHit
{
    #region Parameter
    [Header("Enemy Settings")]
    [SerializeField] private Animator enemyAnimation;
    [SerializeField] protected NavMeshAgent agent;
    public CharacterName enemyName;

    private Vector3 EnemyDestination;
    private float timeLimit;
    private float timeCouting;
    private IState currentState;

    [HideInInspector] public int Level;
    #endregion

    private void Awake()
    {
        enemyName = (CharacterName)Random.Range(0, 16);
        characterSkin.ChangeClothes((SkinController.ClothesType)Random.Range(0, 24));
    }

    void Start()
    {
        InitializeVariables();
        GameManagement.Ins.characterList.Add(this); //Tất cả các enemy được sinh ra sẽ được Add vào trong CharacterList này để quản lý.
    }

    void Update()
    {
        timeCouting += Time.deltaTime;
        if (!IsDeath && GameManagement.Ins.gameState == GameManagement.GameState.gameStarted || GameManagement.Ins.gameState == GameManagement.GameState.gameOver)
        {
            currentState?.OnExecute(this);
        }
    }

    public void EnemyMovement()
    {
        OnRun();
        if (GameManagement.Ins.gameState == GameManagement.GameState.gameStarted || GameManagement.Ins.gameState == GameManagement.GameState.gameOver)
        {
            agent.SetDestination(EnemyDestination);
            OnRun();
            enableToAttackFlag = true;
        }
    }

    public void EnemyStopMoving()
    {
        agent.SetDestination(transform.position);
    }

    public void FindNextDestination()
    {
        EnemyDestination = new Vector3(Random.Range(-24f, 24f), 0, Random.Range(-18.5f, 18.5f)); //Find the random position
    }

    public void CheckArriveDestination()
    {
        if (Vector3.Distance(transform.position, EnemyDestination) < 0.3f)
        {
            ChangeState(new StateEnemyIdle());
        }
    }

    public void ChangeState(IState state)   //Hàm chuyển đổi trạng thái State
    {
        if (state != currentState)
        {
            currentState?.OnExit(this);
            currentState = state;
            currentState?.OnEnter(this);
        }
    }

    public void RestartTimeCounting()
    {
        timeCouting = 0;
        timeLimit = Random.Range(1.5f, 3.5f);
    }

    public void CheckIdletoAttack()
    {
        if (FindNearistEnemy(AttackRange) != Vector3.zero && enableToAttackFlag) ChangeState(new StateEnemyAttack());
    }

    public void CheckPatroltoAttack()
    {
        if (FindNearistEnemy(AttackRange) != Vector3.zero && enableToAttackFlag && timeCouting > 2f) ChangeState(new StateEnemyAttack());
    }

    public void CheckIfAttackIsDone()
    {
        if (enemyAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 || timeCouting > 1.03f)
        {
            ChangeState(new StateEnemyIdle());
        }
    }

    public void CheckIdletoPatrol()
    {
        if (timeCouting > timeLimit)
        {
            ChangeState(new StateEnemyPatrol());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.OBSTACLE))
        {
            ChangeState(new StateEnemyPatrol());
        }
    }

    public void InitializeVariables()
    {
        AttackRange = 5f;
        AttackSpeed = 10;
        WeaponListCreate(); //Khởi tạo danh sách vũ khí
        WeaponSwitching((SkinController.WeaponType)Random.Range((int)SkinController.WeaponType.Arrow, (int)SkinController.WeaponType.Z));   //Đổi vũ khí và Material của vũ khí vào
        AddWeaponPower();
        IsDeath = false;
        Level = 0;
        OnResetAllTrigger();
        ChangeState(new StateEnemyIdle());
    }

    public override void Attack()
    {
        transform.LookAt(FindNearistEnemy(AttackRange));
        OnAttack();
        enableToAttackFlag = false;
        attackScript.SetID(gameObject.GetInstanceID(), opponentID);
    }

    public void OnHit()
    {
        if (SoundManagement.Ins.openSound) SoundManagement.Ins.PlaySFX(SoundManagement.Ins.dieAudio);
        currentState.OnExit(this);

        TargetManagement targetManagement = FindObjectOfType<TargetManagement>();
        if (targetManagement != null)
        {
            targetManagement.RemoveTarget(transform);
        }

        IsDeath = true;
        agent.SetDestination(transform.position);
        GameManagement.Ins.killedAmount++;
        OnDeath();
        StartCoroutine(EnemyDeath());
    }

    IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(2f);
        Pooling.instance.Push(gameObject.tag, gameObject);
    }

    public override void AddLevel()
    {
        characterCanvasAnim.SetTrigger(Constants.ADD_LEVEL);
        Level++;
        transform.localScale = new Vector3(1f + 0.1f * Level, 1f + 0.1f * Level, 1f + 0.1f * Level);
        agent.speed = (1f + 0.05f * Level) * 5f;
        AttackRange = 1.05f * AttackRange;
        if (SoundManagement.Ins.openSound) SoundManagement.Ins.PlaySFX(SoundManagement.Ins.sizeUpAudio);
    }

    public void WeaponSwitching(SkinController.WeaponType _weaponType)
    {
        for (int i = 0; i < characterSkin.weaponInHand.Length; i++)
        {
            if (i == (int)_weaponType)
            {
                Material[] CurrentWeaponMaterial = characterSkin.weaponInHand[i].GetComponent<Renderer>().sharedMaterials;
                Material temp = GetRandomWeaponMaterial(_weaponType);
                for (int j = 0; j < characterSkin.weaponInHand[i].GetComponent<Renderer>().sharedMaterials.Length; j++)
                {
                    CurrentWeaponMaterial[j] = temp;
                }
                characterSkin.weaponInHand[i].GetComponent<Renderer>().sharedMaterials = CurrentWeaponMaterial;
                characterSkin.weaponInHand[i].SetActive(true);
            }
            else
            {
                characterSkin.weaponInHand[i].SetActive(false);
            }
        }
    }
    #region Get Random Weapon Material

    public Material GetRandomWeaponMaterial(SkinController.WeaponType _weaponType)
    {
        return _weaponType switch
        {
            SkinController.WeaponType.Arrow => characterSkin._weapon.ArrowDefaultMaterials[Random.Range(0, characterSkin._weapon.ArrowDefaultMaterials.Length)],
            SkinController.WeaponType.RedAxe => characterSkin._weapon.Axe1DefaultMaterials[Random.Range(0, characterSkin._weapon.Axe1DefaultMaterials.Length)],
            SkinController.WeaponType.BlueAxe => characterSkin._weapon.Axe2DefaultMaterials[Random.Range(0, characterSkin._weapon.Axe2DefaultMaterials.Length)],
            SkinController.WeaponType.Boomerang => characterSkin._weapon.BoomerangDefaultMaterials[Random.Range(0, characterSkin._weapon.BoomerangDefaultMaterials.Length)],
            SkinController.WeaponType.Candy001 => characterSkin._weapon.Candy001DefaultMaterials[Random.Range(0, characterSkin._weapon.Candy001DefaultMaterials.Length)],
            SkinController.WeaponType.Candy002 => characterSkin._weapon.Candy002DefaultMaterials[Random.Range(0, characterSkin._weapon.Candy002DefaultMaterials.Length)],
            SkinController.WeaponType.Candy003 => characterSkin._weapon.Candy003DefaultMaterials[Random.Range(0, characterSkin._weapon.Candy003DefaultMaterials.Length)],
            SkinController.WeaponType.Candy004 => characterSkin._weapon.Candy004DefaultMaterials[Random.Range(0, characterSkin._weapon.Candy004DefaultMaterials.Length)],
            SkinController.WeaponType.Hammer => characterSkin._weapon.HammerDefaultMaterials[Random.Range(0, characterSkin._weapon.HammerDefaultMaterials.Length)],
            SkinController.WeaponType.Knife => characterSkin._weapon.KnifeDefaultMaterials[Random.Range(0, characterSkin._weapon.KnifeDefaultMaterials.Length)],
            SkinController.WeaponType.Uzi => characterSkin._weapon.UziDefaultMaterials[Random.Range(0, characterSkin._weapon.UziDefaultMaterials.Length)],
            SkinController.WeaponType.Z => characterSkin._weapon.ZDefaultMaterials[Random.Range(0, characterSkin._weapon.ZDefaultMaterials.Length)],
            _ => characterSkin._weapon.ArrowDefaultMaterials[Random.Range(0, characterSkin._weapon.ArrowDefaultMaterials.Length)],
        };
    }
    #endregion
}