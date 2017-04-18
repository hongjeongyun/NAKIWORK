using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour{    
	void OnParticleCollision(GameObject tar){ // 파티클하고 충돌했을때 콜백 함수
		if (tar.name == "Muzzle"){ // 총에서 발사된 파티클이면
            GetComponent<Rigidbody>().AddForce(tar.transform.forward * 800f); // 총알이 날아온 방향으로 데미지를 준다.
        }
    }
}