//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//using System.Collections.Generic;
//using System.IO;
//using System.Text.RegularExpressions;
//
//[CustomEditor(typeof(UITemplate))]
//public class UITemplateInspector : Editor
//{
//
//	//模板存放的路径
//    private static string TEMPLATE_PREFAB_PATH =  "Assets/Resources/UITemplate";
//    
//
//    //Prefab存放的路径
//    private static List<string> UIPrefabs = new List<string>()
//    {
//       "Assets/Resources/UIPrefab"
//    };
//
//    private static Dictionary<string, GameObject> _cacheUITemplate;
//
//    const string PrevNodeName = "__PrevNode";
//
//    static UITemplateInspector()
//    {
//        //PrefabUtility.prefabInstanceUpdated = (go) =>
//        //{
//        //    if (go == null)
//        //    {
//        //        return;
//        //    }
//
//        //    if (go.GetComponent<UITemplate>() == null)
//        //    {
//        //        return;
//        //    }
//
//        //    if (EditorUtility.DisplayDialog("提示", "UITemplate已经修改，时候引用到所有预制件？", "ok", "cancel"))
//        //    {
//        //        Object prefabObj = PrefabUtility.GetPrefabParent(go);
//        //        if (prefabObj != null)
//        //        {
//        //            ApplyPrefab(go, prefabObj, true);
//        //        }
//        //    }
//        //};
//
//        CacheUITemplateList();
//    }
//
//	[MenuItem("GameObject/UITemplate/Create To Prefab", false, 11)]
//    static void CreatToPrefab(MenuCommand menuCommand)
//    {
//        if (menuCommand.context != null)
//        {
//			CreatDirectory();
//            GameObject selectGameObject = menuCommand.context as GameObject;
//
//            if (!IsChildOfUIRoot(selectGameObject))
//            {
//                EditorUtility.DisplayDialog("错误！", "不在Root下的对象不能制作成UITempalte.", "OK");
//                return;
//            }  
//
//            if (IsTemplatePrefabInHierarchy(selectGameObject))
//            {
//                CreatPrefab(selectGameObject);
//            }
//            else
//            {
//                CreatPrefab(selectGameObject);
//                GameObject.DestroyImmediate(selectGameObject);
//            }
//        }
//        else
//        {
//            EditorUtility.DisplayDialog("错误！", "请选择一个GameObject", "OK");
//        }
//    }
//
//	
//    
//    private UITemplate uiTemplate;
//
//
//    void OnEnable()
//    {   
//        uiTemplate = (UITemplate)target;
//        
//        if (!EditorApplication.isPlaying && IsTemplatePrefabInInProjectView(uiTemplate.gameObject))
//        {
//            ShowHierarchy();
//        }
//		CreatDirectory();
//    }
//
//    public override void OnInspectorGUI()
//    {
//
// 	    base.OnInspectorGUI();
//
//        EditorGUILayout.LabelField("GUID:" + uiTemplate.GUID);
//	
//        GUILayout.BeginHorizontal();
//
//		if (GUILayout.Button("Select"))
//        {
//            var prefab = GetUITemplate(uiTemplate.GUID);
//            if (prefab != null)
//            {
//                EditorGUIUtility.PingObject(prefab);
//            }
//            return;
//        }
//
//        if (GUILayout.Button("SearchUITemplate"))
//        {
//            uiTemplate.searPrefabs = TrySearchUITemplate(new List<string> { uiTemplate.GUID });
//            return;
//        }
//
//        bool isPrefabInProjectView = IsTemplatePrefabInInProjectView(uiTemplate.gameObject);
//        bool isUITemplateRoot = IsRootOfUITemplate(uiTemplate.gameObject);
//        if (isUITemplateRoot && isPrefabInProjectView)
//        {
//            if (GUILayout.Button("ApplyUITemplate"))
//            {
//                //if (IsTemplatePrefabInHierarchy(uiTemplate.gameObject))
//                //{
//                //    ReplaceUITemplate(uiTemplate.gameObject, GetUITemplate(uiTemplate.GUID));
//                //}
//                ApplyUITemplateReferences(GetUITemplate(uiTemplate.GUID));
//                return;
//            }
//
//            if (GUILayout.Button("DeleteUITemplate"))
//            {
//                //if (IsTemplatePrefabInHierarchy(uiTemplate.gameObject))
//                //{
//                    
//                //}           
//                DeleteUITemplateReferences(new List<string> { uiTemplate.GUID });
//                return;
//            }
//
//
//            //if (GUILayout.Button("SearchPrefab"))
//            //{
//            //    //uiTemplate.searPrefabs = TrySearchPrefab(uiTemplate.GUID);
//            //    return;
//            //}
//
//            //if (GUILayout.Button("ApplyPrefab"))
//            //{
//            //    if (IsTemplatePrefabInHierarchy(uiTemplate.gameObject))
//            //    {
//            //        ApplyPrefab(uiTemplate.gameObject,PrefabUtility.GetPrefabParent(uiTemplate.gameObject), true);
//            //    }
//            //    else
//            //    {  
//            //       ApplyPrefab(uiTemplate.gameObject, uiTemplate.gameObject, false);
//            //    }
//            //    return;
//            //}
//
//            //if (GUILayout.Button("Delete"))
//            //{
//            //    if (IsTemplatePrefabInHierarchy(uiTemplate.gameObject))
//            //    {
//            //        DeletePrefab(GetPrefabPath(uiTemplate.gameObject));
//            //    }
//            //    else
//            //    {
//            //        DeletePrefab(AssetDatabase.GetAssetPath(uiTemplate.gameObject));
//            //    }
//            //    return;
//            //}
//		}
//        GUILayout.EndHorizontal();
//
//
//
//        if (uiTemplate != null && uiTemplate.searPrefabs.Count > 0)
//        {
//            EditorGUILayout.LabelField("Prefab :" + uiTemplate.name);
//
//            foreach (var p in uiTemplate.searPrefabs)
//            {
//                var refrenceUITemplate = GetUITemplate(p.Key);
//
//                if (IsSpecificUITemplate(refrenceUITemplate, uiTemplate.GUID))
//                    continue;
//
//                EditorGUILayout.Space();
//                if (GUILayout.Button(AssetDatabase.GetAssetPath(refrenceUITemplate) + "(" + p.Value + ")"))
//                {
//                    EditorGUIUtility.PingObject(refrenceUITemplate);
//                }
//            }
//        }
//    }
//
//    static private Dictionary<GameObject, int> TrySearchPrefab(string guid)
//    {   
//        Dictionary<GameObject, int> prefabs = new Dictionary<GameObject, int>();
//        
//        foreach(string forder in UIPrefabs)
//        {
//            DirectoryInfo directiory = new DirectoryInfo(Application.dataPath + "/" + forder.Replace("Assets/", ""));
//            FileInfo[] infos = directiory.GetFiles("*.prefab", SearchOption.AllDirectories);
//            for (int i = 0; i < infos.Length; i++)
//            {
//                FileInfo file = infos[i];
//                GameObject prefab = AssetDatabase.LoadAssetAtPath(file.FullName.Substring(file.FullName.IndexOf("Assets")), typeof(GameObject)) as GameObject;
//
//                if (prefab.GetComponentsInChildren<UITemplate>(true).Length > 0)
//                { 
//                    GameObject go = GameObject.Instantiate(prefab) as GameObject;
//                    UITemplate[] templates = go.GetComponentsInChildren<UITemplate>(true);
//                    foreach (UITemplate template in templates)
//                    {
//                        if (template.GUID == guid)
//                        {
//                            int count = 0;
//                            if (prefabs.TryGetValue(prefab, out count))
//                            {
//                                prefabs[prefab] = count + 1;
//                            }
//                            else
//                            {
//                                prefabs.Add(prefab, 1);
//                            }
//                        }
//                    }
//                    GameObject.DestroyImmediate(go);
//                }
//            }
//        }
//
//        return prefabs;
//    }
//
//    static private  Dictionary<string, int> TrySearchUITemplate(List<string> guids)
//    {
//        Dictionary<string, int> prefabs = new Dictionary<string, int>();
//        CacheUITemplateList();
//        foreach(var cacheUITemplate in _cacheUITemplate)
//        {
//            var guid = cacheUITemplate.Key;
//            var prefab = cacheUITemplate.Value;
//            
//            if (prefab.GetComponentsInChildren<UITemplate>(true).Length > 0)
//            {
//                if (IsSpecificUITemplate(prefab, guids))
//                    continue;
//
//                GameObject go = GameObject.Instantiate(prefab) as GameObject;
//                UITemplate[] templates = go.GetComponentsInChildren<UITemplate>(true);
//                foreach (UITemplate template in templates)
//                {
//                    if (guids.Contains(template.GUID))
//                    {
//                        int count = 0;
//                        if (prefabs.TryGetValue(guid, out count))
//                        {
//                            prefabs[guid] = count + 1;
//                        }
//                        else
//                        {
//                            prefabs.Add(guid, 1);
//                        }
//                    }
//                }
//                GameObject.DestroyImmediate(go);
//            }
//        }
//      
//        return prefabs;
//    }
//
//    static private  void ApplyUITemplateReferences(GameObject uiTemplateprefab)
//    {
//        if (uiTemplateprefab == null)
//        {
//            Debug.LogError("uiTemplateprefab is null.");
//            return;
//        }
//
//        if(!IsUITemplate(uiTemplateprefab))
//        {
//            EditorUtility.DisplayDialog("注意！", "不是UI模板!!!!", "确定");
//            return;
//        }
//
//        if (EditorUtility.DisplayDialog("注意！", "是否更新所有UI模板？", "确定", "取消"))
//        {
//            Debug.Log("ApplyUITemplate : " + uiTemplateprefab.name);    
//
//            var uiTemplateprefabGo = PrefabUtility.InstantiatePrefab(uiTemplateprefab) as GameObject;
//            List<string> updatedUITemplateGUIDs = new List<string>();
//            UpdateUITemplate(uiTemplateprefabGo.transform, ref updatedUITemplateGUIDs);
//
//            Dictionary<string, int> references = TrySearchUITemplate(updatedUITemplateGUIDs);
//            foreach (var reference in references)
//            {
//                var uiTemplatePrefab = GetUITemplate(reference.Key);
//                if (uiTemplateprefab == null)
//                {
//                    Debug.LogError("没有模板：" + reference.Key);
//                    continue;
//                }
//                GameObject go = PrefabUtility.InstantiatePrefab(uiTemplatePrefab) as GameObject;
//                UpdateUITemplateForSpecificPrefab(go.transform, updatedUITemplateGUIDs);
//                GameObject.DestroyImmediate(go);
//            } 
//            GameObject.DestroyImmediate(uiTemplateprefabGo);
//        }
//    }
//
//
//    static private void DeleteUITemplateReferences(List<string> guids)
//    {
//        if (guids == null || guids.Count <= 0)
//        {
//            Debug.LogError("guids is empty.");
//            return;
//        }      
//
//        if (EditorUtility.DisplayDialog("注意", "是否删除UI模板的引用？", "确定", "取消"))
//        {
//            Dictionary<string, int> references = TrySearchUITemplate(guids);
//            foreach (var reference in references)
//            {
//                var uiTemplateReferencePrefab = GetUITemplate(reference.Key);
//                if (uiTemplateReferencePrefab == null)
//                {
//                    Debug.LogError("没有模板：" + reference.Key);
//                    continue;
//                }
//                GameObject go = PrefabUtility.InstantiatePrefab(uiTemplateReferencePrefab) as GameObject;
//                UpdateUITemplateForSpecificPrefab(go.transform, guids, true);
//                GameObject.DestroyImmediate(go);
//            } 
//        }
//
//        foreach(var guid in guids)
//        {
//            DeleteUItemplate(guid);
//        }
//    }
//    
//
//    static private  void ApplyPrefab(GameObject prefab, Object targetPrefab, bool replace)
//    {
//        if (EditorUtility.DisplayDialog("注意！", "是否进行递归查找批量替换模板？", "ok", "cancel"))
//        {
//            Debug.Log("ApplyPrefab : " + prefab.name );
//            GameObject replacePrefab;
//            int count = 0;
//            if (replace)
//            {
//                PrefabUtility.ReplacePrefab(prefab, targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
//                Refresh();
//                replacePrefab = targetPrefab as GameObject;
//                count = prefab.GetComponentsInChildren<UITemplate>().Length;
//                
//            }
//            else
//            {
//                replacePrefab = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(targetPrefab), typeof(GameObject)) as GameObject;
//                GameObject checkPrefab = PrefabUtility.InstantiatePrefab(replacePrefab) as GameObject;
//                count = checkPrefab.GetComponentsInChildren<UITemplate>().Length;
//                DestroyImmediate(checkPrefab);
//            }
//
//
//         
//            if (count != 1)
//            {
//                EditorUtility.DisplayDialog("注意！", "无法批量替换，因为模板不支持嵌套。", "ok");
//                return;
//            }
//
//            UITemplate template =  replacePrefab.GetComponent<UITemplate>();
//
//            if(template != null)
//            {
//                Dictionary<GameObject, int> references = TrySearchPrefab(template.GUID);
//
//                foreach(var reference in references)
//                {   
//                    GameObject go = PrefabUtility.InstantiatePrefab(reference.Key) as GameObject;
//                    UITemplate[] instanceTemplates = go.GetComponentsInChildren<UITemplate>();
//                    for (int j = 0; j < instanceTemplates.Length; j++)
//                    {
//                        UITemplate instance = instanceTemplates[j];
//                        if (instance.GUID == template.GUID)
//                        {
//                            GameObject newInstance = GameObject.Instantiate(replacePrefab) as GameObject;
//                            newInstance.name = replacePrefab.name;
//                            newInstance.transform.SetParent(instance.transform.parent);
//                            newInstance.transform.localPosition = instance.transform.localPosition;
//                            DestroyImmediate(instance.gameObject);
//                        }
//                    }
//
//                    PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go), ReplacePrefabOptions.ConnectToPrefab);
//                    DestroyImmediate(go);
//                }
//            }
//            
//            ClearHierarchy();
//            Refresh();
//        }
//    }
//
//    static private void UpdateUITemplate(Transform go, ref List<string> updateTemplateGUIDs)
//    {
//        if (go == null )
//            return;
//
//        if (IsUITemplate(go.gameObject))
//        {
//            var uiTemplateGUID = go.gameObject.GetComponent<UITemplate>().GUID;
//            updateTemplateGUIDs.Add(uiTemplateGUID);
//            var prefabParent = GetUITemplate(uiTemplateGUID);
//            PrefabUtility.ReplacePrefab(go.gameObject, prefabParent, ReplacePrefabOptions.ConnectToPrefab);
//            Refresh();
//        }    
//
//        for (int i = 0; i < go.childCount; ++i)
//        {
//            UpdateUITemplate(go.GetChild(i), ref updateTemplateGUIDs);
//        }
//    }
//
//    static private bool UpdateUITemplateForSpecificPrefab(Transform go, List<string> guids, bool isDelete = false)
//    {
//        if (go == null)
//            return false;
//
//        var uiTemplate = go.GetComponent<UITemplate>();
//        if (uiTemplate != null && guids.Contains(uiTemplate.GUID))
//        {
//            if (!isDelete)
//            {
//                var replacedUITemplatePrefab = GetUITemplate(uiTemplate.GUID);
//
//                GameObject newInstance = GameObject.Instantiate(replacedUITemplatePrefab) as GameObject;
//                newInstance.name = go.name;
//                newInstance.transform.SetParent(go.parent);
//                newInstance.transform.localPosition = go.localPosition;
//                newInstance.SetActive(go.gameObject.activeSelf);
//                var siblingIndex = go.transform.GetSiblingIndex();
//
//                GameObject.DestroyImmediate(go.gameObject);
//
//                newInstance.transform.SetSiblingIndex(siblingIndex);
//            }
//            else 
//            {
//                GameObject.DestroyImmediate(go.gameObject);
//            }
//            return true;
//        }
//        else if (go.childCount <= 0)
//        {
//            return false;
//        }             
//
//       
//        List<bool> canUpdates = null;
//        if (IsUITemplate(go.gameObject))
//            canUpdates = new List<bool>();
//
//        for(int i = 0; i < go.childCount; ++i )
//        {
//            bool canUpdate = UpdateUITemplateForSpecificPrefab(go.GetChild(i), guids);
//            if (canUpdates != null)
//            {
//                canUpdates.Add(canUpdate);
//            }
//        }
//
//        if (canUpdates != null && canUpdates.Count > 0 )
//        {
//            foreach(var haveUpdate in canUpdates)
//            {
//                if(haveUpdate)
//                {
//                    var prefabParent = GetUITemplate(go.gameObject.GetComponent<UITemplate>().GUID);
//                    PrefabUtility.ReplacePrefab(go.gameObject, prefabParent, ReplacePrefabOptions.ConnectToPrefab);
//                    Refresh();
//                    return true;
//                }
//            }
//        }
//        return false;
//    }
//
//    static private void ReplaceUITemplate(GameObject prefab, Object targetPrefab)
//    {
//        if (prefab == null)
//        {
//            Debug.LogError("prefab is null.");
//            return;
//        }
//        if (targetPrefab == null)
//        {
//            Debug.LogError("targetPrefab is null.");
//            return;
//        }
//
//        //var newPrefab = prefab;
//        //if (PrefabUtility.GetPrefabType(prefab) != PrefabType.PrefabInstance)
//        //{
//        //    newPrefab = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
//        //}
//        PrefabUtility.ReplacePrefab(prefab, targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
//        //DestroyImmediate(newPrefab);
//        Refresh();
//    }
//
//    static public void DeletePrefab(string path)
//    {
//        if (EditorUtility.DisplayDialog("注意！", "是否进行递归查找批量删除模板？", "ok", "cancel"))
//        {
//            Debug.Log("DeletePrefab : " + path);
//            GameObject deletePrefab =  AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
//            UITemplate template = deletePrefab.GetComponent<UITemplate>();
//            if (template != null)
//            {
//                Dictionary<GameObject, int> references = TrySearchPrefab(template.GUID);
//
//                foreach (var reference in references)
//                {
//                    GameObject go = PrefabUtility.InstantiatePrefab(reference.Key) as GameObject;
//                    UITemplate[] instanceTemplates = go.GetComponentsInChildren<UITemplate>();
//                    for (int i = 0; i < instanceTemplates.Length; i++)
//                    {
//                        UITemplate instance = instanceTemplates[i];
//                        if (instance.GUID == template.GUID)
//                        {
//                            DestroyImmediate(instance.gameObject);
//                        }
//                    }
//                    PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go), ReplacePrefabOptions.ConnectToPrefab);
//                    DestroyImmediate(go);
//                }
//            }
//            AssetDatabase.DeleteAsset(path);
//            ClearHierarchy();
//            Refresh();
//        }
//    }
//
//  
//
//    static private void CreatPrefab(GameObject prefab)
//    {
//
//        string creatPath = TEMPLATE_PREFAB_PATH + "/" + prefab.name + ".prefab";
//        Debug.Log("CreatPrefab : " + creatPath);
//
//
//        if (AssetDatabase.LoadAssetAtPath(creatPath, typeof(GameObject)) == null)
//        {
//            
//            //UITemplate[] temps =  prefab.GetComponentsInChildren<UITemplate>();
//
//            //for(int i =0; i< temps.Length; i++)
//            //{
//            //    DestroyImmediate(temps[i]);
//            //}
//
//            var uiTemplate = prefab.GetComponent<UITemplate>();
//            if (uiTemplate == null)
//            {
//                uiTemplate = prefab.AddComponent<UITemplate>();
//                uiTemplate.InitGUID();
//            }
//            
//            PrefabUtility.CreatePrefab(TEMPLATE_PREFAB_PATH + "/" + prefab.name + ".prefab", prefab);
//            Refresh();
//
//        }
//        else
//        {
//            EditorUtility.DisplayDialog("错误！", "Prefab名字重复，请重命名！", "OK");
//        }
//        
//    }
//
//
//
//    static private void Refresh()
//    {
//        AssetDatabase.SaveAssets();
//        AssetDatabase.Refresh();
//        EditorApplication.SaveScene();
//    }
//
//    static private GameObject GetUIRoot()
//    {
//        var uiRoot = GameObject.FindObjectOfType<UIRoot>();
//        if (uiRoot != null)
//        {
//            return uiRoot.gameObject;
//        }
//
//        var uiCanvas = GameObject.FindObjectOfType<Canvas>(); ;
//        if (uiCanvas != null)
//        {
//            return uiCanvas.gameObject;
//        }
//        return null;
//    }
//
//    static private bool IsChildOfUIRoot(GameObject go)
//    {
//        var uiRoot = go.GetComponentsInParent<UIRoot>(true);
//        if (uiRoot != null && uiRoot.Length > 0)
//        {
//            return true;
//        }
//
//        var uiCanvas = go.GetComponentsInParent<Canvas>(true);
//        if (uiCanvas != null && uiCanvas.Length > 0)
//        {
//            return true;
//        }
//        return false;
//    }
//
//    static  private void ClearHierarchy()
//    {
//        var uiRoot = GetUIRoot();
//
//        if (uiRoot != null)
//        {
//            var prevNode = uiRoot.transform.FindChild(PrevNodeName);
//            if (prevNode == null)
//            {
//                return;
//            }
//
//            for (int i = 0; i < prevNode.childCount; i++)
//            {
//                Transform t = prevNode.GetChild(i);
//                GameObject.DestroyImmediate(t.gameObject);
//			}
//        }
//    }
//
//    private void ShowHierarchy()
//    {
//       
//        if (!IsTemplatePrefabInHierarchy(uiTemplate.gameObject))
//        {
//            var uiRoot = GetUIRoot();
//            if (uiRoot != null)
//            {
//                var prevNode = uiRoot.transform.FindChild(PrevNodeName );
//                if (prevNode == null)
//                {
//                    GameObject newPrevNode = new GameObject();
//                    newPrevNode.name = PrevNodeName;
//                    newPrevNode.transform.parent = uiRoot.transform;
//                    newPrevNode.transform.SetAsFirstSibling();
//                    newPrevNode.transform.localPosition = new Vector3(0, 0, 0);
//                    newPrevNode.transform.localScale = new Vector3(1f, 1f, 1f);
//                    prevNode = newPrevNode.transform;
//                }
//                
//
//                if (prevNode != null)
//                {	
//                    //if (prevNode.childCount > 0)
//                    //{
//                    //    var prevUITemplate = prevNode.GetChild(0).GetComponent<UITemplate>();
//                    //    if (prevUITemplate != null && prevUITemplate.GUID == uiTemplate.GUID)
//                    //    {
//                    //        return;
//                    //    }                        
//                    //}
//
//                    ClearHierarchy();
//                    var prefabRoot = PrefabUtility.FindPrefabRoot(uiTemplate.gameObject) as GameObject;
//                    GameObject go = PrefabUtility.InstantiatePrefab(prefabRoot) as GameObject;
//                    go.name = prefabRoot.name;
//
//                    GameObjectUtility.SetParentAndAlign(go, prevNode.gameObject);
//                    EditorGUIUtility.PingObject(go);
//				}
//             
//            }
//        }
//
//    }
//
//    static private bool IsTemplatePrefabInHierarchy(GameObject go)
//    {
//        return (PrefabUtility.GetPrefabParent(go) != null);
//    }
//
//    static private bool IsTemplatePrefabInInProjectView(GameObject go)
//    {
//        string path = AssetDatabase.GetAssetPath(go);
//        if (!string.IsNullOrEmpty(path))
//            return (path.Contains(TEMPLATE_PREFAB_PATH));
//        else
//            return false;
//    }
//
//	static private DirectoryInfo CreatDirectory()
//	{
//		DirectoryInfo directiory = new DirectoryInfo(Application.dataPath + "/" + TEMPLATE_PREFAB_PATH.Replace("Assets/",""));
//		if(!directiory.Exists)
//		{
//			directiory.Create();
//			Refresh();
//		}
//		return directiory;
//   	}
//
//    static private string GetPrefabPath(GameObject prefab)
//    {
//        Object prefabObj = PrefabUtility.GetPrefabParent(prefab);
//        if (prefabObj != null)
//        {
//            return AssetDatabase.GetAssetPath(prefabObj);
//        }
//        return null;
//    }
//
//    static private bool IsUITemplate(GameObject prefab)
//    {
//        if (prefab == null)
//            return false;
//
//        var uiTemplate = prefab.GetComponent<UITemplate>();
//        return uiTemplate != null;
//    }
//
//    static private bool IsSpecificUITemplate(GameObject prefab, string guid)
//    {
//        if (prefab == null || string.IsNullOrEmpty(guid))
//            return false;
//
//        var uiTemplate = prefab.GetComponent<UITemplate>();
//        if (uiTemplate == null)
//            return false;
//
//        return uiTemplate.GUID == guid;
//    }
//
//	static private bool IsSpecificUITemplate(GameObject prefab, List<string> guids)
//	{
//		bool ret = false;
//		foreach (var guid in guids) 
//		{
//			ret = ret || IsSpecificUITemplate(prefab, guid);
//		}
//		return ret;
//	}
//
//    static private bool IsRootOfUITemplate(GameObject prefab)
//    {
//        if (prefab == null)
//        {
//            return false;
//        }
//       
//        if(IsTemplatePrefabInHierarchy(prefab))
//        {
//            if (IsUITemplate(prefab))
//            {
//                return true;
//            }
//            return false;
//        }
//        else
//        {
//            var prefabRoot = PrefabUtility.FindPrefabRoot(prefab);
//            if (prefabRoot == null)
//            {
//                return false;
//            }
//            var uiTemplateOfPrefabRoot = prefabRoot.GetComponent<UITemplate>();
//            var uiTemplateOfPrefab = prefab.GetComponent<UITemplate>();
//            if (uiTemplateOfPrefabRoot == null || uiTemplateOfPrefab == null)
//            {
//                return false;
//            }
//            return uiTemplateOfPrefabRoot.GUID == uiTemplateOfPrefab.GUID;
//        }
//    }
//
//    static public void CacheUITemplateList()
//    {
//        _cacheUITemplate = new Dictionary<string, GameObject>();
//   
//        DirectoryInfo directiory = new DirectoryInfo(Application.dataPath + "/" + TEMPLATE_PREFAB_PATH.Replace("Assets/", ""));
//        FileInfo[] infos = directiory.GetFiles("*.prefab", SearchOption.AllDirectories);
//        for (int i = 0; i < infos.Length; i++)
//        {
//            FileInfo file = infos[i];
//            GameObject prefab = AssetDatabase.LoadAssetAtPath(file.FullName.Substring(file.FullName.IndexOf("Assets")), typeof(GameObject)) as GameObject;
//
//            var uiTemplate = prefab.GetComponent<UITemplate>();
//            if (uiTemplate != null && !_cacheUITemplate.ContainsKey(uiTemplate.GUID))
//            {
//                _cacheUITemplate.Add(uiTemplate.GUID, prefab);
//            }
//        }
//    }
//
//    static private GameObject GetUITemplate(string guid)
//    {
//        CacheUITemplateList();
//        foreach (var uiTemplateGo in _cacheUITemplate)
//        {
//            var uiTemplate = uiTemplateGo.Value.GetComponent<UITemplate>();
//            if (uiTemplate != null && uiTemplate.GUID == guid)
//            {
//                return uiTemplateGo.Value;
//            }
//        }
//        return null;
//    }
//
//    static private void DeleteUItemplate(string guid)
//    {
//        var uiTemplatePrefab = GetUITemplate(guid);
//        if (uiTemplatePrefab != null)
//        {
//            AssetDatabase.MoveAssetToTrash(GetPrefabPath(uiTemplatePrefab));
//        }
//    }
//    
//}
