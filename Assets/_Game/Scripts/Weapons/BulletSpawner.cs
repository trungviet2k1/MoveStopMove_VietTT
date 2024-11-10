using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [HideInInspector] public Transform weaponParent;

    private void Awake()
    {
        if (weaponParent == null)
        {
            GameObject weaponObj = GameObject.Find("Weapon");
            if (weaponObj != null)
            {
                weaponParent = weaponObj.transform;
            }
        }
    }

    public void CreateBullet(Vector3 bulletPosition, int _ownerID, int _opponentID, Material[] _bulletMaterial)
    {
        GameObject bullet = Pooling.instance.Pull(gameObject.tag, GetPath(gameObject.tag));
        if (bullet == null) return;

        bullet.transform.position = bulletPosition;
        bullet.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterials = _bulletMaterial;
        bullet.transform.SetParent(weaponParent);

        Bullet _bullet = bullet.GetComponent<Bullet>();
        _bullet.SetID(_ownerID, _opponentID);
        _bullet.BulletMove();
    }

    string GetPath(string tag)
    {
        return tag switch
        {
            Constants.ARROW => Constants.PREFAB_PATH + "Arrow",
            Constants.AXE_1 => Constants.PREFAB_PATH + "Axe_1",
            Constants.AXE_2 => Constants.PREFAB_PATH + "Axe_2",
            Constants.BOOMERANG => Constants.PREFAB_PATH + "Boomerang",
            Constants.CANDY_001 => Constants.PREFAB_PATH + "Candy_001",
            Constants.CANDY_002 => Constants.PREFAB_PATH + "Candy_002",
            Constants.CANDY_003 => Constants.PREFAB_PATH + "Candy_003",
            Constants.CANDY_004 => Constants.PREFAB_PATH + "Candy_004",
            Constants.HAMMER => Constants.PREFAB_PATH + "Hammer",
            Constants.KNIFE => Constants.PREFAB_PATH + "Knife",
            Constants.UZI => Constants.PREFAB_PATH + "Uzi",
            _ => Constants.PREFAB_PATH + "Z",
        };
    }
}