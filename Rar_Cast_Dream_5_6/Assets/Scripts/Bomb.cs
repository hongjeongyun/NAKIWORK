using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
	void Start()
	{
        Invoke("Explode", 2.0f); // 2초 후에 Explode 함수 호출

        // 2초 후에 0.5초 동안 스케일을 10f까지 늘린다. 스케일이 다 늘어나면 오브젝트를 삭제한다.
        LeanTween.scale(gameObject, new Vector3(6.0f, 6.0f, 6.0f), 0.5f).setEase(LeanTweenType.easeOutExpo).setDelay(2.0f).setDestroyOnComplete(true);
        // 2초 후에 오브젝트의 알파값을 0.5초간 감소시킨다.
        LeanTween.alpha(gameObject, 0f, 0.5f).setDelay(2.0f);
	}

    void Explode()
    {
        GetComponent<Rigidbody>().useGravity = false; // 리지드바디의 중력 적용을 끈다.
        GetComponent<Collider>().enabled = false; // 콜라이더를 비활성화 한다.

        GameObject[] Boxes = GameObject.FindGameObjectsWithTag("Box"); // 현재 씬의 모든 박스를 가져온다.

        for (int i = 0; i < Boxes.Length; i++)
        {
            Boxes[i].GetComponent<Rigidbody>().AddExplosionForce(2000f, transform.position, 6.0f); // 폭탄이 터진 지점에서 폭발 포스를 발생시킨다.
        }
    }
}
