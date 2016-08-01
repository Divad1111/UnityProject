using UnityEngine;
using System.Collections;
using UnityEditor.UI;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(ImageExt), true)]
[CanEditMultipleObjects]
public class ImageEditorExt : ImageEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI ();

        var image = target as ImageExt;
        image.isAdjustSize = EditorGUILayout.Toggle ("IsAutoSize", image.isAdjustSize);
    }
}
