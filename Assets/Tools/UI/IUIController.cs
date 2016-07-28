using UnityEngine;
using System.Collections;

public enum UIType
{
	Normal,	//不能看到下层UI的
	Popup, 	//能看到下层UI的，比如像一些对话框或有半透明的UI
}

public interface IUIController 
{
	void OnOpen ();
	void OnBecomeTopUI (UIType uiType);
	void OnBecomeSecondaryUI (UIType topUIType, string topUIName);
	//void OnBeforePlayOpenAnimation ();
	void OnAfterPlayOpenAnimation();
	void OnBeforePlayCloseAnimation ();
	//void OnAfterPlayCloseAnimation();
	void OnClose ();
}
