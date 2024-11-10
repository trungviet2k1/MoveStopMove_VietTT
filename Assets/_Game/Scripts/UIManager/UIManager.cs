using System.Collections.Generic;
using UnityEngine;

public enum UIName { BlockRayCast, CoinShop, GameOver, GamePlay, MainMenu, Setting, SkinShop, Victory, WeaponShop }  //NOTE: thứ tự trong UIName phải trùng với thứ tự trong Resources/Prefabs/UI
public enum PlayerRank { Wood, Silver, Gold }

public class UIManager : Singleton<UIManager>
{
    [Header("Main Canvas")]
    [SerializeField] protected Transform mainCanvas;

    [HideInInspector] public int coinAmount;
    [HideInInspector] public bool soundState, vibrationState;
    [HideInInspector] public int playerEXP;
    [HideInInspector] public PlayerRank playerRank;
    [HideInInspector] public int HoldingWeaponID;

    public Dictionary<UIName, UICanvas> canvasPrefabs = new();
    public Dictionary<UIName, UICanvas> canvasManagers = new();

    private void Awake()
    {
        coinAmount = 0;
        coinAmount = PlayerPrefs.GetInt("Score");
        soundState = true;
        vibrationState = true;
        playerRank = PlayerRank.Wood;
        playerEXP = 1;
    }
    
    void Start()
    {
        UICanvas[] canvas = Resources.LoadAll<UICanvas>("Prefabs/UI/");
        for (int i = 0; i < canvas.Length; i++)
        {
            canvasPrefabs.Add((UIName)i, canvas[i]);
        }
        OpenUI(UIName.MainMenu);
    }

    public UICanvas OpenUI(UIName name)
    {
        UICanvas canvas;
        CloseAllCanvas();
        if (!canvasManagers.ContainsKey(name) || canvasManagers[name] == null)  //Nếu không tồn tại Key name trong canvasManager hoặc canvasManagers[name] rỗng thì sinh ra canvasPrefabs mới để trong cái parent
        {
            canvas = Instantiate(canvasPrefabs[name], mainCanvas);
            canvasManagers.Add(name, canvas);
        }
        else
        {
            canvas = canvasManagers[name];
            canvas.gameObject.SetActive(true);
        }
        
        canvas.OnOpen();
        canvas.OnInit();
        return canvas;
    }

    public void CloseUI(UIName name)    
    {
        if (canvasManagers.ContainsKey(name)&& canvasManagers[name] != null)    //Kiểm tra nếu trong canvasManager có key Name và có Value thì chạy hàm Onclose.Không thì thôi
        {
            canvasManagers[name].OnClose();
        }
    }

    public bool IsOpened(UIName name) //Kiểm tra xem UI này có đang bật hay không.Nếu ko có trong list thì trả về false.Nếu có trong list thì check xem nó có đang bật hay không.
    {
        if (canvasManagers.ContainsKey(name) && canvasManagers[name] != null)    //Kiểm tra nếu trong canvasManager có key Name và có Value thì chạy hàm Onclose.Không thì thôi
        {
            if (canvasManagers[name].gameObject.activeSelf) return true;
        }
        return false;
    }

    public UICanvas GetUI(UIName name) //Mục đích để lấy UI trong canvasManager ra. Kiểm tra list xem có UI không. Nếu ko có thì trả về null. Nếu có thì trả về UI.
    {
        if (canvasManagers.ContainsKey(name) && canvasManagers[name] != null) return canvasManagers[name];
        return null;
    }

    public void CloseAllCanvas()
    {
        foreach (KeyValuePair<UIName, UICanvas> item in canvasManagers)
        {
            item.Value.OnClose();
        }
    }
}