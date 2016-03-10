using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    Animation ani =  null;
	// Use this for initialization
	void Start () {
	    ani = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
        ani.wrapMode = WrapMode.Loop;
	
	}

    void AnimationEventFunc()
    {
        Debug.Log("动画事件触发");
    }
}
