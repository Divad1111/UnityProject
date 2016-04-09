using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DisableCreateSameUITemplate : AssetModificationProcessor
{
    // Known issues:
    // You can still apply changes to prefabs of locked files (but the prefabs wont be saved)
    // You can add add components to prefabs (but the prefabs wont be saved)
    // IsOpenForEdit might get called a few too many times per object selection, so try and cache the result for performance (i.e called in same frame)

    static string UITemplateFolder = @"Assets/Resources/UITemplate";


    private static string willCreateAssetPath = string.Empty;

    public static void OnWillCreateAsset(string path)
    {
        willCreateAssetPath = path;
    }

    //public static string[] OnWillSaveAssets(string[] paths)
    //{   
    //    List<string> result = new List<string>();
    //    foreach (var path in paths)
    //    {   
    //        bool isInUITemplateFolder = IsInUITemplateFolder(path);
    //        bool isUITemplate = IsUITemplate(path);
    //        if (isInUITemplateFolder && isUITemplate)
    //        {
    //            if(!HaveSameUITemplate(path))
    //            {
    //                result.Add(path);
    //            }
    //        }
    //        else if (isInUITemplateFolder && !isUITemplate)
    //        {
    //            EditorUtility.DisplayDialog("错误", "在UITemplate目录下必须要包含UITemplate组件", "确定");
    //            AssetDatabase.DeleteAsset(path);
    //        }
    //        else if (isUITemplate && !isInUITemplateFolder)
    //        {
    //            EditorUtility.DisplayDialog("提示", "UITemplate预制必须放置在UITemplate目录，必须移动到UITemplate目录", "确定");
    //            string validateMove = AssetDatabase.ValidateMoveAsset(path, UITemplateFolder);
    //            if(string.IsNullOrEmpty(validateMove))
    //            {
    //                AssetDatabase.MoveAsset(path, UITemplateFolder);
    //            }
    //            else
    //            {
    //                AssetDatabase.MoveAssetToTrash(path);
    //            }
    //        }
    //        else 
    //        {
    //            if (IsUnlocked(path))
    //                result.Add(path);
    //            else
    //                Debug.LogError(path + " is read-only.");
    //        }
    //    }
    //    return result.ToArray();
    //}

    //public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
    //{   
    //    AssetMoveResult result = AssetMoveResult.DidNotMove;
    //    if (IsLocked(oldPath))
    //    {
    //        Debug.LogError(string.Format("Could not move {0} to {1} because {0} is locked!", oldPath, newPath));
    //        result = AssetMoveResult.FailedMove;
    //    }
    //    else if (IsLocked(newPath))
    //    {
    //        Debug.LogError(string.Format("Could not move {0} to {1} because {1} is locked!", oldPath, newPath));
    //        result = AssetMoveResult.FailedMove;
    //    }

        
    //    if (IsInUITemplateFolder(oldPath))
    //    {
    //        result = AssetMoveResult.FailedMove;
    //    }
        
    //    if(IsUITemplate(oldPath) && !IsInUITemplateFolder(newPath))
    //    {
    //        result = AssetMoveResult.FailedMove;
    //    }
    //    return result;
    //}

    //public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
    //{
    //    if (IsLocked(assetPath))
    //    {
    //        Debug.LogError(string.Format("Could not delete {0} because it is locked!", assetPath));
    //        return AssetDeleteResult.FailedDelete;
    //    }

    //    if (IsInUITemplateFolder(assetPath) && IsUITemplate(assetPath))
    //    {
    //        UITemplateInspector.DeletePrefab(assetPath);   
    //    }

    //    return AssetDeleteResult.DidNotDelete;
    //}

    static bool IsUnlocked(string path)
    {
        return !IsLocked(path);
    }

    static bool IsLocked(string path)
    {
        if (!File.Exists(path))
            return false;
        FileInfo fi = new FileInfo(path);
        return fi.IsReadOnly;
    }

    static bool IsUITemplate(string path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        return path.Contains(UITemplateFolder);
    }

    static bool IsInUITemplateFolder(string path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        var uiTemplate = AssetDatabase.LoadAssetAtPath(path.Substring(path.IndexOf("Assets")), typeof(UITemplate));
        return uiTemplate != null;
    }

    static bool HaveSameUITemplate(string path)
    {   
        UITemplate destUITemplate = AssetDatabase.LoadAssetAtPath(path.Substring(path.IndexOf("Assets")), typeof(UITemplate)) as UITemplate;
        if (destUITemplate == null)
        {
            Debug.LogError(path + " isn't UITemplate.");
            return false;
        }

        int count = 0;
        DirectoryInfo directiory = new DirectoryInfo(Application.dataPath + "/" + UITemplateFolder.Replace("Assets/", ""));
        FileInfo[] infos = directiory.GetFiles("*.prefab", SearchOption.AllDirectories);
        for (int i = 0; i < infos.Length; i++)
        {
            FileInfo file = infos[i];
            UITemplate uiTemplate = AssetDatabase.LoadAssetAtPath(file.FullName.Substring(file.FullName.IndexOf("Assets")), typeof(UITemplate)) as UITemplate;
            if (uiTemplate.GUID == destUITemplate.GUID)
            {
                ++count;
            }
        }

        return count > 1;
    }
}
