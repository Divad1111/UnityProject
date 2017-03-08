using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI1Controller : UIBase
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



        _btnOpen.onClick.AddListener(() =>
        {
            UIMgr.Instance.Open("UI2");
        });

        _btnClose.onClick.AddListener(() =>
        {
            UIMgr.Instance.Close();
        });       

        UIEvtListener.Get (_cube).onClick = (go) =>
        {
            Debug.Log("Click Cube.....");
        };
    }



    public override void OnOpen ()
    {
        Debug.Log ("Open UI1");
    }

    public override void OnBecomeTopUI(UIType uiType)
    {
        Debug.Log ("OnBecomeTopUI UI1");
    }

    public override void OnBecomeSecondaryUI (UIType topUIType, string topUIName)
    {
        Debug.Log ("OnBecomeSecondaryUI UI1");
    }

    public override void OnAfterPlayOpenAnimation()
    {
        Debug.Log ("OnAfterPlayOpenAnimation UI1");
    }

    public override void OnBeforePlayCloseAnimation ()
    {
        Debug.Log ("OnBeforePlayCloseAnimation UI1");
    }

    public override void OnClose ()
    {
        Debug.Log ("OnClose UI1");
    }

	
}
