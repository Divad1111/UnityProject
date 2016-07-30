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
    public const int UIDistance = 20; 

    public static GameObject UIRoot { get; private set; }

    public static Camera UICamera { get; private set; }



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
        GameObject uiRoot = new GameObject ("UI Root");
        uiRoot.transform.position = new Vector3 (2000F, 2000F, 2000F);

        var uiLayerIndex = LayerMask.NameToLayer ("UI");
        uiRoot.layer = uiLayerIndex;
            
        //创建并设置相机
        GameObject caremaGo = new GameObject ("Camera");
        var camera = caremaGo.AddComponent<Camera> ();
        camera.orthographic = true;
        camera.cullingMask = 1 << uiLayerIndex;
        camera.clearFlags = CameraClearFlags.Depth;
        camera.nearClipPlane = 0.3F;
        camera.farClipPlane = 1000F;

        camera.transform.SetParent (uiRoot.transform, false);
        caremaGo.layer = uiLayerIndex;

        var phyRaycaster = caremaGo.AddMissingComponent<PhysicsRaycaster> ();
        phyRaycaster.eventMask = 1 << uiLayerIndex;


        //创建事件系统
        GameObject eventSystemGo = new GameObject();
        eventSystemGo.name = "EventSystem";
        eventSystemGo.layer = uiLayerIndex;

        var eventSystem = eventSystemGo.AddMissingComponent<EventSystem> ();
        eventSystem.sendNavigationEvents = true;
        eventSystem.pixelDragThreshold = 5;

        eventSystemGo.AddMissingComponent<StandaloneInputModule> ();

        eventSystemGo.transform.SetParent (uiRoot.transform, false);

        //保存UIRoot和UI相机
        UIRoot = uiRoot;
        UICamera = camera;
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

        if (!CheckLayer (uiGo, "UI"))
        {
            Debug.LogError ("Not in layer of UI.");
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



        var openingUIController = uiGo.GetComponent<IUIController> ();
        openingUIController.OnOpen ();

        _uiStack.Add (uiCfg);

        AdjustDisplayOrder ();

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

    static bool CheckUIRoot()
    {
        return UIRoot != null && UICamera != null;
    }   

    static bool CheckLayer(GameObject go, string targetLayerName)
    {
        if (go == null)
            return false;

        return go.layer == LayerMask.NameToLayer (targetLayerName);
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

    bool CheckUIStack()
    {
        return _uiStack != null && _uiStack.Count > 0;
    }

    GameObject AddUIToUIRoot(UICfg uiCfg)
    {
        if (uiCfg == null)
            return null;

        if (!CheckSameName (UIRoot.transform, uiCfg.name))
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
        if (!CheckUIRoot ())
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
        UICfg uiCfg = null;
        if (_uiStack.Count > 1)
        {
            uiCfg = _uiStack [_uiStack.Count - 2];

            if (uiCfg == null || uiCfg.instance == null)
            {
                Debug.LogError ("Don't instance object.");
                return;
            }

            if (!CheckUIInterface (uiCfg.instance))
            {
                Debug.LogError ("Missing component of IUIController.");
                return;
            }

            var uiController = uiCfg.instance.GetComponent<IUIController> ();
            uiController.OnBecomeSecondaryUI (openingUICfg.type, openingUICfg.name);
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
        if (!CheckUIRoot())
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

            //调节UI到相机的距离
            var canvas = uiGo.GetComponent<Canvas> ();
            if (canvas == null)
                continue;
            
            if (canvas.worldCamera == null || canvas.worldCamera != UICamera)
                canvas.worldCamera = UICamera;

            canvas.planeDistance = UIDistance * (_uiStack.Count - i);

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
        
        for (int i = _uiStack.Count - 1; i >= 0; --i)
        {
            var uiCfg = _uiStack [i];
            if (uiCfg.name != name)
                continue;

            if (uiCfg.instance == null)
                continue;
            
            if (!CheckUIInterface (uiCfg.instance))
            {
                Debug.LogError ("Missing component of IUIController.");
                continue;
            }

            if (!string.IsNullOrEmpty (uiCfg.closeAni))
            {
                var uiController = uiCfg.instance.GetComponent<IUIController> ();
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
        var uiController = uiCfg.instance.GetComponent<IUIController> ();
        if (uiController == null)
            return;
        
        uiController.OnClose ();

        RemoveUIFromUIRoot (uiCfg.name);

        RemoveFromUIStack(uiCfg.name);

        AdjustDisplayOrder ();


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
                Debug.LogError ("Missing component of IUIController.");
                return;
            }

            var newTopUIController = newTopUICfg.instance.GetComponent<IUIController> ();
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
