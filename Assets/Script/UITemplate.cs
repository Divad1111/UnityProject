using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class UITemplate : MonoBehaviour {

#if UNITY_EDITOR
    [HideInInspector] public string GUID = string.Empty;
	[HideInInspector] [System.NonSerialized]public List<GameObject> searPrefabs = new List<GameObject>();
    public void InitGUID()
    {
        if(string.IsNullOrEmpty(GUID))
        {   
            GUID = System.Guid.NewGuid().ToString();
        }
    }
#endif

}
