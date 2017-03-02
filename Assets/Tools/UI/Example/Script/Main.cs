using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour 
{
    UIMgr _uiMgr;
    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () 
    {
		
		if (gameObject.GetComponent<UIMgr>() == null)
		{
			_uiMgr = gameObject.AddComponent<UIMgr> ();
		}
        _uiMgr.AddUIToCache (new UICfg ("UI1", UIType.Normal, "", "", false));
        _uiMgr.AddUIToCache (new UICfg ("UI2", UIType.Normal, "", "", false));
        _uiMgr.AddUIToCache (new UICfg ("UI3", UIType.Normal, "Open", "Close", false));

        _uiMgr.Load ();

        //uiMgr.OpenUI ("UI1");
	}

    void OnGUI()
    {
        if(GUILayout.Button("OpenUI1"))
        {
            _uiMgr.Open ("UI1");
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnDisable()
    {
        //UIMgr.Instance.UnLoad ();
    }

    void OnDestroy()
    {
        
    }
}
