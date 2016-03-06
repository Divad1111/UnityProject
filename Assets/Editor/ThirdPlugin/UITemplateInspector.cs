﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

[CustomEditor(typeof(UITemplate))]
public class UITemplateInspector : Editor
{
    static UITemplateInspector()
    {
        //PrefabUtility.prefabInstanceUpdated = (go) =>
        //{
        //    if (go == null)
        //    {
        //        return;
        //    }

        //    if (go.GetComponent<UITemplate>() == null)
        //    {
        //        return;
        //    }

        //    if (EditorUtility.DisplayDialog("提示", "UITemplate已经修改，时候引用到所有预制件？", "ok", "cancel"))
        //    {
        //        Object prefabObj = PrefabUtility.GetPrefabParent(go);
        //        if (prefabObj != null)
        //        {
        //            ApplyPrefab(go, prefabObj, true);
        //        }
        //    }
        //};
    }

	//模板存放的路径
    private static string TEMPLATE_PREFAB_PATH =  "Assets/UI/Template";
    

    //Prefab存放的路径
    private static List<string> UIPrefabs = new List<string>()
    {
       "Assets/UI/Prefab"
    };


    const string PrevNodeName = "__PrevNode";

	[MenuItem("GameObject/UITemplate/Create To Prefab", false, 11)]
    static void CreatToPrefab(MenuCommand menuCommand)
    {
        if (menuCommand.context != null)
        {
			CreatDirectory();
            GameObject selectGameObject = menuCommand.context as GameObject;

            if (!IsChildOfUIRoot(selectGameObject))
            {
                EditorUtility.DisplayDialog("错误！", "不在Root下的对象不能制作成UITempalte.", "OK");
                return;
            }  

            if (IsTemplatePrefabInHierarchy(selectGameObject))
            {
                CreatPrefab(selectGameObject);
            }
            else
            {
                CreatPrefab(selectGameObject);
                GameObject.DestroyImmediate(selectGameObject);
            }
        }
        else
        {
            EditorUtility.DisplayDialog("错误！", "请选择一个GameObject", "OK");
        }
    }

	
    
    private UITemplate uiTemplate;


    void OnEnable()
    {
        uiTemplate = (UITemplate)target;

        if (IsTemplatePrefabInInProjectView(uiTemplate.gameObject))
        {
            ShowHierarchy();
        }
		CreatDirectory();
    }

    public override void OnInspectorGUI()
    {

 	    base.OnInspectorGUI();
	    bool isPrefabInProjectView = IsTemplatePrefabInInProjectView(uiTemplate.gameObject);
        EditorGUILayout.LabelField("GUID:" + uiTemplate.GUID);
	
        GUILayout.BeginHorizontal();

		if (GUILayout.Button("Select"))
        {
			DirectoryInfo directiory = CreatDirectory();

            FileInfo[] infos = directiory.GetFiles("*.prefab", SearchOption.AllDirectories);
            for (int i = 0; i < infos.Length; i++)
            {
                FileInfo file = infos[i];
                GameObject prefab = AssetDatabase.LoadAssetAtPath(file.FullName.Substring(file.FullName.IndexOf("Assets")), typeof(GameObject)) as GameObject;
                if(prefab.GetComponent<UITemplate>().GUID == uiTemplate.GUID)
                {
                    EditorGUIUtility.PingObject(prefab);
                    return;
                }
            }
        }
        
		if (isPrefabInProjectView)
        {

	        if (GUILayout.Button("Search"))
	        {
	            TrySearchPrefab(uiTemplate.GUID, out uiTemplate.searPrefabs);
				return;
	        }

	        if (GUILayout.Button("Apply"))
	        {
	            if (IsTemplatePrefabInHierarchy(uiTemplate.gameObject))
	            {

	                ApplyPrefab(uiTemplate.gameObject,PrefabUtility.GetPrefabParent(uiTemplate.gameObject), true);
	            }
	            else
	            {
	                
	               ApplyPrefab(uiTemplate.gameObject, uiTemplate.gameObject, false);
	            }
	            return;
	        }

	        if (GUILayout.Button("Delete"))
	        {
	            if (IsTemplatePrefabInHierarchy(uiTemplate.gameObject))
	            {
	                DeletePrefab(GetPrefabPath(uiTemplate.gameObject));
	            }
	            else
	            {
	                DeletePrefab(AssetDatabase.GetAssetPath(uiTemplate.gameObject));
	            }
				return;
	        }
		}
        GUILayout.EndHorizontal();


		if (isPrefabInProjectView)
		{
	        if(uiTemplate != null && uiTemplate.searPrefabs.Count > 0)
	        {
	            EditorGUILayout.LabelField("Prefab :" + uiTemplate.name);

	            foreach (GameObject p in uiTemplate.searPrefabs)
	            {
	                EditorGUILayout.Space();
	                if (GUILayout.Button(AssetDatabase.GetAssetPath(p))) {
	                    EditorGUIUtility.PingObject(p);
	                }
	            }
	        }
        }
    
    }

