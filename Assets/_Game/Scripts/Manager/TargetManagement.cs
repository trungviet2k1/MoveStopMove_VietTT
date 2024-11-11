using System.Collections.Generic;
using UnityEngine;

public class TargetManagement : MonoBehaviour
{
    public GameObject targetIndicatorPrefab;
    public Camera mainCamera;
    public float offset;

    private readonly List<TargetIndicatorPoint> targetIndicator = new();
    private GameObject targetIndicatorContainer;

    void Start()
    {
        CreateTargetIndicatorContainer();
        FindAllTargets();
    }

    void Update()
    {
        if (GameManagement.Ins.gameState == GameManagement.GameState.gameStarted)
        {
            foreach (var indicator in targetIndicator)
            {
                indicator.UpdateTargetPosition();
            }
        }
        else
        {
            foreach (var indicator in targetIndicator)
            {
                indicator.gameObject.SetActive(false);
            }
        }
    }

    void CreateTargetIndicatorContainer()
    {
        targetIndicatorContainer = GameObject.Find("Target Indicator");

        if (targetIndicatorContainer == null)
        {
            targetIndicatorContainer = new GameObject("Target Indicator");
            targetIndicatorContainer.transform.SetParent(transform);
        }
    }

    void FindAllTargets()
    {
        List<Character> allCharacters = GameManagement.Ins.characterList;
        int enemyCount = allCharacters.Count - 1;

        foreach (var character in allCharacters)
        {
            if (character.CompareTag(Constants.PLAYER))
                continue;

            AddTarget(character.transform);
            if (--enemyCount <= 0) break;
        }
    }

    public void AddTarget(Transform targetTranform)
    {
        if (!HasTargetIndicator(targetTranform))
        {
            GameObject indicatorObj = Instantiate(targetIndicatorPrefab, targetIndicatorContainer.transform);
            TargetIndicatorPoint indicatorPoint = indicatorObj.GetComponent<TargetIndicatorPoint>();
            indicatorPoint.Initialize(targetTranform, mainCamera, offset);

            targetIndicator.Add(indicatorPoint);
        }
    }

    public void RemoveTarget(Transform targetTranform)
    {
        TargetIndicatorPoint indicatorPoint = targetIndicator.Find(indicator => indicator.Target == targetTranform);
        if (indicatorPoint != null)
        {
            targetIndicator.Remove(indicatorPoint);
            Destroy(indicatorPoint.gameObject);
        }
    }

    bool HasTargetIndicator(Transform targetTranform)
    {
        return targetIndicator.Exists(indicator => indicator.Target == targetTranform);
    }
}