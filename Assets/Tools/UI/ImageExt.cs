using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageExt : Image 
{
    public bool isAdjustSize = true;

    BoxCollider _boxCollider;

    protected override void Start()
    {
        base.Start ();

        _boxCollider = GetComponent<BoxCollider> ();
    }

    [ExecuteInEditMode]
    void Update()
    {
        
        if (isAdjustSize)
        {
            if (_boxCollider != null)
                _boxCollider = GetComponent<BoxCollider> ();
            UITools.AdjustBoxColliderSize (_boxCollider);
        }
    }
}
