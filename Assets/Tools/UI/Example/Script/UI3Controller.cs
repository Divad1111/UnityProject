﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI3Controller : MonoBehaviour, IUIController
{
    Button _btnOpen;
    Button _btnClose;
    // Use this for initialization
    void Start () 
    {
        _btnOpen = UITools.Find (transform, "Button(Open)").GetComponent<Button>();
        _btnClose = UITools.Find (transform, "Button(Close)").GetComponent<Button>();

        UIEventListener.Get (_btnOpen.gameObject).onClick = (go) =>
        {
            //UIMgr.Instance.OpenUI("UI2");
        };

        UIEventListener.Get (_btnClose.gameObject).onClick = (go) =>
        {
            UIMgr.Instance.Close();
        };
    }

    // Update is called once per frame
    void Update () 
    {

    }

    public virtual void OnOpen ()
    {
        Debug.Log ("Open UI3");
    }

    public virtual void OnBecomeTopUI(UIType uiType)
    {
        Debug.Log ("OnBecomeTopUI UI3");
    }

    public virtual void OnBecomeSecondaryUI (UIType topUIType, string topUIName)
    {
        Debug.Log ("OnBecomeSecondaryUI UI3");
    }

    public virtual void OnAfterPlayOpenAnimation()
    {
        Debug.Log ("OnAfterPlayOpenAnimation UI3");
    }

    public virtual void OnBeforePlayCloseAnimation ()
    {
        Debug.Log ("OnBeforePlayCloseAnimation UI3");
    }

    public virtual void OnClose ()
    {
        Debug.Log ("OnClose UI3");
    }

	
}