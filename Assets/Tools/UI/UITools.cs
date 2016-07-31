using UnityEngine;
using System.Collections;
using System.Text;


public enum EffectPlayPosition
{
    Front,
    Middle,
    Back
}

public static class UITools
{
    public static T AddMissingComponent<T>(this GameObject go) where T:Component
    {
        T com = go.GetComponent<T> ();
        if (com != null)
            return com;
        
        return go.AddComponent<T> ();
    }

    public static Transform Find(Transform parent, string name)
    {
        if (parent == null)
            return null;

        if (parent.name == name)
            return parent;

        for (int i = 0; i < parent.childCount; ++i)
        {
            var result = Find (parent.GetChild (i), name);
            if (result != null)
                return result;
        }

        return null;
    }

    public static void PlayEffect(GameObject go, EffectPlayPosition pos, string effectName)
    {
        if(go == null)
        {
            Debug.LogError ("go is null.");
            return;
        }

        if (string.IsNullOrEmpty (effectName))
        {
            Debug.LogError ("effectName is empty.");
            return;
        }

    }

    public static string GetPath(GameObject go)
    {
        if (go == null)
        {
            Debug.LogError ("go is null.");
            return string.Empty;
        }

        StringBuilder sb = new StringBuilder();

        sb.Insert(0, go.name);
        Transform parent = go.transform.parent;
        while(parent != null)
        {
            sb.Insert(0, parent.name + "/");
            parent = parent.parent;
        }
        return sb.ToString();
    }
}
