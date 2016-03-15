using UnityEngine;
using UnityEditor;
using System.Collections;

public class TestEditorExtension  {

    [MenuItem("Tools/ObtainGUID")]
    public static void ObtainGUID()
    {
        Debug.Log(AssetDatabase.AssetPathToGUID("Assets/Resources/UITemplate/AAA.prefab"));
    }

    
}
