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
		return Resources.Load(string.Format("UIPrefab/{0}", name)) as GameObject;
	}
}
