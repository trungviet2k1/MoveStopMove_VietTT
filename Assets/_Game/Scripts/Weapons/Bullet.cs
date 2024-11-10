using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 targetPos, ownerAttackPos;
    private float bulletSpeed;
    private float attackRange;
    private Rigidbody bullet;
    private int ownerID, opponentID;
    private Character ownerCharacter;

    void Start()
    {
        BulletMove();
    }

    private void OnEnable()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.attackAudio);
    }

    private void Update()
    {
        if (Vector3.Distance(ownerAttackPos, transform.position) > attackRange)
        {
            DestroyBullet();
        }
    }

    public void BulletMove()
    {
        if (bullet == null)
            bullet = GetComponent<Rigidbody>();

        Vector3 direction = targetPos - transform.position;
        bullet.velocity = direction.normalized * bulletSpeed;
        transform.LookAt(targetPos);
    }

    public void SetID(int _ownerID, int _oppenentID)
    {
        ownerID = _ownerID;
        opponentID = _oppenentID;
        ownerCharacter = FindCharacterByID(ownerID);

        if (ownerCharacter != null && !ownerCharacter.IsDeath)
        {
            attackRange = ownerCharacter.AttackRange;
            bulletSpeed = ownerCharacter.AttackSpeed;
            transform.localScale = ownerCharacter.transform.localScale;
            FindTarget();
        }
    }

    private Character FindCharacterByID(int id)
    {
        foreach (var character in GameManagement.Ins.characterList)
        {
            if (character.gameObject.GetInstanceID() == id && character.gameObject.activeSelf)
            {
                return character;
            }
        }
        return null;
    }

    public void FindTarget()
    {
        var opponentCharacter = FindCharacterByID(opponentID);
        if (opponentCharacter != null)
        {
            targetPos = opponentCharacter.transform.position;
            targetPos.y = 1f;
            ownerAttackPos = ownerCharacter.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() != ownerID)
        {
            if (other.CompareTag(Constants.ENEMY) || other.CompareTag(Constants.PLAYER))
            {
                var character = other.GetComponent<Character>();
                if (character != null && !character.IsDeath)
                {
                    character.GetComponent<IHit>().OnHit();
                    if (other.CompareTag(Constants.PLAYER))
                    {
                        SetKillerName(other);
                    }
                    AddOwnerLevel();
                    DestroyBullet();
                }
            }
        }
        else if (other.CompareTag(Constants.OBSTACLE))
        {
            SoundManagement.Ins.PlaySFX(SoundManagement.Ins.weaponImpact);
            DestroyBullet();
        }
    }

    private void SetKillerName(Collider playerCollider)
    {
        if (ownerCharacter != null && ownerCharacter.CompareTag(Constants.ENEMY))
        {
            var playerController = playerCollider.GetComponent<PlayerController>();
            playerController.KillerName = ownerCharacter.GetComponent<EnemyController>().enemyName;
        }
    }

    void AddOwnerLevel()
    {
        if (ownerCharacter != null && !ownerCharacter.IsDeath)
        {
            ownerCharacter.AddLevel();
        }
    }

    void DestroyBullet()
    {
        bullet.velocity = Vector3.zero;
        Pooling.instance.Push(gameObject.tag, gameObject);
    }
}