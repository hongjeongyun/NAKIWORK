using UnityEngine;
using System.Collections;

public class TextBlink : MonoBehaviour {

    public TextMesh textMesh;

    private string str;

    void Start ()
    {
        str = textMesh.text;
        StartCoroutine(BlinkText());
    }

    IEnumerator BlinkText()
    {
        while(true)
        {
            textMesh.text = "";
            yield return new WaitForSeconds(0.5f);
            textMesh.text = str;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