    static private bool TrySearchPrefab(string guid,out List<GameObject> searchList )
    {
        List<GameObject> prefabs = new List<GameObject>();
        bool trySearch = false;
        foreach(string forder in UIPrefabs)
        {
            DirectoryInfo directiory = new DirectoryInfo(Application.dataPath + "/" + forder.Replace("Assets/", ""));
            FileInfo[] infos = directiory.GetFiles("*.prefab", SearchOption.AllDirectories);
            for (int i = 0; i < infos.Length; i++)
            {
                FileInfo file = infos[i];
                GameObject prefab = AssetDatabase.LoadAssetAtPath(file.FullName.Substring(file.FullName.IndexOf("Assets")), typeof(GameObject)) as GameObject;

                if (prefab.GetComponentsInChildren<UITemplate>(true).Length > 0)
                {
                    GameObject go = GameObject.Instantiate(prefab) as GameObject;
                    UITemplate[] templates = go.GetComponentsInChildren<UITemplate>();
                    foreach (UITemplate template in templates)
                    {
                        if (template.GetComponentsInChildren<UITemplate>().Length > 1)
                        {
                            Debug.LogError(file.FullName + " 模板 " + template.name + " 进行了嵌套的错误操作~请删除重试");
                            if (!trySearch)
                                trySearch = true;
                        }
                        else
                        {
                            if (template.GUID == guid && !prefabs.Contains(prefab))
                            {
                                prefabs.Add(prefab);
                            }
                        }


                    }
                    GameObject.DestroyImmediate(go);
                }
            }
        }

        searchList = prefabs;
        return !trySearch;
    }

