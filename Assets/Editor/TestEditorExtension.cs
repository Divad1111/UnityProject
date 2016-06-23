using UnityEngine;
using UnityEditor;
using System.Collections;

public class TestEditorExtension  {

    [MenuItem("Tools/ObtainGUID")]
    public static void ObtainGUID()
    {
        Debug.Log(AssetDatabase.AssetPathToGUID("Assets/Resources/UITemplate/AAA.prefab"));
    }

    [MenuItem("Tools/Highlight")]
    public static void Highlight()
    {
        Highlighter.Highlight("Inspector", "Depth");
    }

    [MenuItem("Tools/HideHandle")]
    public static void HideHandle()
    {
        Tools.hidden = !Tools.hidden;
    }

    
}
