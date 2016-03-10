using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class UITemplate : MonoBehaviour {

#if UNITY_EDITOR
    [HideInInspector] public string GUID = string.Empty;
	[HideInInspector] [System.NonSerialized]public Dictionary<GameObject, int> searPrefabs = new Dictionary<GameObject, int>();
    [ContextMenu("GenerateGUID")]
    public void InitGUID()
    {
        if(string.IsNullOrEmpty(GUID))
        {   
            GUID = System.Guid.NewGuid().ToString();
        }
    }

    void OnDisable()
    {
        //Don't disable the component.
        enabled = true;
    }
#endif
}
