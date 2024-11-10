using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasSetting : UICanvas
{
    public void ContinueButton()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        Time.timeScale = 1f;
        UIManager.Ins.OpenUI(UIName.GamePlay);
    }

    public void HomeButton()
    {
        SoundManagement.Ins.PlaySFX(SoundManagement.Ins.clickSound);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}