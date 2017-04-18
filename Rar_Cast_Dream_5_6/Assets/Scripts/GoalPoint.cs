using UnityEngine;
using System.Collections;


public class GoalPoint : MonoBehaviour {
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name == "Valken") 
		{
			Application.LoadLevel("ClearScene");
			//Debug.Log("GOAL_OK");
		}
	}
}