    static private  void ApplyPrefab(GameObject prefab, Object targetPrefab, bool replace)
    {
        if (EditorUtility.DisplayDialog("注意！", "是否进行递归查找批量替换模板？", "ok", "cancel"))
        {
            Debug.Log("ApplyPrefab : " + prefab.name );
            GameObject replacePrefab;
            int count = 0;
            if (replace)
            {
                PrefabUtility.ReplacePrefab(prefab, targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
                Refresh();
                replacePrefab = targetPrefab as GameObject;
                count = prefab.GetComponentsInChildren<UITemplate>().Length;
                
            }
            else
            {
                replacePrefab = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(targetPrefab), typeof(GameObject)) as GameObject;
                GameObject checkPrefab = PrefabUtility.InstantiatePrefab(replacePrefab) as GameObject;
                count = checkPrefab.GetComponentsInChildren<UITemplate>().Length;
                DestroyImmediate(checkPrefab);
            }


         
            if (count != 1)
            {
                EditorUtility.DisplayDialog("注意！", "无法批量替换，因为模板不支持嵌套。", "ok");
                return;
            }

            UITemplate template =  replacePrefab.GetComponent<UITemplate>();

            if(template != null)
            {
               List<GameObject> references;
               if (TrySearchPrefab(template.GUID, out references)){
                    for (int i = 0; i < references.Count; i++)
                    {
                        GameObject reference = references[i];
                        GameObject go = PrefabUtility.InstantiatePrefab(reference) as GameObject;
                        UITemplate[] instanceTemplates = go.GetComponentsInChildren<UITemplate>();
                        for (int j = 0; j < instanceTemplates.Length; j++)
                        {
                            UITemplate instance = instanceTemplates[j];
                            if (instance.GUID == template.GUID)
                            {
                                GameObject newInstance = GameObject.Instantiate(replacePrefab) as GameObject;
                                newInstance.name = replacePrefab.name;
                                newInstance.transform.SetParent(instance.transform.parent);
                                newInstance.transform.localPosition = instance.transform.localPosition;
                                DestroyImmediate(instance.gameObject);
                            }
                        }

                        PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go), ReplacePrefabOptions.ConnectToPrefab);
                        DestroyImmediate(go);
                    }
                }
            }
            ClearHierarchy();
            Refresh();
        }
    }



    static private void DeletePrefab(string path)
    {
        if (EditorUtility.DisplayDialog("注意！", "是否进行递归查找批量删除模板？", "ok", "cancel"))
        {
            Debug.Log("DeletePrefab : " + path);
            GameObject deletePrefab =  AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            UITemplate template = deletePrefab.GetComponent<UITemplate>();
            if (template != null)
            {
                List<GameObject> references;
                if (TrySearchPrefab(template.GUID, out references))
                {
                    
                    foreach (GameObject reference in references)
                    {
                        GameObject go = PrefabUtility.InstantiatePrefab(reference) as GameObject;
                        UITemplate[] instanceTemplates = go.GetComponentsInChildren<UITemplate>();
                        for (int i = 0; i < instanceTemplates.Length; i++)
                        {
                            UITemplate instance = instanceTemplates[i];
                            if (instance.GUID == template.GUID)
                            {
                                DestroyImmediate(instance.gameObject);
                            }
                        }
                        PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go), ReplacePrefabOptions.ConnectToPrefab);
                        DestroyImmediate(go);
                    }
                }
            }
            AssetDatabase.DeleteAsset(path);
            ClearHierarchy();
            Refresh();
        }
    }


  

    static private void CreatPrefab(GameObject prefab)
    {

        string creatPath = TEMPLATE_PREFAB_PATH + "/" + prefab.name + ".prefab";
        Debug.Log("CreatPrefab : " + creatPath);


        if (AssetDatabase.LoadAssetAtPath(creatPath, typeof(GameObject)) == null)
        {
            
            UITemplate[] temps =  prefab.GetComponentsInChildren<UITemplate>();

            for(int i =0; i< temps.Length; i++)
            {
                DestroyImmediate(temps[i]);
            }

            prefab.AddComponent<UITemplate>().InitGUID();
            PrefabUtility.CreatePrefab(TEMPLATE_PREFAB_PATH + "/" + prefab.name + ".prefab", prefab);
            Refresh();

        }
        else
        {
            EditorUtility.DisplayDialog("错误！", "Prefab名字重复，请重命名！", "OK");
        }
        
    }



    static private void Refresh()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorApplication.SaveScene();
    }

    static private GameObject GetUIRoot()
    {
        var uiRoot = GameObject.FindObjectOfType<UIRoot>();
        if (uiRoot != null)
        {
            return uiRoot.gameObject;
        }

        var uiCanvas = GameObject.FindObjectOfType<Canvas>(); ;
        if (uiCanvas != null)
        {
            return uiCanvas.gameObject;
        }
        return null;
    }

    static private bool IsChildOfUIRoot(GameObject go)
    {
        var uiRoot = go.GetComponentsInParent<UIRoot>(true);
        if (uiRoot != null && uiRoot.Length > 0)
        {
            return true;
        }

        var uiCanvas = go.GetComponentsInParent<Canvas>(true);
        if (uiCanvas != null && uiCanvas.Length > 0)
        {
            return true;
        }
        return false;
    }

    static  private void ClearHierarchy()
    {
        var uiRoot = GetUIRoot();

        if (uiRoot != null)
        {
            var prevNode = uiRoot.transform.FindChild(PrevNodeName);
            for (int i = 0; i < prevNode.childCount; i++)
            {
                Transform t = prevNode.GetChild(i);
				if(t.GetComponent<UITemplate>()!= null){
                    GameObject.DestroyImmediate(t.gameObject);
                }

			}
        }
    }

    private void ShowHierarchy()
    {
       
        if (!IsTemplatePrefabInHierarchy(uiTemplate.gameObject))
        {
            var uiRoot = GetUIRoot();
            if (uiRoot != null)
            {
                var prevNode = uiRoot.transform.FindChild(PrevNodeName );
                if (prevNode == null)
                {
                    GameObject newPrevNode = new GameObject();
                    newPrevNode.name = PrevNodeName;
                    newPrevNode.transform.parent = uiRoot.transform;
                    newPrevNode.transform.SetAsFirstSibling();
                    newPrevNode.transform.localPosition = new Vector3(0, 0, 0);
                    newPrevNode.transform.localScale = new Vector3(1f, 1f, 1f);
                    prevNode = newPrevNode.transform;
                }
                

                if (prevNode != null)
                {	
                    if (prevNode.childCount > 0)
                    {
                        var prevUITemplate = prevNode.GetChild(0).GetComponent<UITemplate>();
                        if (prevUITemplate != null && prevUITemplate.GUID == uiTemplate.GUID)
                        {
                            return;
                        }                        
                    }

                    ClearHierarchy();
                    GameObject go = PrefabUtility.InstantiatePrefab(uiTemplate.gameObject) as GameObject;
                    go.name = uiTemplate.gameObject.name;

                    GameObjectUtility.SetParentAndAlign(go, prevNode.gameObject);
                    EditorGUIUtility.PingObject(go);
				}
             
            }
        }

    }

    static private bool IsTemplatePrefabInHierarchy(GameObject go)
    {
        return (PrefabUtility.GetPrefabParent(go) != null);
    }

    static private bool IsTemplatePrefabInInProjectView(GameObject go)
    {
        string path = AssetDatabase.GetAssetPath(go);
        if (!string.IsNullOrEmpty(path))
            return (path.Contains(TEMPLATE_PREFAB_PATH));
        else
            return false;
    }

	static private DirectoryInfo CreatDirectory()
	{
		DirectoryInfo directiory = new DirectoryInfo(Application.dataPath + "/" + TEMPLATE_PREFAB_PATH.Replace("Assets/",""));
		if(!directiory.Exists)
		{
			directiory.Create();
			Refresh();
		}
		return directiory;
   	}

    static private string GetPrefabPath(GameObject prefab)
    {
        Object prefabObj = PrefabUtility.GetPrefabParent(prefab);
        if (prefabObj != null)
        {
            return AssetDatabase.GetAssetPath(prefabObj);
        }
        return null;
    }

}
