using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour 
{

    public event System.Action<object> OnTimer = null;

    public float time { get; set; } //多少秒后开始
    public float repeatTime { get; set; } //周期时间 小于零表示是一次性的
    public object userData { get; set; } //用户数据

    float _curTime = 0F;	
	
	void Update () 
    {
        if (OnTimer == null)
        {
            Debug.LogError ("OnTimer is null. please set OnTimer callback.");
            return;
        }
        
        _curTime += Time.deltaTime;

        if (_curTime >= time)
        {
            OnTimer (userData);

            if (repeatTime <= 0.0001F)
            {
                Destroy (this);
            }
            else
            {
                _curTime = 0F;
                time = repeatTime;
            }
        }
	}
}
