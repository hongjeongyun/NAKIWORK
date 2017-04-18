using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCount : MonoBehaviour {

    public TextMesh tm;

    float deltaTime = 0.0f;
    float msec;
    float fps;
    // Use this for initialization
	void Start () {
        tm.text = "";
		
	}
	
	// Update is called once per frame
	void Update () {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        msec = deltaTime * 1000f;
        fps = 1.0f / deltaTime;
        tm.text =  "FPS : " + fps.ToString();
	}
}
