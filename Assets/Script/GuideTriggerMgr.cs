using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GuideTriggerMgr
{
    public event Action<int> StartGuide;

    Dictionary<int, List<GuideTrigger>> _guideTriggerMap = new Dictionary<int,List<GuideTrigger>>();

    bool _isPause = false;
    float _updateInterval = 1.0f;
    float _curTime = 0.0f;


    public void Init()
    {

    }

    void AddGuide()
    {

    }

    public void SetUpdateInterval( float time )
    {
        _updateInterval = time;
    }

    public void Update(float ft)
    {
        if (_isPause)
        {
            return;
        }

        if (StartGuide != null)
        {
            StartGuide(1);
        }   
    }

    void AddTrigger()
    {

    }

    public void ClearAllTrigger()
    {
        _guideTriggerMap.Clear();
    }

    public void ResetAll()
    {
        ForeachTrigger((trigger) =>
        {
            trigger.Reset();
        });       
    }

    public void Destory()
    {
        ForeachTrigger((trigger) =>
        {
            trigger.Destory();
        });
    }

    void ForeachTrigger(Action<GuideTrigger> callback)
    {
        if (callback == null)
        {
            return;
        }

        foreach (var triggers in _guideTriggerMap)
        {
            foreach (var trigger in triggers.Value)
            {
                callback(trigger);
            }
        }
    }

    public void Pause()
    {
        _isPause = true;
    }

    public void Resume()
    {
        _isPause = false;
    }

}
