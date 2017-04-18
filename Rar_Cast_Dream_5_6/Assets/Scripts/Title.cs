using UnityEngine;
using UnityEngine.SceneManagement;
//using System.Collections;

public class Title : MonoBehaviour {

    public TextMesh[] textMeshes;

    public GameObject go_PressAnyButtonText;
    public GameObject go_Cursor;

    void Awake()
    {
        //Time.captureFramerate = 60;
        //Application.targetFrameRate = 60;
    }

    void Update()
	{
		if (true == go_PressAnyButtonText.activeInHierarchy && true == Input.anyKeyDown)
		{
			go_PressAnyButtonText.SetActive (false);
			textMeshes [0].gameObject.SetActive (true);
			textMeshes [1].gameObject.SetActive (true);
			go_Cursor.SetActive (true);
		}

		else if(false == go_PressAnyButtonText.activeInHierarchy && go_Cursor.activeInHierarchy)
		{
			if (Input.GetKeyDown (KeyCode.DownArrow))
			{
				Vector3 pos = go_Cursor.transform.position;
				pos.y -= 1.0f;
				go_Cursor.transform.position = pos;
			}
			else if (Input.GetKeyDown (KeyCode.UpArrow))
			{
				Vector3 pos = go_Cursor.transform.position;
				pos.y += 1.0f;
				go_Cursor.transform.position = pos;
			}

			else if ( go_Cursor.transform.position.y == -1f )
			{
				if(Input.GetKeyDown(KeyCode.Return))
				{
					Debug.Log("-1");
					//Application.LoadLevel("Rar Cast Dream 3");
                    SceneManager.LoadScene("Rar Cast Dream 3");
				}
			}
			else if ( go_Cursor.transform.position.y == -2f )
			{
				if(Input.GetKeyDown(KeyCode.Return))
				{
					Debug.Log("-2");

				}
			}
			else if ( go_Cursor.transform.position.y == -3f )
			{
				if(Input.GetKeyDown(KeyCode.Return))
				{
					Debug.Log("-3");
					Application.Quit();
				}
			}
		}
	}
}