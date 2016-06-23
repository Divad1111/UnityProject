using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GuideTrigger 
{
    public enum TriggerType
    {
        Unknow,
        FirstFightOver,
    }


    bool _isTrigger = false;
    bool _canRemove = false;

    List<object> _param = new List<object>();

    public virtual void Init(List<object> trgParam)
    {

    }

    public virtual void Reset()
    {
        _isTrigger = false;
    }

    public virtual void Update(float ft)
    {

    }

    public bool CanRemove()
    {
        return _canRemove;
    }

    public bool GetTriggerState()
    {
        return _isTrigger;
    }

    public virtual TriggerType GetTriggerType()
    {
        return TriggerType.Unknow;
    }

    public virtual void Destory()
    {

    }

    protected void SetTrggerState( bool isTrigger)
    {
        _isTrigger = isTrigger;
    }

    protected void SetCanRemove(bool canRemove)
    {
        _canRemove = canRemove;
    }

}
