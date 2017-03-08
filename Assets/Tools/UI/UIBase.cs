using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour, IUIController {

    public virtual void  OnOpen()
    {
       
    }

    public virtual void OnBecomeTopUI(UIType uiType)
    {
        
    }

    public virtual void OnBecomeSecondaryUI(UIType topUIType, string topUIName)
    {
        
    }

    public virtual void OnAfterPlayOpenAnimation()
    {
        
    }

    public virtual void OnBeforePlayCloseAnimation()
    { 

    }

    public virtual void OnClose()
    {
        
    }
}
