using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMgr : MonoBehaviour
{
    Dictionary<string, UICfg> _uiCacheList = new Dictionary<string, UICfg> ();

    List<UICfg> _uiStack = new List<UICfg> (10);

    //两个UI之间的间距，为了可以放置3D模型，所有UI之间间隔一定距离，所有在制作UI时UI的厚度不能超过UIDistance，也是就是前后各一半UIDistance/2
    public const int UIIntervalDistance = 1000;

    public const string UIRootName = "UI Root";
    public const string EventSystemName = "EventSystem";
    public const string CanvasName = "Canvas";
    public const string UICameraName = "Camera";


    public static GameObject UIRoot { get; private set; }

    public static Camera UICamera { get; private set; }

    public static Canvas UICanvas { get; private set; }



    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance);
            Debug.LogError("Don't exist multiple UIMgr!");
        }

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

    public void AddUIToCache(UICfg uiCfg)
    {
        if (_uiCacheList.ContainsKey (uiCfg.name))
        {
            Debug.LogError ("ui is exist.");
            return;
        }

        _uiCacheList.Add (uiCfg.name, uiCfg);

    }

    public void RemoveUIFromCache(string name)
    {
        UICfg cfg;
        if (_uiCacheList.TryGetValue (name, out cfg))
        {
            cfg.UnLoad ();
            _uiCacheList.Remove (name);
        }
    }

    public UICfg GetCacheUI(string name)
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

    public UICfg GetTopUI()
    {
        if (!CheckUIStack())
            return null;

        return _uiStack [_uiStack.Count - 1];
    }

    public static void CreateDefaultUIRoot()
    {
        //创建根对象
        GameObject uiRoot = new GameObject (UIRootName);
        uiRoot.transform.position = new Vector3 (2000F, 2000F, 2000F);

        var uiLayerIndex = LayerMask.NameToLayer("UI");
        uiRoot.layer = uiLayerIndex;

        //创建并设置相机
        GameObject caremaGo = new GameObject (UICameraName);
        var camera = caremaGo.AddComponent<Camera> ();
        camera.orthographic = true;
        camera.cullingMask = 1 << uiLayerIndex;
        camera.clearFlags = CameraClearFlags.Depth;
        camera.nearClipPlane = 0.3F;
        camera.farClipPlane = 1000F;

        caremaGo.transform.SetParent (uiRoot.transform, false);
        caremaGo.layer = uiLayerIndex;

        var phyRaycaster = caremaGo.AddMissingComponent<PhysicsRaycaster>();
        phyRaycaster.eventMask = 1 << uiLayerIndex;

        caremaGo.AddMissingComponent<UIDebuger> ();

        //创建事件系统
        GameObject eventSystemGo = new GameObject();
        eventSystemGo.name = EventSystemName;
        eventSystemGo.layer = uiLayerIndex;

        var eventSystem = eventSystemGo.AddMissingComponent<EventSystem> ();
        eventSystem.sendNavigationEvents = true;
        eventSystem.pixelDragThreshold = 5;

        eventSystemGo.AddMissingComponent<StandaloneInputModule> ();

        eventSystemGo.transform.SetParent (uiRoot.transform, false);

        //创建默认的Canvas
        var canvas = CreateDefaultCanvas(uiRoot, camera);

        //保存UIRoot,UI相机和UICanvas
        UIRoot = uiRoot;
        UICamera = camera;
        UICanvas = canvas;
    }

    public static Canvas CreateDefaultCanvas(GameObject uiRoot, Camera canvasCamera)
    {
        if (uiRoot.name != UIRootName)
        {
            Debug.LogError("Creating canvas must be UI Root");
            return null;
        }

        GameObject canvasGo = new GameObject();
        canvasGo.name = CanvasName;

        UITools.InitDefaultParamForCanvas(canvasGo, canvasCamera);

        canvasGo.transform.SetParent(uiRoot.transform, false);

        return canvasGo.GetComponent<Canvas>();
    }


    public void Open(string name, bool hideSecondaryUI = false, System.Action<GameObject> callback = null)
    {
        if (!CheckVaildForUI ())
            CreateDefaultUIRoot ();

        var topCfg = GetTopUI();
        if (topCfg != null && topCfg.IsOperating())
        {
            Debug.Log("Top ui is opening or closing.");
            return;
        }

        if (IsInUIStack(name))
        {
            Debug.Log(string.Format("Exist same UI of {0} in UIStack.", name));
            return;
        }

        UICfg uiCfg;
        if (!_uiCacheList.TryGetValue (name, out uiCfg))
        {
            Debug.LogError (string.Format("Don't found any UI of {0},Please call AddUI.", name));
            return;
        }

        var uiGo = AddUIToCanvas (uiCfg);
        if (uiGo == null)
        {
            Debug.LogError ("Don't add ui to UIRoot.");
            return;
        }

        if (!CheckLayer (uiGo, "UI"))
        {
            Debug.LogError ("Not in layer of UI.");
        }

        if (!CheckUIInterface (uiGo))
        {
            RemoveUIFromCanvas (uiCfg.name);
            Debug.LogError ("Missing component of UIBase.");
            return;
        }

        if (!CheckComponent<UIBase> (uiGo))
        {
            RemoveUIFromCanvas (uiCfg.name);
            Debug.LogError ("Missing component of Canvas.");
            return;
        }

        _uiStack.Add(uiCfg);

        AdjustDisplayOrder();

        uiCfg.state = UICfg.UIState.Opening;

        var openingUIController = uiGo.GetComponent<UIBase> ();
        openingUIController.OnOpen ();

        if (!string.IsNullOrEmpty (uiCfg.openAni))
        {
            bool playSuccess = PlayAnimation (uiGo, uiCfg.openAni, (go) =>
            {
                openingUIController.OnAfterPlayOpenAnimation ();
                OnOpen(uiCfg);
            });   

            if (!playSuccess)
            {
                openingUIController.OnAfterPlayOpenAnimation ();
                OnOpen (uiCfg);
            }
        } else
        {
            OnOpen(uiCfg);
        }
    }

    static bool CheckVaildForUI()
    {
        return UIRoot != null && UICamera != null && UICanvas != null;
    }   

    static bool CheckLayer(GameObject go, string targetLayerName)
    {
        if (go == null)
            return false;

        return go.layer == LayerMask.NameToLayer (targetLayerName);
    }

    bool IsInUIStack(string name)
    {
        for (int i = _uiStack.Count - 1; i >= 0; --i)
        {
            if (_uiStack [i].name == name)
                return true;
        }

        return false;
    }

    static bool CheckUIInterface(GameObject go)
    {
        if (go == null)
            return false;

        return go.GetComponent<UIBase> () != null;
    }

    static bool CheckComponent<T>(GameObject go)
    {
        if (go == null)
            return false;

        return go.GetComponent<T> () != null;
    }

    bool CheckUIStack()
    {
        return _uiStack != null && _uiStack.Count > 0;
    }

    GameObject AddUIToCanvas(UICfg uiCfg)
    {
        if (uiCfg == null)
            return null;

        if (!CheckSameName (UICanvas.transform, uiCfg.name))
        {
            Debug.LogError ("There be same ui name in UIRoot.");
            return null;
        }

        GameObject uiPrefab = uiCfg.prefab;
        if (uiCfg.prefab == null)
        {
            if (uiCfg.isCacheAsset)
            {   
                uiCfg.Load ();
                uiPrefab = uiCfg.prefab;
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
        var rectTrans = prefabInstance.transform as RectTransform;
        rectTrans.SetParent (UICanvas.transform);
        rectTrans.pivot = new Vector2(0.5F, 0.5F);
        rectTrans.localPosition = Vector3.zero;
        rectTrans.localScale = Vector3.one;
        rectTrans.localRotation = Quaternion.Euler (0F, 0F, 0F);

        uiCfg.instance = prefabInstance;

        return prefabInstance;
    }

    void RemoveUIFromCanvas(string name)
    {
        if (!CheckVaildForUI ())
            return;

        var uiGo = UICanvas.transform.FindChild (name);
        if (uiGo == null)
            return;

        UICfg uiCfg = null;
        if (_uiCacheList.TryGetValue (name, out uiCfg))
        {
            if (uiCfg.instance != null)
            {
                GameObject.DestroyImmediate (uiCfg.instance, true);
                uiCfg.instance = null;
            }
        }
    }

    bool CheckSameName(Transform go, string name)
    {
        if (go == null)
            return false;

        return go.FindChild (name) == null;

    }

    bool PlayAnimation(GameObject go, string name, System.Action<GameObject> callback)
    {
        if (go == null || string.IsNullOrEmpty(name))
            return false;

        var animationClip = AssetsMgr.Instance.LoadUIAnimation (name);
        if (animationClip == null)
        {
            Debug.LogError ("Loading ui animation is failed.");
            return false;
        }

        Animation ani = go.AddMissingComponent<Animation> ();
        ani.playAutomatically = false;
        ani.cullingType = AnimationCullingType.AlwaysAnimate;
        ani.AddClip (animationClip, name);
        ani.clip = animationClip;

        if (ani.isPlaying)
            ani.Stop ();
        
        ani.Play ();

        StartupDelayCallback (animationClip.length, go, callback);

        return true;
    }

    void StartupDelayCallback(float time, GameObject userData, System.Action<GameObject> callback)
    {
        var timer = gameObject.AddComponent<Timer> ();
        timer.time = time;
        timer.userData = userData;
        timer.OnTimer += (obj) =>
        {
            callback(obj as GameObject);
        };
    }

    void OnOpen( UICfg openingUICfg )
    {
        openingUICfg.state = UICfg.UIState.Opened;

        UICfg uiCfg = null;
        if (_uiStack.Count > 1)
        {
            uiCfg = _uiStack[_uiStack.Count - 2];

            if (uiCfg == null || uiCfg.instance == null)
            {
                Debug.LogError("Don't instance object.");
                return;
            }

            if (!CheckUIInterface(uiCfg.instance))
            {
                Debug.LogError("Missing component of UIBase.");
                return;
            }

            var uiController = uiCfg.instance.GetComponent<UIBase>();
            uiController.OnBecomeSecondaryUI(openingUICfg.type, openingUICfg.name);
        }
    }

    void BringToFront(string name)
    {   
        if (!CheckUIStack ())
            return;
        
        int findUICfgIndex = -1;
        for (int i = _uiStack.Count - 1; i >= 0; --i)
        {
            if (_uiStack [i].name == name)
            {
                findUICfgIndex = i;
                break;
            }
        }

        if (findUICfgIndex < 0)
        {
            Debug.LogError (name + " isn't in uiStatck. Don't bring to front.");
            return;
        }

        var tempCfg = _uiStack [findUICfgIndex];
        _uiStack.RemoveAt (findUICfgIndex);
        _uiStack.Add (tempCfg);

        AdjustDisplayOrder ();
    }

    void AdjustDisplayOrder( )
    {
        if (!CheckVaildForUI())
        {
            Debug.LogError ("Missing UIRoot or camera.");
            return;
        }

        if (!CheckUIStack ())
            return;

        for (int i = _uiStack.Count - 1; i >= 0; --i)
        {
            var uiCfg = _uiStack [i];
            var uiGo = uiCfg.instance;

            var localPos = uiGo.transform.localPosition;
            localPos.z = -UIIntervalDistance * i - 1;
            uiGo.transform.localPosition = localPos;

            //调节transform顺序
            uiGo.transform.SetSiblingIndex (i);
        }
    }

    public void Close()
    {
        if (!CheckUIStack())
            return;

        var uiCfg = _uiStack [_uiStack.Count - 1];
        
        Close (uiCfg.name);
    }

    public void Close(string name)
    {
        if (!CheckUIStack ())
            return;

        var topCfg = GetTopUI();
        if (topCfg != null && topCfg.IsOperating())
        {
            Debug.Log("Top ui is opening or closing.");
            return;
        }

        for (int i = _uiStack.Count - 1; i >= 0; --i)
        {
            var uiCfg = _uiStack [i];
            if (uiCfg.name != name)
                continue;

            if (uiCfg.instance == null)
                continue;
            
            if (!CheckUIInterface (uiCfg.instance))
            {
                Debug.LogError ("Missing component of UIBase.");
                continue;
            }

            uiCfg.state = UICfg.UIState.Closing;

            if (!string.IsNullOrEmpty (uiCfg.closeAni))
            {
                var uiController = uiCfg.instance.GetComponent<UIBase> ();
                uiController.OnBeforePlayCloseAnimation ();

                bool playSuccess = PlayAnimation (uiCfg.instance, uiCfg.closeAni, (go) =>
                {
                    OnClose(uiCfg);
                });

                if (!playSuccess)
                    OnClose (uiCfg);
            }
            else
            {
                OnClose(uiCfg);
            }
        }
    }

    void OnClose(UICfg uiCfg)
    {
    	if (uiCfg == null || uiCfg.instance == null)
    	{
    		Debug.LogError("uiCfg or uiCfg.instance is null");
    		return;
    	}

        var uiController = uiCfg.instance.GetComponent<UIBase> ();
        if (uiController == null)
            return;
        
        uiController.OnClose ();

        RemoveUIFromCanvas (uiCfg.name);

        RemoveFromUIStack(uiCfg.name);

        AdjustDisplayOrder ();

        uiCfg.state = UICfg.UIState.Closed;

        Resources.UnloadUnusedAssets ();

        //System.GC.Collect ();

        UICfg newTopUICfg = null;
        if (_uiStack.Count > 0)
        {
            newTopUICfg = _uiStack [_uiStack.Count - 1];

            if (newTopUICfg == null || newTopUICfg.instance == null)
            {
                Debug.LogError ("Don't instance object.");
                return;
            }

            if (!CheckUIInterface (newTopUICfg.instance))
            {
                Debug.LogError ("Missing component of UIBase.");
                return;
            }

            var newTopUIController = newTopUICfg.instance.GetComponent<UIBase> ();
            newTopUIController.OnBecomeTopUI (newTopUICfg.type);
        }

        
    }

    void RemoveFromUIStack(string name)
    {
        for (int i = _uiStack.Count - 1; i >= 0; --i)
        {
            if (_uiStack [i].name == name)
            {
                _uiStack.RemoveAt (i);
            }
        }
    }

}
