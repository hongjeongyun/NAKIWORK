using UnityEngine;
using System.Collections;

public class DestroyBox : MonoBehaviour {
	
	public GameObject MeshParticle;
		
	void OnParticleCollision(GameObject tar) // 파티클하고 충돌했을때 콜백 함수
	{
		if (tar.name == "Muzzle") // 총에서 발사된 파티클이면
		{
			Damage();
		}
	}
	
	void Damage()
	{
		Instantiate(MeshParticle, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
