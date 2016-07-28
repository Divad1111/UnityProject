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

    [MenuItem("GameObject/UI/Tools/ShortCutCreate/Canvas %#e")]
    static void Canvas()
    {
        EditorApplication.ExecuteMenuItem ("GameObject/UI/Canvas");
    }
}
