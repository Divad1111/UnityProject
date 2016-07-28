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

    [MenuItem("GameObject/UI/Tools/Canvas")]
    static void CreateDefaultCanvas()
    {        
        if (!CheckCreateDefaultCanvas())
        {
            return;
        }

        var canvasCamera = Selection.activeGameObject.GetComponentInChildren<Camera> (true);
            
        GameObject canvasGo = new GameObject ();
        canvasGo.name = "Canvas";
        canvasGo.layer = LayerMask.NameToLayer ("UI");

        var canvas = canvasGo.AddComponent<Canvas> ();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = canvasCamera;
        canvas.planeDistance = UIMgr.UIDistance;

        var canvasScaler = canvasGo.AddComponent<CanvasScaler> ();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2 (1280, 720);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 0F;

        canvasGo.transform.SetParent (Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/UI/Tools/Canvas", true)]
    static bool CheckCreateDefaultCanvas()
    {
        if (Selection.gameObjects.Length != 1)
            return false;

        var selectedName = Selection.activeGameObject.name;
        if (selectedName == "UI Root")
            return true;

        return false;
    }

  
    [MenuItem("GameObject/UI/Tools/ShortCutCreate/Label &#l")]
    static void Label()
    {
        EditorApplication.ExecuteMenuItem ("GameObject/UI/Text");
    }

    [MenuItem("GameObject/UI/Tools/ShortCutCreate/Sprite &#s")]
    static void Sprite()
    {
        EditorApplication.ExecuteMenuItem ("GameObject/UI/Image");
    }

    [MenuItem("GameObject/UI/Tools/ShortCutCreate/Texture &#t")]
    static void Texture()
    {
        EditorApplication.ExecuteMenuItem ("GameObject/UI/Raw Image");
    }

    [MenuItem("GameObject/UI/Tools/ShortCutCreate/Button &#b")]
    static void Button()
    {
        EditorApplication.ExecuteMenuItem ("GameObject/UI/Button");
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

    static void AdjustBoxColliderSize(BoxCollider boxcld)
    {
        if (boxcld == null)
            return;

        var go = boxcld.gameObject;
        if (go == null)
            return;


        var graphic = go.GetComponent<Graphic> ();
        var rectTransform = go.GetComponent<RectTransform> ();
        if (graphic == null || rectTransform == null)
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
}
