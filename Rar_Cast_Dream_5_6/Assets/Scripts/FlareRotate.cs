﻿using UnityEngine;
using System.Collections;

public class FlareRotate : MonoBehaviour {

    float speed = 0.2f;

    void Start()
    {
        //StartCoroutine(myUpdate());
        Time.captureFramerate = 60;
        //Application.targetFrameRate = 60;
    }

    void FixedUpdate()
    {
        this.transform.Rotate(Vector3.up * speed * Time.deltaTime, 0.1f, Space.Self);
    }

    /*
    IEnumerator myUpdate()
    {
        while(true)
        {
            yield return null;
            this.transform.Rotate(Vector3.up * speed * Time.deltaTime, 0.1f, Space.Self);
        }
    }*/
}
