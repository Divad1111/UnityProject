using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorExtensionTest  {

	[MenuItem("Test/OjectNamesTest")]
	static public void ObjectNamesTest()
	{
		Debug.Log(ObjectNames.GetClassName(Selection.activeObject));
		Debug.Log(ObjectNames.GetDragAndDropTitle(Selection.activeObject));
		Debug.Log(ObjectNames.GetInspectorTitle(Selection.activeObject));
		Debug.Log(ObjectNames.NicifyVariableName("__tName"));
		//ObjectNames.SetNameSmart(Selection.activeObject, "ttt");
	}

	static AnimationClip aniClip = null;
	[MenuItem("Test/StartAnimation")]
	static public void StartAnimation()
	{
//		if (!AnimationMode.InAnimationMode()) {
//			AnimationMode.StartAnimationMode();
//			aniClip = AssetDatabase.LoadAssetAtPath("Assets/Animation/record.anim", typeof(AnimationClip)) as AnimationClip;
//			AnimationMode.BeginSampling();
//			AnimationMode.SampleAnimationClip(Selection.activeGameObject,aniClip, 0.0f); 
//		}
		AnimationUtility.onCurveWasModified += (AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType deleted) =>
		{
			Debug.Log (clip.name + "  " + binding.path + "       " + binding.propertyName + "  " + deleted.ToString ());
		};
	}

	[MenuItem("Test/EndAnimation")]
	static public void EndAnimation()
	{
		if (AnimationMode.InAnimationMode()) {
			AnimationMode.SampleAnimationClip(Selection.activeGameObject,aniClip, 1.0f); 
			AnimationMode.EndSampling();
			AnimationMode.StopAnimationMode();		
		}
	}


	static EditorExtensionTest()
	{
		AnimationUtility.onCurveWasModified += (AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType deleted) =>
		{
			Debug.Log (clip.name + "  " + binding.path + "  " + binding.propertyName + "  " + deleted.ToString ());
		};
	}



}
