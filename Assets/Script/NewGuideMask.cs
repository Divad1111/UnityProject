using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewGuideMask : MonoBehaviour
{
    public int _width = 2048;
    public int _height = 2048;

    public Vector2 _offset;
    public Vector2 _holeSize = new Vector2(100F, 100F);

    public UIAtlas _uiAtlas;
    public string _maskSpriteName;

    public UIWidget _uiTargetWidget;
	// Use this for initialization
	void Start () 
    {
	
	}


    [ContextMenu("Generate")]
    void Generate()
    {
        if (_uiTargetWidget != null)
        {
            ClearChildMask();
            GenerateMask(_uiTargetWidget.gameObject);
        }
    }
    void GenerateMask(GameObject maskObject)
    {
        //计算
        //var x = holePosition.x
        var holeWorldPosition = maskObject.transform.position;
        var holeLocalPositionInMask = transform.InverseTransformPoint(holeWorldPosition);

        //左边的Mask
        var leftMaskWidth = Mathf.Abs(-(_width * 0.5F) - holeLocalPositionInMask.x);
        var leftMaskX = holeLocalPositionInMask.x - leftMaskWidth * 0.5F - _holeSize.x * 0.25F + _offset.x;
        int leftMaskY = 0;


        //右边的Mask
        var rightMaskWidth = ((_width * 0.5F) - holeLocalPositionInMask.x);
        var rightMaskX = holeLocalPositionInMask.x + rightMaskWidth * 0.5F + _holeSize.x * 0.25F + _offset.x;
        int rightMaskY = 0;

        //上边的Mask
        var topMaskHeight = ((_height * 0.5F) - holeLocalPositionInMask.y);
        var topMaskX = holeLocalPositionInMask.x;
        var topMaskY = holeLocalPositionInMask.y + topMaskHeight * 0.5F + _holeSize.y * 0.25F + _offset.y;

        //下边的Mask
        var bottomMaskHeight = Mathf.Abs(-(_height * 0.5F) - holeLocalPositionInMask.y);
        var bottomMaskX = holeLocalPositionInMask.x;
        var bottomMaskY = holeLocalPositionInMask.y - bottomMaskHeight * 0.5F - _holeSize.y * 0.25F + _offset.y;

        CreateChildMask("Left", new Vector3(leftMaskX, leftMaskY, 0F), new Vector2(leftMaskWidth - _holeSize.x * 0.5F, _height));
        CreateChildMask("Right", new Vector3(rightMaskX, rightMaskY, 0F), new Vector2(rightMaskWidth - _holeSize.x * 0.5F, _height));
        CreateChildMask("Top", new Vector3(topMaskX, topMaskY, 0F), new Vector2(_holeSize.x, topMaskHeight - _holeSize.y * 0.5F));
        CreateChildMask("Bottom", new Vector3(bottomMaskX, bottomMaskY, 0F), new Vector2(_holeSize.x, bottomMaskHeight - _holeSize.y * 0.5F));
    }

    void CreateChildMask(string name, Vector3 pos, Vector2 size)
    {
        GameObject childMask = new GameObject(name);

        childMask.AddComponent<BoxCollider>().isTrigger = true;

        var uiSprite = childMask.AddComponent<UISprite>();
        if (uiSprite != null)
        {   
            uiSprite.autoResizeBoxCollider = true;
            uiSprite.width = (int)size.x;
            uiSprite.height = (int)size.y;
            uiSprite.atlas = _uiAtlas;
            uiSprite.spriteName = _maskSpriteName;
        }

        childMask.transform.parent = transform;
        childMask.transform.localPosition = pos;
        childMask.transform.localScale = Vector3.one;
    }


    void ClearChildMask()
    {
        List<GameObject> _destoryObject = new List<GameObject>();
        for (int i = 0; i < transform.childCount; ++i )
        {
            if (Application.isPlaying)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                _destoryObject.Add(transform.GetChild(i).gameObject);
            }
        }

        if (_destoryObject.Count > 0)
        {
            foreach(var destoryObject in _destoryObject)
            {
                Object.DestroyImmediate(destoryObject);
            }
        }

        _destoryObject.Clear();
    }

    void Update()
    {
        //if (_uiTargetWidget != null)
        //{
        //    ClearChildMask();
        //    _holeSize.x = _uiTargetWidget.width;
        //    _holeSize.y = _uiTargetWidget.height;
        //    GenerateMask(_uiTargetWidget.gameObject);
        //}
    }
}
