using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour 
{
    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () 
    {
        var uiMgr = gameObject.AddMissingComponent<UIMgr> ();
        uiMgr.AddUIToCache (new UICfg ("UI1", UIType.Normal, "", "", true));
        uiMgr.AddUIToCache (new UICfg ("UI2", UIType.Normal, "", "", false));
        uiMgr.AddUIToCache (new UICfg ("UI3", UIType.Normal, "Open", "Close", true));

        uiMgr.Load ();

        uiMgr.OpenUI ("UI1");
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
