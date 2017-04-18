using UnityEngine;
using System.Collections;
using NS_GAMEMANAGER;

public class BlockMove : MonoBehaviour
{
    Vector3 pos; // 블럭과 플레이어 사이의 거리를 저장할 변수
    public float TargetX = 0f, HorizonTimer = 2.0f;
    bool onPlayer = false;
    public Valken Player;

	void Start()
	{
        if (HorizonTimer != 0f)
        {
            LeanTween.moveX(gameObject, TargetX, HorizonTimer).setLoopPingPong(); // Block 오브젝트를 현재 위치부터 TargetX 좌표까지 좌우로 반복이동합니다.
        }
		StartCoroutine (blockUpdate ());
	}

	IEnumerator blockUpdate()
	{
		while (true)
		{
			yield return null;
		    
			// 플레이어 밑에 있는 객체가 블럭이고, 플레이어는 멈춘 상태로, Collider가 충돌한 상태이면
			if (Player != null)
			{
				if (Player.RayGround () == gameObject && !Player.isMoving && onPlayer)
				{
					Player.transform.position = transform.position - pos; // 미리 저장했던 플레이어와 블럭 사이의 거리를 블럭의 위치에 더해, 플레이어 위치로 설정한다.
				} 
				else
				{
					pos = transform.position - Player.transform.position; // 블럭 위에서 이동하는 경우 거리를 재설정한다.
				}
			}
		}
	}
    
	/*
	void Update()
    {
        
    }*/

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Valken") // 블럭와 플레이어 충돌
        {
            pos = transform.position - Player.transform.position; // 플레이어와 블럭 사이의 거리를 설정한다.
            onPlayer = true; // 충돌 플래그를 true로 설정한다.
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "Valken") // 플레이어가 블럭에서 벗어난 경우
        {
            onPlayer = false; // 충돌 플래그를 false로 리셋한다.
        }
    }
}