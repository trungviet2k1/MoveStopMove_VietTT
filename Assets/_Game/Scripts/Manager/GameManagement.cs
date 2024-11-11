using System.Collections.Generic;
using UnityEngine;

public class GameManagement : Singleton<GameManagement>, IInitializeVariables
{
    public enum GameState { gameUI, gameStarted, gameOver, gameWin, gamePause }

    [Header("Game State")]
    public GameState gameState;

    [Header("Game Environments")]
    public List<Transform> obstacle;

    [Header("Camera")]
    public Camera mainCamera;
    public Camera shopCamera;

    [HideInInspector] public int isAliveAmount, killedAmount, totalCharAlive, spawnAmount;
    [HideInInspector] public List<Character> characterList = new();

    private int mapID = 0;

    [Header("Map Controller")]
    public MapController mapController;
    public int totalCharacterAmount;

    void Awake()
    {
        SoundManagement.Ins.openSound = true;
        mapID = PlayerPrefs.GetInt("CurrentMapID", 0);
        LoadMap();
    }

    void Update()
    {
        isAliveAmount = IsAliveCounting();
        totalCharAlive = spawnAmount + isAliveAmount;
    }

    public void InitializeVariables()
    {
        gameState = GameState.gameUI;
        totalCharacterAmount.ToString();
        killedAmount = 0;
    }

    public int IsAliveCounting() // Số character đang có trên map
    {
        int IsAliveAmount = 0;
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].gameObject.activeSelf)
            {
                if (characterList[i].IsDeath == false) IsAliveAmount++;
            }
        }
        return IsAliveAmount;
    }

    public void LoadMap()
    {
        InitializeVariables();

        if (isAliveAmount == 1 && !PlayerController.instance.IsDeath)
        {
            mapID++;
        }

        if (mapID >= mapController.totalMap.Count)
        {
            mapID = 0;
        }

        PlayerPrefs.SetInt("CurrentMapID", mapID);
        PlayerPrefs.Save();

        if (mapController != null)
        {
            mapController.ClearCurrentMap();
            mapController.SpawnMap(mapID);
        }
    }

    public void FindObstacles()
    {
        obstacle.Clear();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(Constants.OBSTACLE))
        {
            obstacle.Add(obj.transform);
        }
    }
}