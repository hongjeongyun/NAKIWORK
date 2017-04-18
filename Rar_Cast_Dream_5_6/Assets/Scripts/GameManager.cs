using UnityEngine;
using System.Collections;
using NS_GAMEMANAGER;

public class GameManager : MonoBehaviour 
{
	public string playerName; 
	public static Transform playerTransform;

	void Awake()
	{
        //Time.captureFramerate = 60;
        //Application.targetFrameRate = 60;
        playerTransform = GameObject.Find (playerName).GetComponent<Transform> ();
	}
	   
	void Update()
	{
		if(Valken.isDie == true)
		{
			StartCoroutine(GameOver());
		}
	}

	IEnumerator GameOver()
	{
		yield return new WaitForSeconds (2.0f);
        //Application.LoadLevel ("GameOverScene");
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
    }
}