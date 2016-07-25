using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Test : EventTrigger {


	public override void OnPointerClick (PointerEventData eventData)
	{
		Debug.Log ("OnClick......" + gameObject.name);
	}
	
	
	[Header("This is test fields")]
	[Space(50)]
	public int fields = 1;


	[ContextMenu("PrintCanvasInfo")]
	void PrintScreenAndCanvas()
	{
		var canvasRect = GetComponentInParent<RectTransform> ();

      
		print ("ScreenWidth: " + Screen.width);
		print ("ScreenHeight: " + Screen.height);
		print ("CavasWidth: " + canvasRect.rect.width);
		print ("CanvasHeight: " + canvasRect.rect.height);
	}

//
//    Animation ani =  null;
//	// Use this for initialization
//	void Start () {
//	    ani = GetComponent<Animation>();
//	}
//	
//	// Update is called once per frame
//	void Update () {
//        ani.wrapMode = WrapMode.Loop;
//	
//	}
//
//    void AnimationEventFunc()
//    {
//        Debug.Log("动画事件触发");
//    }
}
