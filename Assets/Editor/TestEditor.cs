using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomEditor(typeof(Test))]
public class TestEditor : Editor {

    float coneSize = 1;
    public override void OnInspectorGUI()
    {
        //Test tar = target as Test;

        //if (GUILayout.Button("Process"))
        //{
        //    EditorUtility.DisplayProgressBar("process", "coping", 0.5f);
        //}
        //tar.canJump = EditorGUILayout.Toggle("Can Jump", tar.canJump);

        //EditorGUIUtility.AddCursorRect(new Rect(0, 0, 100, 100), MouseCursor.Link);
    
        //// Disable the jumping height control if canJump is false:
        //EditorGUI.BeginDisabledGroup(tar.canJump == false);
        //tar.jumpHeight = EditorGUILayout.FloatField("Jump Height", tar.jumpHeight);

        //EditorGUILayout.HelpBox("This help box", MessageType.Warning);

        //EditorGUI.EndDisabledGroup();
    }


    void OnSceneGUI()
    {
        var goTarget = target as Test;
        Handles.color = Color.red;
        Handles.ConeCap(0,
            goTarget.transform.position + new Vector3(-5, 0, 0),
            goTarget.transform.rotation,
            coneSize);
        Handles.color = Color.green;
        Handles.ConeCap(0,
            goTarget.transform.position + new Vector3(0, -5, 0),
            goTarget.transform.rotation,
            coneSize);
        Handles.color = Color.blue;
        Handles.ConeCap(0,
            goTarget.transform.position + new Vector3(0, 0, -5),
            goTarget.transform.rotation,
            coneSize);
    }

}
