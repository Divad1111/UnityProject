using UnityEngine;
using UnityEditor;
using System.Collections;

public class HierarchyNameLengthSort : BaseHierarchySort 
{
	public override int Compare (GameObject lhs, GameObject rhs)
	{
		return lhs.name.CompareTo (rhs.name);
	}

	public override GUIContent content
	{
		get{return new GUIContent("NameLength");}
	}
}
