using UnityEngine;
using System.Collections;
using NS_ENEMY;

namespace NS_ENEMY
{
	public class Enemy : MonoBehaviour
	{
	    Animator anim;
	    public GameObject MeshParticle;
	    public float speed = 1.0f;
	    bool CanMove = true;

		void Start()
		{
	        anim = GetComponentInChildren<Animator>();
	        anim.Play("Walk");
			StartCoroutine (enemyUpdate ());
		}
		
		IEnumerator enemyUpdate()
		{
			while (true) 
			{
				yield return null;

				if (CanMove)
				{
					transform.Translate(Vector3.forward * speed * Time.deltaTime); // 캐릭터가 바라보는 방향으로 이동
					
					if (!isGrounded() || CheckFront())
					{
						anim.Play("Idle");
						LeanTween.rotateAroundLocal(gameObject, Vector3.up, 180f, 0.5f).setOnComplete(CompleteMove);
						CanMove = false;
					}
				}
			}
		}

	    void CompleteMove()
	    {
	        anim.Play("Walk");
	        CanMove = true;
	    }

	    bool CheckFront()
	    {
	        return Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.5f, transform.forward, 1.0f);
	    }

	    bool isGrounded() // Raycast를 이용하여 캐릭터 아래 땅이 있는지 여부를 체크한다.
	    {
	        return Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.1f, Vector3.down, 0.1f);
	    }

	    void OnParticleCollision(GameObject tar) // 파티클하고 충돌했을때 콜백 함수
	    {
	        if (tar.name == "Muzzle") // 총에서 발사된 파티클이면
	        {
	            Damage();
				Score.score += 100; // 스코어 합산 추가
	        }
	    }

	    void Damage()
	    {
	        Instantiate(MeshParticle, transform.position, Quaternion.identity);
	        Destroy(gameObject);
	    }
	}
}