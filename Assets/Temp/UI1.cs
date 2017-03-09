// created: 3/9/2017 10:53:12 AM
// author: Zhangwei

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


public class UI1 : UIBase
{
    Button _open;
    Button _close;
    Slider _testSlider;
    Toggle _testCheckBox;
    GameObject _template;
    InputField _inputField;

    void Awake()
    {
        _open = UITools.Find(transform, "Button(Open)").GetComponent<Button>();
        _close = UITools.Find(transform, "Button(Close)").GetComponent<Button>();
        _testSlider = UITools.Find(transform, "Slider(testSlider)").GetComponent<Slider>();
        _testCheckBox = UITools.Find(transform, "Toggle(testCheckBox)").GetComponent<Toggle>();
        _template = UITools.Find(transform, "Template").gameObject;
        _template.SetActive(false);
        _inputField = UITools.Find(transform, "InputField").GetComponent<InputField>();


        _open.onClick.AddListener(OnClickOpen);
        _close.onClick.AddListener(OnClickClose);
        _testSlider.onValueChanged.AddListener(OntestSliderChange);
        _testCheckBox.onValueChanged.AddListener(OntestCheckBoxChange);
    }



    void OnClickOpen( )
    {

    }

    void OnClickClose( )
    {

    }

    void OntestSliderChange(float value)
    {

    }

    void OntestCheckBoxChange(bool value)
    {

    }

}
