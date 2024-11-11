using UnityEngine;

public class EnemySpawner : MonoBehaviour, IInitializeVariables
{
    private enum SpawnArea { bottomLeft, bottomRight, upLeft, upRight }

    [SerializeField] protected int characterOnMap;
    [SerializeField] private TargetManagement targetManagement;

    private SpawnArea areaToSpawn;
    private int spawnAmount;

    void Start()
    {
        InitializeVariables();
        if (targetManagement == null)
        {
            targetManagement = FindObjectOfType<TargetManagement>();
        }
    }

    void Update()
    {
        FindAreaToSpawn();
        GameManagement.Ins.spawnAmount = spawnAmount;
    }

    public void InitializeVariables()
    {
        spawnAmount = GameManagement.Ins.totalCharacterAmount - 2;
    }

    #region Find the Area to Spawn Enemy
    void FindAreaToSpawn()
    {
        int upRight = 0, upLeft = 0, bottomRight = 0, bottomLeft = 0;
        Vector3 playerPosition;

        if (GameManagement.Ins.characterList == null || GameManagement.Ins.characterList.Count == 0) return;

        foreach (var character in GameManagement.Ins.characterList)
        {
            Vector3 charPos = character.transform.position;

            if (character.CompareTag(Constants.ENEMY))
            {
                if (charPos.x >= 0 && charPos.z >= 0) upRight++;
                else if (charPos.x < 0 && charPos.z > 0) upLeft++;
                else if (charPos.x > 0 && charPos.z < 0) bottomRight++;
                else bottomLeft++;
            }
            else if (character.CompareTag(Constants.PLAYER))
            {
                playerPosition = charPos;
                BlockPlayerArea(playerPosition, ref upRight, ref upLeft, ref bottomRight, ref bottomLeft);
            }
        }

        SelectAreaToSpawn(upRight, upLeft, bottomRight, bottomLeft);

        if (GameManagement.Ins.isAliveAmount < characterOnMap && spawnAmount > 0)
        {
            SpawnEnemy();
        }
    }

    // Khóa các khu vực gần Player để không spawn ở đó
    void BlockPlayerArea(Vector3 playerPos, ref int upRight, ref int upLeft, ref int bottomRight, ref int bottomLeft)
    {
        if (playerPos.x >= 0 && playerPos.z >= 0) upRight = GameManagement.Ins.totalCharacterAmount;
        else if (playerPos.x < 0 && playerPos.z > 0) upLeft = GameManagement.Ins.totalCharacterAmount;
        else if (playerPos.x > 0 && playerPos.z < 0) bottomRight = GameManagement.Ins.totalCharacterAmount;
        else bottomLeft = GameManagement.Ins.totalCharacterAmount;
    }

    // Chọn khu vực ít nhân vật nhất
    void SelectAreaToSpawn(int upRight, int upLeft, int bottomRight, int bottomLeft)
    {
        int minAmount = upRight;
        areaToSpawn = SpawnArea.upRight;

        if (upLeft < minAmount)
        {
            minAmount = upLeft;
            areaToSpawn = SpawnArea.upLeft;
        }
        if (bottomRight < minAmount)
        {
            minAmount = bottomRight;
            areaToSpawn = SpawnArea.bottomRight;
        }
        if (bottomLeft < minAmount)
        {
            areaToSpawn = SpawnArea.bottomLeft;
        }
    }

    // Hàm sinh Enemy
    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        if (!IsInAttackRange(spawnPosition))
        {
            GameObject gob = Pooling.instance.Pull("Enemy", "Prefabs/Enemy");
            gob.transform.position = spawnPosition;
            gob.transform.SetParent(transform);

            EnemyController enemyController = gob.GetComponent<EnemyController>();
            enemyController.IsDeath = false;
            enemyController.InitializeVariables();

            if (targetManagement != null)
            {
                targetManagement.AddTarget(gob.transform);
            }

            spawnAmount--;
        }
    }

    // Kiểm tra xem vị trí spawn có nằm trong vùng tấn công của bất kỳ nhân vật nào không
    bool IsInAttackRange(Vector3 spawnPosition)
    {
        foreach (var character in GameManagement.Ins.characterList)
        {
            if (Vector3.Distance(character.transform.position, spawnPosition) < character.AttackRange && !character.IsDeath)
            {
                return true;
            }
        }
        return false;
    }

    // Lấy vị trí ngẫu nhiên dựa trên khu vực spawn
    Vector3 GetRandomSpawnPosition()
    {
        return areaToSpawn switch
        {
            SpawnArea.upRight => new Vector3(Random.Range(5f, 24f), 0, Random.Range(5f, 18.5f)),
            SpawnArea.upLeft => new Vector3(Random.Range(-24f, -5f), 0, Random.Range(5f, 18.5f)),
            SpawnArea.bottomRight => new Vector3(Random.Range(5f, 24f), 0, Random.Range(-18.5f, -5f)),
            SpawnArea.bottomLeft => new Vector3(Random.Range(-24f, -5f), 0, Random.Range(-18.5f, -5f)),
            _ => Vector3.zero,
        };
    }
    #endregion
}