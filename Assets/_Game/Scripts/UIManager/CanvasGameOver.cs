using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasGameOver : UICanvas
{
    [SerializeField] private Transform GuideText;
    [SerializeField] private TextMeshProUGUI RankText;
    [SerializeField] private TextMeshProUGUI KillerName;
    [SerializeField] private TextMeshProUGUI CoinAmount;
    private PlayerController playerController;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GuideText.gameObject.activeSelf)
        {
            SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
            GuideText.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.loseAudio);
        StartCoroutine(ShowGuide());
        playerController = FindObjectOfType<PlayerController>();
        RankText.text = "#" + GameManagement.Ins.totalCharAlive;
        KillerName.text = playerController.KillerName.ToString();
        CoinAmount.text = playerController.Level.ToString();
        UIManager.Ins.coinAmount += playerController.Level;
        PlayerPrefs.SetInt("Score", UIManager.Ins.coinAmount);
        PlayerPrefs.Save();
    }

    IEnumerator ShowGuide()
    {
        yield return new WaitForSeconds(3);
        GuideText.gameObject.SetActive(true);
    }
}