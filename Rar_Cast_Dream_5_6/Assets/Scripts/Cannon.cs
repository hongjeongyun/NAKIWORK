using UnityEngine;
using System.Collections;
using NS_ENEMY;
using NS_GAMEMANAGER;

namespace NS_ENEMY
{
	public class Cannon : MonoBehaviour
	{
	    public Valken Player;
	    public GameObject MeshParticle;

		void Start()
		{
			StartCoroutine (cannonUpdate ());
		}
		
		IEnumerator cannonUpdate()
		{
			while (true)
			{
				yield return null;

				if(Player != null)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position), 2.0f * Time.deltaTime);
				}
			}
		}

	    void OnParticleCollision(GameObject tar) // 파티클하고 충돌했을때 콜백 함수
	    {
	        if (tar.name == "Muzzle") // 총에서 발사된 파티클이면
	        {
	            Damage();
				Score.score += 50; // 스코어 합산 추가
	        }
	    }

	    void Damage()
	    {
	        Instantiate(MeshParticle, transform.position, Quaternion.identity);
	        Destroy(gameObject);
	    }
	}
}