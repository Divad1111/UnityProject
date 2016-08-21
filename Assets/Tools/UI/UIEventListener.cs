using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIEvtListener : EventTrigger
{
    public delegate void VoidDelegate (GameObject go);
    public delegate void BoolDelegate (GameObject go, bool state);
    //public delegate void FloatDelegate (GameObject go, float delta);
    public delegate void VectorDelegate (GameObject go, Vector2 delta);
    public delegate void ObjectDelegate (GameObject go, GameObject obj);
    public delegate void KeyCodeDelegate (GameObject go, KeyCode key);


    public VoidDelegate onSubmit;
    public VoidDelegate onClick;
    public VoidDelegate onDoubleClick;
    public BoolDelegate onHover;
    public BoolDelegate onPress;
    public BoolDelegate onSelect;
    public VectorDelegate onScroll;
    public VoidDelegate onDragStart;
    public VectorDelegate onDrag;
    public VoidDelegate onDragOver;
    public VoidDelegate onDragOut;
    public VoidDelegate onDragEnd;
    public ObjectDelegate onDrop;
    public KeyCodeDelegate onKey;
    public BoolDelegate onTooltip;

  


//    void OnDoubleClick ()           { if (isColliderEnabled && onDoubleClick != null) onDoubleClick(gameObject); }
//    void OnHover (bool isOver)      { if (isColliderEnabled && onHover != null) onHover(gameObject, isOver); }
//    void OnDragOver ()              { if (isColliderEnabled && onDragOver != null) onDragOver(gameObject); }
//    void OnDragOut ()               { if (isColliderEnabled && onDragOut != null) onDragOut(gameObject); }
//    void OnKey (KeyCode key)        { if (isColliderEnabled && onKey != null) onKey(gameObject, key); }
//    void OnTooltip (bool show)      { if (isColliderEnabled && onTooltip != null) onTooltip(gameObject, show); }
  

    static public UIEvtListener Get (GameObject go)
    {
        UIEvtListener listener = go.GetComponent<UIEvtListener>();
        if (listener == null) listener = go.AddComponent<UIEvtListener>();
        return listener;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onDragStart != null) onDragStart(gameObject);
    }
           
    public override void OnCancel(BaseEventData eventData)
    {
        
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject, true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject, false);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(gameObject, eventData.delta);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (onDrop != null) onDrop(gameObject, eventData.pointerEnter); 
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onDragEnd != null) onDragEnd(gameObject);    
    }

           
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (onDragStart != null) onDragStart(gameObject); 
    }
           
    public override void OnMove(AxisEventData eventData)
    {
        
    }
           
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
    }
           
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onPress != null) onPress(gameObject, true);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onPress != null) onPress(gameObject, false);
    }
           
    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }
           
    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }
           
    public override void OnScroll(PointerEventData eventData)
    {
        if (onScroll != null) onScroll(gameObject, eventData.scrollDelta); 
    }
           
    public override void OnSubmit(BaseEventData eventData)
    {
        if (onSubmit != null) onSubmit(gameObject);
    }
           
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        
    }
}
