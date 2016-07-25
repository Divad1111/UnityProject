using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour 
{

    public event System.Action<object> OnTimer = null;

    public float time { get; set; } //多少秒后开始
    public float repeatTime { get; set; } //周期时间 小于零表示是一次性的

    float _curTime = 0F;

	void Start () 
    {
	    
	}
	
	
	void Update () 
    {
	
	}
}
