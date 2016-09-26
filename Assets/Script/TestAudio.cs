using UnityEngine;
using System.Collections;

public class TestAudio : MonoBehaviour {

    public AudioClip playClip;
    AudioSource aso;
   
    public int value = 1;
	// Use this for initialization
	void Start () {
        aso = GetComponent<AudioSource> ();
	
	}
	
	// Update is called once per frame
	void Update () {

        if (aso == null)
            return;

        Debug.Log (aso.timeSamples);
        Debug.Log (aso.time);
	
	}

    [ContextMenu("PlayOther")]
    void PlayOther()
    {
        if (aso == null)
            return;

        if (playClip == null)
            return;
        
        aso.PlayOneShot (playClip);
    }

    [ContextMenu("SetTime")]
    void SetTime()
    {
        if (aso == null)
            return;

        aso.time = 10F;
    }

    [ContextMenu("PlayAtPoint")]
    void PlayAtPoint()
    {
        AudioSource.PlayClipAtPoint (playClip, new Vector3(1000F, 1000F, 1000F), 2F);
    }

    [ContextMenu("TestPause")]
    void TestPause()
    {
        aso.ignoreListenerPause = true;
        AudioListener.pause = true;
    }

    [ContextMenu("TestResume")]
    void TestResume()
    {
        AudioListener.pause = false;
    }

    [ContextMenu("TestPlayScheduled")]
    void TestPlayScheduled()
    {
        if (aso == null)
            return;

        aso.SetScheduledStartTime (5.0F);
        //aso.PlayScheduled (AudioSettings.dspTime + 5.0F);
        aso.SetScheduledEndTime(60F);
        //aso.Play ();
    }
}
