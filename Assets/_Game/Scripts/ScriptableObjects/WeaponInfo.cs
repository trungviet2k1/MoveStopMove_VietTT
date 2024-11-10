using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/Weapon")]

public class WeaponInfo : ScriptableObject
{
    [Header("Weapon Type")]
    public GameObject[] WeaponType;

    [Header("Add Attack Range")]
    public float[] AddAttackRange;

    [Header("Add Attack Speed")]
    public float[] AddAttackSpeed;

    [Header("Arrow Materials")]
    public Material[] ArrowDefaultMaterials;

    [Header("Axe 1 Materials")]
    public Material[] Axe1DefaultMaterials;

    [Header("Axe 2 Materials")]
    public Material[] Axe2DefaultMaterials;

    [Header("Boomerang Materials")]
    public Material[] BoomerangDefaultMaterials;

    [Header("Candy 001 Materials")]
    public Material[] Candy001DefaultMaterials;

    [Header("Candy 002 Materials")]
    public Material[] Candy002DefaultMaterials;

    [Header("Candy 003 Materials")]
    public Material[] Candy003DefaultMaterials;

    [Header("Candy 004 Materials")]
    public Material[] Candy004DefaultMaterials;

    [Header("Hammer Materials")]
    public Material[] HammerDefaultMaterials;

    [Header("Knife Materials")]
    public Material[] KnifeDefaultMaterials;

    [Header("Uzi Materials")]
    public Material[] UziDefaultMaterials;

    [Header("Z Materials")]
    public Material[] ZDefaultMaterials;

    [Header("Custom Materials")]
    public Material[] CustomMaterials;
}