// created: 3/14/2016 2:52:41 PM
// author: Zhangwei

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class bag : MonoBehaviour
{
    Transform _friendListPanel;
    Transform _playerIconTemplate;
    UISprite _playerQuality;
    UISprite _playerIcon;
    UISprite _t;
    Transform _mainPanel;
    UISprite _gold;
    UILabel _goldCount;
    Transform _close;

    void Start()
    {
        _friendListPanel = UICGTools.FindChild(transform, "FriendListPanel");
        _playerIconTemplate = UICGTools.FindChild(_friendListPanel, "PlayerIconTemplate");
        _playerQuality = UICGTools.FindChild(_playerIconTemplate, "Sprite(PlayerQuality)").GetComponent<UISprite>();
        _playerIcon = UICGTools.FindChild(_playerIconTemplate, "Sprite(PlayerIcon)").GetComponent<UISprite>();
        _t = UICGTools.FindChild(_friendListPanel, "Sprite(t)").GetComponent<UISprite>();
        _mainPanel = UICGTools.FindChild(transform, "MainPanel");
        _gold = UICGTools.FindChild(_mainPanel, "Sprite(Gold)").GetComponent<UISprite>();
        _goldCount = UICGTools.FindChild(_mainPanel, "Label(GoldCount)").GetComponent<UILabel>();
        _close = UICGTools.FindChild(_mainPanel, "Button(Close)");

        UIEventListener.Get(_close.gameObject).onClick = OnClickClose;
    }

    void OnEnable()
    {
        RegisterMsgCallBack();
    }

    void OnDisable()
    {
        UnRegisterMsgCallBack();
    }

    void RegisterMsgCallBack()
    {

    }

    void UnRegisterMsgCallBack()
    {

    }

    void OnClickClose(GameObject go)
    {

    }

}
