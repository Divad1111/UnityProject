using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMgr : MonoBehaviour
{
    Dictionary<string, UICfg> _uiCacheList = new Dictionary<string, UICfg> ();

    List<UICfg> _uiStack = new List<UICfg> ();

    const int UIInterval = 20;

    public GameObject UIRoot { get; private set; }

    public Camera UICamera { get; private set; }



    void Awake()
    {
        Instance = this;
    }

    static UIMgr _instance = null;

    public static UIMgr Instance {
        get
		{
            return _instance;
		}
        private set 
        {
            _instance = value;
        }
    } 

    void OnDestroy()
    {
        Instance = null;
    }

    public void AddUI(UICfg uiCfg)
    {
        if (_uiCacheList.ContainsKey (uiCfg.name))
        {
            Debug.LogError ("ui is exist.");
            return;
        }

        _uiCacheList.Add (uiCfg.name, uiCfg);

    }

    public void RemoveUI(string name)
    {
        UICfg cfg;
        if (_uiCacheList.TryGetValue (name, out cfg))
        {
            cfg.UnLoad ();
            _uiCacheList.Remove (name);
        }
    }

    public UICfg GetUI(string name)
    {
        UICfg cfg = null;
        _uiCacheList.TryGetValue (name, out cfg);
        return cfg;
    }

    public void Load()
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
        GameObject caremaGo = new GameObject ("Camera");
        var camera = caremaGo.AddComponent<Camera> ();
        camera.orthographic = true;
        camera.cullingMask = LayerMask.NameToLayer ("UI");
        camera.clearFlags = CameraClearFlags.Depth;
        camera.nearClipPlane = 0.3F;
        camera.farClipPlane = 1000F;

        caremaGo.AddComponent<PhysicsRaycaster> ();

    }

    public void OpenUI(string name, bool hideSecondaryUI = false, System.Action<GameObject> callback = null)
    {
        if (!CheckUIRoot ())
            CreateDefaultUIRoot ();

        UICfg uiCfg;
        if (!_uiCacheList.TryGetValue (name, out uiCfg))
        {
            Debug.LogError (string.Format("Don't found any UI of {0},Please call AddUI.", name));
            return;
        }

        var uiGo = AddUIToUIRoot (uiCfg);
        if (uiGo == null)
        {
            Debug.LogError ("Don't add ui to UIRoot.");
            return;
        }

        if (!CheckUIInterface (uiGo))
        {
            RemoveUIFromUIRoot (uiCfg.name);
            Debug.LogError ("Missing component of IUIController.");
            return;
        }

        if (!CheckComponent<Canvas> (uiGo))
        {
            RemoveUIFromUIRoot (uiCfg.name);
            Debug.LogError ("Missing component of Canvas.");
            return;
        }


        _uiStack.Add (uiCfg);
        var openingUIController = uiGo.GetComponent<IUIController> ();
        openingUIController.OnOpen ();
        openingUIController.OnBecomeTopUI (uiCfg.type);
        //openingUIController.On

        UICfg curUICfg = null;
        if (_uiStack.Count > 1)
        {
            curUICfg = _uiStack [_uiStack.Count - 1];

        }

		
    }

    bool CheckUIRoot()
    {
        return UIRoot == null || UICamera == null;
    }   

    static bool CheckUIInterface(GameObject go)
    {
        if (go == null)
            return false;

        return go.GetComponent<IUIController> () != null;
    }

    static bool CheckComponent<T>(GameObject go) where T:Component
    {
        if (go == null)
            return false;

        return go.GetComponent<T> () != null;
    }

    GameObject AddUIToUIRoot(UICfg uiCfg)
    {
        if (uiCfg == null)
            return null;

        if (!CheckSmaeName (UIRoot.transform, uiCfg.name))
        {
            Debug.LogError ("There be same ui name in UIRoot.");
            return null;
        }

        GameObject uiPrefab = null;
        if (uiCfg.prefab == null)
        {
            if (uiCfg.isCacheAsset)
            {   
                uiCfg.Load ();
                uiPrefab = uiCfg.instance;
            }
            else
            {
                uiPrefab = AssetsMgr.Instance.LoadUI (uiCfg.name);
            }
        }

        if (uiPrefab == null)
        {
            Debug.LogError ("Loading ui is failed of " + uiCfg.name );
            return null;
        }


        var prefabInstance = GameObject.Instantiate (uiPrefab) as GameObject;
        prefabInstance.name = uiCfg.name;
        prefabInstance.transform.SetParent (UIRoot.transform);
        prefabInstance.transform.localPosition = Vector3.zero;
        prefabInstance.transform.localScale = Vector3.one;
        prefabInstance.transform.localRotation = Quaternion.Euler (0F, 0F, 0F);

        uiCfg.instance = prefabInstance;

        return prefabInstance;
    }

    void RemoveUIFromUIRoot(string name)
    {
        if (CheckUIRoot ())
            return;

        var uiGo = UIRoot.transform.FindChild (name);
        if (uiGo == null)
            return;

        UICfg uiCfg = null;
        if (_uiCacheList.TryGetValue (name, out uiCfg))
        {
            uiCfg.instance = null;
        }
        GameObject.DestroyImmediate (uiGo.gameObject);
    }

    bool CheckSmaeName(Transform go, string name)
    {
        if (go == null)
            return false;

        return go.FindChild (name) == null;

    }

    void PlayAnimation(GameObject go, string name)
    {
        if (go == null || string.IsNullOrEmpty(name))
            return;

        var animatiinClip = AssetsMgr.Instance.LoadUIAnimation (name);
        if (animatiinClip == null)
        {
            Debug.LogError ("Loading ui animation is failed.");
            return;
        }
        
        Animation ani = go.GetComponent<Animation>();
        if(ani == null)
            ani = go.AddComponent<Animation> ();

        ani.playAutomatically = false;
        ani.cullingType = AnimationCullingType.AlwaysAnimate;
        ani.clip = animatiinClip;

        if (ani.isPlaying)
            ani.Stop ();
        
        ani.Play ();
    }

    void StartupDelayCallback(float time, System.Action<GameObject> callback)
    {
        //StartCoroutine ("Timer", new KeyValuePair<UICfg, System.Action<GameObject>>());

    }

//    IEnumerator Timer()
//    {
//        
//    }

    void BringToFront(GameObject go)
    {
        if (go == null)
            return;


    }

    void AdjustDistanceToCamera()
    {

    }

    void AdjustTransformOrder()
    {

    }

    public void Close()
    {
        
    }

    public void Close(string name)
    {
        
    }



}
