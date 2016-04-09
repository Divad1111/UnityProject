using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class Test : MonoBehaviour, IPointerClickHandler
{

    public bool canJump = false;
    public float jumpHeight = 0f;

    public UnityEvent _event;
    public event Action _action;

    //Animation ani =  null;
    // Use this for initialization
    void Start()
    {
        //ani = GetComponent<Animation>();
        DebugTest();
    }

	void OnGUI()
	{
		if (GUI.Button (new Rect(0, 0, 100f, 100f), "Test")) {
			_event.Invoke();
		}
	}

    public void OnClick()
    {
        Debug.LogError("OnClick");
    }
	
    //// Update is called once per frame
    //void Update () {
    //    ani.wrapMode = WrapMode.Loop;
	
    //}

    //void AnimationEventFunc()
    //{
    //    Debug.Log("动画事件触发");
    //}

    void OnClose(GameObject go)
    {
        UICamera.onClick += OnClose;
    }

    [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
    void DebugTest()
    {
        Debug.Log("Debug test.");
    }

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        print("click cube.");
    }
}
