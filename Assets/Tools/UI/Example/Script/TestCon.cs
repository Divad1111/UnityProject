using UnityEngine;
using System.Collections;

public class TestCon : MonoBehaviour {

    bool _isFirstUpdate = true;
	// Use this for initialization
//    IEnumerator Start () 
//    {
////        Debug.Log ("1111");
////        yield return null;
////
////        Debug.Log ("2222");
////        yield return null;
////
////        Debug.Log ("333");
////        yield return null;
//	}
	
	// Update is called once per frame
	void Update () {
	
        Debug.Log ("XXXXXXX");
        if(_isFirstUpdate)
            StartCoroutine(Test());

        _isFirstUpdate = false;
        
	}

    IEnumerator Test()
    {
        Debug.Log ("1111");
        yield return null;

        Debug.Log ("2222");
        yield return null;

        Debug.Log ("333");
        yield return null;
    }
}
