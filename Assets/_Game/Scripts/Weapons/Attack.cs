using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Weapon Tranform")]
    [SerializeField] private Transform weapon;
    private int ownerID, opponentID;

    public void HideWeapon()
    {
        foreach (Transform weapon in weapon)
        {
            var meshRenderer = weapon.GetComponent<MeshRenderer>();
            var bulletSpawner = weapon.GetComponent<BulletSpawner>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            if (bulletSpawner != null && weapon.gameObject.activeSelf)
            {
                Material[] bulletMaterial = weapon.GetComponent<Renderer>().sharedMaterials;
                bulletSpawner.CreateBullet(this.weapon.position, ownerID, opponentID, bulletMaterial);
            }
        }
        StartCoroutine(ShowWeapon(0.18f));
    }

    IEnumerator ShowWeapon(float _timeCounting)
    {
        yield return new WaitForSeconds(_timeCounting);
        foreach (Transform weapon in weapon)
        {
            weapon.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void SetID(int _ownerID, int _opponentID)
    {
        ownerID = _ownerID;
        opponentID = _opponentID;
    }
}