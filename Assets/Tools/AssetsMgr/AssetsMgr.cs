using UnityEngine;
using System.Collections;

public class AssetsMgr 
{
	static AssetsMgr _instance = null;

	public static AssetsMgr Instance 
	{
		get 
		{
			if (_instance == null)
				_instance = new AssetsMgr ();
			return _instance;
		}
	}

	AssetsMgr() {}

	public GameObject LoadUI(string name)
	{
        return Resources.Load<GameObject> (string.Format ("UIPrefab/{0}", name));
	}

    public AnimationClip LoadUIAnimation(string name)
    {
        return Resources.Load<AnimationClip> (string.Format ("UIAnimation/{0}", name));
    }
}
