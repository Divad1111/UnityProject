using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI1Controller : MonoBehaviour, IUIController
{
    Button _btnOpen;
    Button _btnClose;
    GameObject _cube;
    // Use this for initialization
    void Start () 
    {
        _btnOpen = UITools.Find (transform, "Button(Open)").GetComponent<Button>();
        _btnClose = UITools.Find (transform, "Button(Close)").GetComponent<Button>();
        _cube = UITools.Find (transform, "Cube").gameObject;


        UIEvtListener.Get (_btnOpen.gameObject).onClick = (go) =>
        {
            UIMgr.Instance.OpenUI("UI2");
        };

        UIEvtListener.Get (_btnClose.gameObject).onClick = (go) =>
        {
            UIMgr.Instance.Close();
        };

        UIEvtListener.Get (_cube).onClick = (go) =>
        {
            Debug.Log("Click Cube.....");
        };
    }



    public virtual void OnOpen ()
    {
        Debug.Log ("Open UI1");
    }

    public virtual void OnBecomeTopUI(UIType uiType)
    {
        Debug.Log ("OnBecomeTopUI UI1");
    }

    public virtual void OnBecomeSecondaryUI (UIType topUIType, string topUIName)
    {
        Debug.Log ("OnBecomeSecondaryUI UI1");
    }

    public virtual void OnAfterPlayOpenAnimation()
    {
        Debug.Log ("OnAfterPlayOpenAnimation UI1");
    }

    public virtual void OnBeforePlayCloseAnimation ()
    {
        Debug.Log ("OnBeforePlayCloseAnimation UI1");
    }

    public virtual void OnClose ()
    {
        Debug.Log ("OnClose UI1");
    }

	
}
