using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public static float fTime;
	public TextMesh selfComponent;


	void Start () 
	{
		fTime = 181.0f;
		selfComponent.text = "";
		StartCoroutine (timerUpdate ());
	}
	
	IEnumerator timerUpdate()
	{
		while (true) 
		{
			yield return null;

			if(fTime != 0)
			{
				fTime -= Time.fixedDeltaTime * 0.5f;
			    
				if(fTime <=0)
					fTime = 0;
			}
		
			int iTime = Mathf.FloorToInt(fTime); // float 에서 int 로 절사
			selfComponent.text = "" + iTime.ToString() + "s";
		}
	}
}