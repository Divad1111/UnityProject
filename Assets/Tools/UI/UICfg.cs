using UnityEngine;
using System.Collections;

public class UICfg 
{
	public string name { get; private set; }
	public UIType type { get; private set; }
	public string openAni { get; private set; }
	public string closeAni{ get; private set; }
	public bool isCacheAsset { get; private set; }
	public GameObject prefab { get; set; }
	public GameObject instance { get; set; }

	public UICfg(string name, UIType type, string openAni, string closeAni, bool isCacheAsset)
	{
		this.name = name;
		this.type = type;
		this.openAni = openAni;
		this.closeAni = closeAni;
		this.isCacheAsset = isCacheAsset;
	}

	public void Load()
	{
		if (!isCacheAsset)
			return;

		prefab = AssetsMgr.Instance.LoadUI (name);
	}

	public void UnLoad(bool clearInstance = false)
	{
		if (prefab != null) 
		{
			GameObject.Destroy (prefab);
			prefab = null;
		}

		if (clearInstance && instance != null) 
		{
			GameObject.Destroy (instance);
			instance = null;
		}
	}
}
