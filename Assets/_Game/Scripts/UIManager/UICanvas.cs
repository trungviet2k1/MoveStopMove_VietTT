using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [Header("Title")]
    public UIName nameUI;

    public virtual void OnInit() { }

    public virtual void OnOpen()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);
    }
}