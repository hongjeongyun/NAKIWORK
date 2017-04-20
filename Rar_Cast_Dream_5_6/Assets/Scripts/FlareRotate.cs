using UnityEngine;
using System.Collections;

public class FlareRotate : MonoBehaviour {

    float speed = 0.2f;

    void Start()
    {
        StartCoroutine(myUpdate());
    }

    /*
    void FixedUpdate()
    {
        //this.transform.Rotate(Vector3.up * speed, 0.1f, Space.Self);
    }*/

    IEnumerator myUpdate()
    {
        while(true)
        {
            yield return null;
            //this.transform.Rotate(Vector3.up * speed * Time.deltaTime, 0.1f, Space.Self);   

            Quaternion rot = this.transform.rotation;
            rot.y = (rot.y += 360.0f * Time.deltaTime) * 0.01f;

            if (rot.y > 360.0f)
            {
                rot.y = 0;
                this.transform.rotation = Quaternion.identity;
            }

            Vector3 r = new Vector3(0, rot.y, 0);

            this.transform.Rotate(r);
        }
    }
}
