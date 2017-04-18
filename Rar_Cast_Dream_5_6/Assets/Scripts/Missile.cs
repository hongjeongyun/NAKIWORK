using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    public GameObject Explosion;

	void LaunchMissile(Vector3 tarPos) // 미사일을 발사한다.
	{
        Invoke("SetActive", 0.9f); // 미사일이 발사되면 뒤로 날아갔다가 앞으로 나아가기 때문에 0.9초간 딜레이를 준다.
        LeanTween.move(gameObject, tarPos, 1.6f).setEase(LeanTweenType.easeInBack).setOnComplete(Explode); // 타겟포지션까지 1.6초 동안 이동시킨다.
	}

    void SetActive()
    {
        GetComponent<Collider>().enabled = true; // 컬라이더를 활성화시킨다.
    }

    void Explode() // 목적지까지 이동했거나 다른 객체와 충돌한 경우 호출
    {
        Instantiate(Explosion, transform.position, Quaternion.identity); // 폭팔 파티클 생성
        Destroy(gameObject); // 미사일 제거
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (col.tag == "Box")
            {
                col.GetComponent<Rigidbody>().AddExplosionForce(2000f, transform.position, 6.0f); // 박스와 충돌하면 박스를 날아가게 한다.
            }
            else if (col.tag == "Enemy")
            {
                col.SendMessage("Damage"); // 적과 충돌했으면 적을 폭파시킨다.
            }
            Explode();
        }
    }
}
