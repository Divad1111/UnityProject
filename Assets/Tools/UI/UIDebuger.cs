using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIDebuger : MonoBehaviour
{
    public bool _debug = false;

    PointerEventData _pointerData = null;
    protected List<RaycastResult> _raycastResultCache = new List<RaycastResult>();

    string _curPath;

    void Update()
    {
        if (!_debug)
            return;

        if (EventSystem.current == null)
            return;

        //if (_pointerData == null)
            _pointerData = new PointerEventData (EventSystem.current);
        
        _pointerData.pointerId = PointerInputModule.kMouseLeftId;
        _pointerData.position = Input.mousePosition;
        _pointerData.button = PointerEventData.InputButton.Left;

        _raycastResultCache.Clear ();
        EventSystem.current.RaycastAll (_pointerData, _raycastResultCache);

        var firstRaycastResult = FindFirstRaycast (_raycastResultCache);

        if (firstRaycastResult.gameObject == null)
        {
            _curPath = string.Empty;
        }
        else
        {
            _curPath = UITools.GetPath (firstRaycastResult.gameObject);
        }
    }

    void OnGUI()
    {
        GUILayout.Label (_curPath);
    }

    protected static RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
    {
        for (var i = 0; i < candidates.Count; ++i)
        {
            if (candidates[i].gameObject == null)
                continue;

            return candidates[i];
        }
        return new RaycastResult();
    }


}


