using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI3Controller : UIBase
{
    Button _btnOpen;
    Button _btnClose;
    // Use this for initialization
    void Start () 
    {
        _btnOpen = UITools.Find (transform, "Button(Open)").GetComponent<Button>();
        _btnClose = UITools.Find (transform, "Button(Close)").GetComponent<Button>();

        UIEvtListener.Get (_btnOpen.gameObject).onClick = (go) =>
        {
            //UIMgr.Instance.Open("UI3");
        };

        UIEvtListener.Get (_btnClose.gameObject).onClick = (go) =>
        {
            UIMgr.Instance.Close();
        };
    }

    // Update is called once per frame
    void Update () 
    {

    }

    public override void OnOpen ()
    {
        Debug.Log ("Open UI3");
    }

    public override void OnBecomeTopUI(UIType uiType)
    {
        Debug.Log ("OnBecomeTopUI UI3");
    }

    public override void OnBecomeSecondaryUI (UIType topUIType, string topUIName)
    {
        Debug.Log ("OnBecomeSecondaryUI UI3");
    }

    public override void OnAfterPlayOpenAnimation()
    {
        Debug.Log ("OnAfterPlayOpenAnimation UI3");
    }

    public override void OnBeforePlayCloseAnimation ()
    {
        Debug.Log ("OnBeforePlayCloseAnimation UI3");
    }

    public override void OnClose ()
    {
        Debug.Log ("OnClose UI3");
    }

	
}
