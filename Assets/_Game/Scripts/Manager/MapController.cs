using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("Map Prefabs")]
    public List<GameObject> totalMap;

    [Header("Spawned Position")]
    public Transform mapsParent;

    private GameObject currentMap;

    public void SpawnMap(int mapID)
    {
        if (mapID < 0 || mapID >= totalMap.Count) return;

        if (currentMap != null)
        {
            Destroy(currentMap);
        }

        currentMap = Instantiate(totalMap[mapID], mapsParent);
        currentMap.name = totalMap[mapID].name;
    }

    public void ClearCurrentMap()
    {
        if (currentMap != null)
        {
            Destroy(currentMap);
            currentMap = null;
        }
    }
}