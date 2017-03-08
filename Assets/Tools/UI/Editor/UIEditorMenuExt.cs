using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public static class UIEditorMenuExt
{
    [MenuItem("GameObject/UI/Tools/CreateUIRoot")]
    static void CreateUIRoot()
    {
        UIMgr.CreateDefaultUIRoot ();
    }

    //[MenuItem("GameObject/UI/Tools/Canvas")]
    //static void CreateDefaultCanvas()
    //{
    //    if (!CheckCreateDefaultCanvas())
    //    {
    //        return;
    //    }

    //    var canvasCamera = Selection.activeGameObject.GetComponentInChildren<Camera>(true);

    //    GameObject canvasGo = new GameObject();
    //    canvasGo.name = "Canvas";

    //    UITools.InitDefaultParamForCanvas(canvasGo, canvasCamera);

    //    canvasGo.transform.SetParent(Selection.activeGameObject.transform, false);
    //}

    //[MenuItem("GameObject/UI/Tools/Canvas", true)]
    //static bool CheckCreateDefaultCanvas()
    //{
    //    if (Selection.gameObjects.Length != 1)
    //        return false;

    //    var selectedName = Selection.activeGameObject.name;
    //    if (selectedName == "UI Root")
    //        return true;

    //    return false;
    //}

    [MenuItem("GameObject/UI/Tools/New UI")]
    static void CreateNewUI()
    {
        if (!CheckCreateNewUI())
        {
            Debug.LogWarning("创建UI必须选择一个Canvas!");
            return;
        }
        

        GameObject newUIGo = new GameObject();
        newUIGo.name = "New UI";
        var rectTrans = newUIGo.AddComponent<RectTransform>();
        rectTrans.SetParent(Selection.activeGameObject.transform);
        rectTrans.localPosition = Vector3.zero;
        rectTrans.localScale = Vector3.one;
        rectTrans.localRotation = Quaternion.Euler(0F, 0F, 0F);
        rectTrans.anchorMin = Vector2.zero;
        rectTrans.anchorMax = Vector2.one;
        rectTrans.pivot = new Vector2(0.5F, 0.5F);
        rectTrans.offsetMin = Vector2.zero;
        rectTrans.offsetMax = Vector2.zero;
    }

    //[MenuItem("GameObject/UI/Tools/New UI", true)]
    static bool CheckCreateNewUI()
    {
        if (Selection.gameObjects.Length != 1)
            return false;

        var selectedObj = Selection.activeGameObject;
        if (selectedObj != null && selectedObj.GetComponent<Canvas>() != null)
            return true;

        return false;
    }


    [MenuItem("GameObject/UI/Tools/ShortCutCreate/Label &#l")]
    static void Label()
    {
        var selectedObject = Selection.activeTransform;
        EditorApplication.ExecuteMenuItem ("GameObject/UI/Text");

        if (selectedObject != null)
        {
            Selection.activeTransform.SetParent(selectedObject);
        }
    }

    [MenuItem("GameObject/UI/Tools/ShortCutCreate/Sprite &#s")]
    static void Sprite()
    {
        var selectedObject = Selection.activeTransform;
        EditorApplication.ExecuteMenuItem ("GameObject/UI/Image");

        if (selectedObject != null)
        {
            Selection.activeTransform.SetParent(selectedObject);
        }
    }

    [MenuItem("GameObject/UI/Tools/ShortCutCreate/Texture &#t")]
    static void Texture()
    {
        var selectedObject = Selection.activeTransform;
        EditorApplication.ExecuteMenuItem ("GameObject/UI/Raw Image");

        if (selectedObject != null)
        {
            Selection.activeTransform.SetParent(selectedObject);
        }
    }

    [MenuItem("GameObject/UI/Tools/ShortCutCreate/Button &#b")]
    static void Button()
    {
        var selectedObject = Selection.activeTransform;
        EditorApplication.ExecuteMenuItem ("GameObject/UI/Button");

        if (selectedObject != null)
        {
            Selection.activeTransform.SetParent(selectedObject);
        }
    }

    [MenuItem("GameObject/UI/Tools/ShortCutCreate/BoxCollider &#c")]
    static void Collider()
    {
        if (Selection.activeGameObject == null)
        {
            EditorApplication.Beep ();
            return;
        }

        var boxcld = Selection.activeGameObject.AddMissingComponent<BoxCollider> ();
        AdjustBoxColliderSize (boxcld);

    }

    public static void AdjustBoxColliderSize(BoxCollider boxcld)
    {
        if (boxcld == null)
            return;

        var go = boxcld.gameObject;
        if (go == null)
            return;
        
        var rectTransform = go.GetComponent<RectTransform> ();
        if (rectTransform == null)
            return;

        boxcld.center = CalcColliderCenter(rectTransform);
        boxcld.size = new Vector3 (rectTransform.rect.width, rectTransform.rect.height, 1F);
        boxcld.isTrigger = true;
    }

    static Vector3 CalcColliderCenter(RectTransform rectTransform)
    {
        return new Vector3 (rectTransform.rect.width * (0.5F - rectTransform.pivot.x), rectTransform.rect.height * (0.5F - rectTransform.pivot.y ), 1F);
    }

    [MenuItem("CONTEXT/BoxCollider/SizeAdapter")]
    static void ResetColliderSize(MenuCommand context)
    {
        var collider = context.context as BoxCollider;
        if (collider != null)
        {
            AdjustBoxColliderSize (collider);
        }
    }

    [MenuItem("GameObject/AssetBundle/Create")]
    static void AssetBundleCreate()
    {
       // BuildPipeline.BuildAssetBundles ("Assets/AssetBundle/");
    }
}
