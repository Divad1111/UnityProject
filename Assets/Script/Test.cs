using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Test : EventTrigger 
{
	[Header("This is test fields")]
	[Space(50)]
	public int fields = 1;


	[ContextMenu("PrintCanvasInfo")]
	void PrintScreenAndCanvas()
	{
		var canvasRect = GetComponentInParent<RectTransform> ();
		print ("ScreenWidth:" + Screen.currentResolution.width);
		print ("ScreenHeight: " + Screen.currentResolution.width);
		print ("CavasWidth: " + canvasRect.rect.width);
		print ("CanvasHeight: " + canvasRect.rect.height);
	}


	GameObject errorButton;
	GameObject normalButton;

	// Use this for initialization
	void Start () 
	{
		
	}

	void LogEror()
	{
		Debug.LogError (gameObject.name);
	}
}
