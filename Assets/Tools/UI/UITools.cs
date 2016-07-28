using UnityEngine;
using System.Collections;

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
}
