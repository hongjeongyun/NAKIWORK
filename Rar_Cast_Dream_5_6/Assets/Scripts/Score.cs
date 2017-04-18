using UnityEngine;
using System.Collections;
using NS_ENEMY;

namespace NS_ENEMY
{
	public class Score : MonoBehaviour 
	{	
		public static int score;
		
		public TextMesh selfComponent;
		
		void Start ()
		{
			score = 0;
			StartCoroutine (scoreUpdate ());
		}
		
		IEnumerator scoreUpdate()
		{
			while (true) 
			{
				yield return null;
				selfComponent.text = "" + score.ToString() + "p";
			}
		}
	}
}