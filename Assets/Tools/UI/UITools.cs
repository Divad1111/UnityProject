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
//#if UGUI
    public static T AddMissingComponent<T>(this GameObject go) where T:Component
    {
        T com = go.GetComponent<T> ();
        if (com != null)
            return com;
        
        return go.AddComponent<T> ();
    }
//#endif

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

    public static void AdjustBoxColliderSize(BoxCollider boxcld)
    {
        if (boxcld == null)
            return;

        var go = boxcld.gameObject;
        if (go == null)
            return;

        var rectTransform = go.GetComponent<RectTransform> ();
        if (rectTransform == null)
            return;

        boxcld.center = new Vector3 (rectTransform.rect.width * (0.5F - rectTransform.pivot.x), rectTransform.rect.height * (0.5F - rectTransform.pivot.y ), 1F);
        boxcld.size = new Vector3 (rectTransform.rect.width, rectTransform.rect.height, 1F);
        boxcld.isTrigger = true;
    }

    public static IEnumerator DelayCallback(float delay, System.Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }

    /// <summary>
    /// Add a new child game object.
    /// </summary>

    static public GameObject AddChild (GameObject parent)
    {
        GameObject go = new GameObject();
        if (parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }

    /// <summary>
    /// Instantiate an object and add it to the specified parent.
    /// </summary>

    static public GameObject AddChild (GameObject parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
#if UNITY_EDITOR
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }


    /// <summary>
    /// Destroy the specified object, immediately if in edit mode.
    /// </summary>

    static public void Destroy (UnityEngine.Object obj)
    {
        if (obj)
        {
            if (obj is Transform)
            {
                Transform t = (obj as Transform);
                GameObject go = t.gameObject;

                if (Application.isPlaying)
                {
                    t.parent = null;
                    UnityEngine.Object.Destroy(go);
                }
                else UnityEngine.Object.DestroyImmediate(go);
            }
            else if (obj is GameObject)
            {
                GameObject go = obj as GameObject;
                Transform t = go.transform;

                if (Application.isPlaying)
                {
                    t.parent = null;
                    UnityEngine.Object.Destroy(go);
                }
                else UnityEngine.Object.DestroyImmediate(go);
            }
            else if (Application.isPlaying) UnityEngine.Object.Destroy(obj);
            else UnityEngine.Object.DestroyImmediate(obj);
        }
    }

    /// <summary>
    /// Convenience extension that destroys all children of the transform.
    /// </summary>

    static public void DestroyChildren (this Transform t)
    {
        bool isPlaying = Application.isPlaying;

        while (t.childCount != 0)
        {
            Transform child = t.GetChild(0);

            if (isPlaying)
            {
                child.parent = null;
                UnityEngine.Object.Destroy(child.gameObject);
            }
            else UnityEngine.Object.DestroyImmediate(child.gameObject);
        }
    }

    /// <summary>
    /// Determines whether the 'parent' contains a 'child' in its hierarchy.
    /// </summary>

    static public bool IsChild (Transform parent, Transform child)
    {
        if (parent == null || child == null) return false;

        while (child != null)
        {
            if (child == parent) return true;
            child = child.parent;
        }
        return false;
    }

    /// <summary>
    /// Recursively set the game object's layer.
    /// </summary>

    static public void SetLayer (GameObject go, int layer)
    {
        go.layer = layer;

        Transform t = go.transform;

        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            SetLayer(child.gameObject, layer);
        }
    }
}
