using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMgr
{
	Dictionary<string, UICfg> _uiCacheList = new Dictionary<string, UICfg> ();

	List<UICfg> _uiStack = new List<UICfg> ();

	public GameObject UIRoot { get; private set; }
	public Camera UICamera { get; private set; }




	static UIMgr _instance = null;

	public static UIMgr Instance 
	{
		get
		{
			if (_instance == null)
				_instance = new UIMgr ();

			return _instance;
		}
	}

	UIMgr ()
	{
	}

	public void AddUI (UICfg uiCfg)
	{
		if (_uiCacheList.ContainsKey (uiCfg.name))
		{
			Debug.LogError ("ui is exist.");
			return;
		}

		_uiCacheList.Add (uiCfg.name, uiCfg);

	}

	public void RemoveUI (string name)
	{
		UICfg cfg;
		if (_uiCacheList.TryGetValue (name, out cfg))
		{
			cfg.UnLoad ();
			_uiCacheList.Remove (name);
		}
	}

	public UICfg GetUI (string name)
	{
		UICfg cfg = null;
		_uiCacheList.TryGetValue (name, out cfg);
		return cfg;
	}

	public void Load ()
	{
		foreach (var cfg in _uiCacheList)
		{
			cfg.Value.Load ();
		}
	}

	public void UnLoad(bool clearInstance = false)
	{
		foreach (var cfg in _uiCacheList)
		{
			cfg.Value.UnLoad (clearInstance);
		}
	}

	public static void CreateDefaultUIRoot()
	{
		//创建根对象
		GameObject uiRoot = new GameObject ("UI Root");
		uiRoot.transform.position = new Vector3 (2000F, 2000F, 2000F);

		//创建并设置相机
		GameObject caremaGo = new GameObject("Camera");
		var camera = caremaGo.AddComponent<Camera>();
		camera.orthographic = true;
		camera.cullingMask = LayerMask.NameToLayer ("UI");

		caremaGo.AddComponent<PhysicsRaycaster> ();

	}

	public void OpenUI()
	{
		
	}

}
