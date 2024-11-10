using System.Collections;
using UnityEngine;
using TMPro;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private FloatingJoystick _Joystick;
    [SerializeField] private TextMeshProUGUI aliveAmount;
    [SerializeField] private GameObject guide;

    private void Awake()
    {
        GameObject.FindObjectOfType<PlayerController>()._Joystick = _Joystick;
    }

    void Update()
    {
        UpdateAliveNumber();
        if (guide.activeSelf)
        {
            if (Input.GetMouseButtonDown(0)) guide.SetActive(false);
        }
        if (GameManagement.Ins.gameState == GameManagement.GameState.gameOver)
        {
            StartCoroutine(GameOver());
            _Joystick.gameObject.SetActive(false);
        }
        else if (GameManagement.Ins.gameState == GameManagement.GameState.gameWin)
        {
            StartCoroutine(GameWin());
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        guide.SetActive(true);
        _Joystick.gameObject.SetActive(true);
    }

    public void OpenSetting()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        UIManager.Ins.OpenUI(UIName.Setting);
        Time.timeScale = 0f;
    }

    public void UpdateAliveNumber()
    {
        aliveAmount.text = "Alive: " + GameManagement.Ins.totalCharAlive;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        UIManager.Ins.OpenUI(UIName.GameOver);
    }

    IEnumerator GameWin()
    {
        yield return new WaitForSeconds(2);
        UIManager.Ins.OpenUI(UIName.Victory);
    }
}