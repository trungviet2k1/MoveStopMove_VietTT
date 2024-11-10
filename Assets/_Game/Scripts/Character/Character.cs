using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public enum CharacterName { ABI, Uniqlo, Bitis, Vinamilk, KoBaYaShi, Ford, Vinfast, ToYoTa, Yamato, Biden, Biladen, Vodka, Yamaha, Honda, Suzuki, NiShiNo, Furuki }

    public UnityAction OnAttack;
    public UnityAction OnRun;
    public UnityAction OnIdle;
    public UnityAction OnDeath;
    public UnityAction OnWin;
    public UnityAction OnDance;
    public UnityAction OnUlti;
    public UnityAction OnResetAllTrigger;

    [Header("Skin Settings")]
    public SkinController characterSkin;

    [Header("Animation Settings")]
    [SerializeField] private Animator anim;
    public Animator characterCanvasAnim;

    [Header("Attack Settings")]
    public Attack attackScript;

    [HideInInspector] public float AttackRange;
    [HideInInspector] public float AttackSpeed;
    [HideInInspector] public float MoveSpeed;
    [HideInInspector] public bool enableToAttackFlag = false;
    [HideInInspector] public float distanceToNearistEnemy;
    [HideInInspector] public Vector3 nearistEnemyPosition;
    [HideInInspector] public int opponentID;
    [HideInInspector] public int EnemySkinID;
    [HideInInspector] public bool IsDeath;

    public virtual void Attack() { }

    public virtual void Move() { }

    public virtual void AddLevel() { }

    public void WeaponListCreate() //Thêm vũ khí vào weaponList
    {
        for (int i = 0; i < characterSkin.weaponInHand.Length; i++)
        {
            characterSkin.weaponInHand[i] = characterSkin.weaponPosition.GetComponent<Transform>().transform.GetChild(i).gameObject;
        }
    }

    public Vector3 FindNearistEnemy(float attackRange)
    {
        distanceToNearistEnemy = 1000f;
        for (int i = 0; i < GameManagement.Ins.characterList.Count; i++)
        {
            if (GameManagement.Ins.characterList[i].gameObject.GetInstanceID() != gameObject.GetInstanceID() && Vector3.Distance(GameManagement.Ins.characterList[i].gameObject.transform.position, gameObject.transform.position) < attackRange && GameManagement.Ins.characterList[i].gameObject.activeSelf)
            {
                if (Vector3.Distance(GameManagement.Ins.characterList[i].gameObject.transform.position, gameObject.transform.position) < distanceToNearistEnemy && GameManagement.Ins.characterList[i].IsDeath == false)
                {
                    distanceToNearistEnemy = Vector3.Distance(GameManagement.Ins.characterList[i].gameObject.transform.position, gameObject.transform.position);
                    nearistEnemyPosition = GameManagement.Ins.characterList[i].gameObject.transform.position;
                    opponentID = GameManagement.Ins.characterList[i].gameObject.GetInstanceID(); //Lấy ID của đối phương
                }
            }
        }
        if (distanceToNearistEnemy > 900f) return Vector3.zero;
        else return nearistEnemyPosition;
    }

    public void AddWeaponPower() //Đang cầm loại nào thì sẽ cộng thêm AttackRange và AttackSpeed tương ứng vào.
    {
        for (int i = 0; i < characterSkin.weaponInHand.Length; i++)
        {
            if (characterSkin.weaponInHand[i].activeSelf)
            {
                AttackRange += characterSkin._weapon.AddAttackRange[i];
                AttackSpeed += characterSkin._weapon.AddAttackSpeed[i];
                break;
            }
        }
    }
}