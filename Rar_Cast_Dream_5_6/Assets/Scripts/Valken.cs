using UnityEngine;
using System.Collections;
using NS_GAMEMANAGER;

enum Direction // 캐릭터의 방향을 나타내는 열거형 배열
{
    LEFT,
    RIGHT,
}

namespace NS_GAMEMANAGER
{
	public class Valken : MonoBehaviour
	{
	    Animator anim; // 캐릭터의 애니메이션 객체
	    Direction dir = Direction.RIGHT; // 기본 방향값
	    
		public GameObject MeshParticle;
		public GameObject Bomb; // 폭탄 프리펩
	    public ParticleSystem RightMuzzle, LeftMuzzle, RightFire, LeftFire; // 메카닉의 좌측 총구, 우측 총구의 파티클시스템
	    public Transform LeftArm, RightArm; // 양쪽 팔의 객체
	    public ParticleSystem Boost; // 부스터 파티클 객체
	    public Transform MissilePoint; // 미사일이 발사될 포인트를 나타내는 객체

	    public Light LeftLight, RightLight; // 총알이 발사될때 주변을 밝게 하기 위한 포인트 라이트
		
	    bool bJump = false;
	    bool bBackedArm = false;

		public static bool isDie = false;

	    public bool isMoving = false; // 캐릭터가 점프했거나 좌우로 이동 중이거나 할때만 true이다.
	    
		//float JumpTimer = 0f; //

		public float armTurnSpeed;

		void Start()
	    {
	        anim = GetComponentInChildren<Animator>(); // 등록된 애니메이터를 얻어온다.
	        anim.Play("Walk"); // 걷기 동작
	        RightMuzzle.emissionRate = RightFire.emissionRate = LeftMuzzle.emissionRate = LeftFire.emissionRate = 0; // 파티클 초기화
			StartCoroutine(playerUpdate());
			StartCoroutine (ArmUpdate ());
		}

	    bool isGrounded() // Raycast를 이용하여 캐릭터 아래 땅이 있는지 여부를 체크한다.
	    {
	        return Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.1f, Vector3.down, 0.1f);
	    }

	    public GameObject RayGround() // 이동 블럭을 위해 isGrounded()와 같은 Raycast를 쏘고 충돌한 객체를 반환한다.
	    {
	        GameObject temp = null;
	        RaycastHit hit;
	        Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.1f, Vector3.down, out hit, 0.1f);
	        if (hit.collider != null) temp = hit.collider.gameObject; // Raycast와 충돌한 오브젝트를 리턴한다.
	        return temp;
	    }

        IEnumerator LightControl() // 머신건 공격시 코루틴으로 호출하여 0.03초 단위로 포인트 라이트를 반짝이게 한다.
        {
            while (true)
            {
                LeftLight.intensity = RightLight.intensity = 1.0f;
                yield return new WaitForSeconds(0.03f);
                LeftLight.intensity = RightLight.intensity = 0f;
                yield return new WaitForSeconds(0.03f);
            }
        }

        IEnumerator ArmUpdate()
		{
			while (true) 
			{
				yield return null;
		        
				if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && bBackedArm == false) // 복수 조건은 우선적
				{
				    TurnArm(360.0f,180.0f,armTurnSpeed);
				}

				//대각선 이 우선적이어야 함
				else if(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
				{
					if(dir != Direction.RIGHT)
						TurnArm(315.0f,135.0f,armTurnSpeed);
				}
				else if(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
				{
					if(dir != Direction.LEFT)
						TurnArm(315.0f,135.0f,armTurnSpeed);
				}
				else if(Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
				{
					if(dir != Direction.LEFT)
						TurnArm(405.0f, 225.0f,armTurnSpeed);
				}
				else if(Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
				{
					if(dir != Direction.RIGHT)
						TurnArm(405.0f, 225.0f,armTurnSpeed);
				}
				//
				else if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0 || Input.GetAxis("verticalDpad_360") > 0.0f ) // UP키를 누르면 양팔을 위쪽으로 회전
				{
					TurnArm(270.0f, 90.0f,armTurnSpeed);
				}
				else if (Input.GetKey(KeyCode.DownArrow)) // DOWN키를 누르면 양팔을 아래쪽으로 회전
				{
					TurnArm(450.0f, 270.0f,armTurnSpeed);
				}
			}
		}
	    	
		IEnumerator playerUpdate()
	    {
	        while(true)
	        {
	            yield return null;

				if (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("leftBumper_360")) // 이 키는 팔을 뒤로 젖히게 한다.
				{
					bBackedArm = true;
					TurnArm(-180.0f, 0.0f, 10.0f);
				}

				else if(!Input.GetKey(KeyCode.LeftShift) || !Input.GetButtonUp("leftBumper_360"))
				{ 
					if(bBackedArm)
					{
						bBackedArm = false;
					}
				}

				if ( Input.GetAxis("Horizontal") < 0  ||  Input.GetAxis("horizonDpad_360") < 0.0f  ) // 왼쪽으로 이동
	            {
	                if (!LeanTween.isTweening(gameObject)) // Tween이 작동하지 않는 상태인지 확인 (작동하면 회전 중이므로)
	                {
	                    if (isGrounded()) anim.Play("Walk"); // RayCast로 땅이 검출됐으면 걷기 동작
	                    else anim.Play("Idle"); // 공중에 떠있으면 Idle 동작 (비행 애니를 따로 만들어도 된다.)

	                    if (dir != Direction.LEFT) // 오른쪽을 향한 상태면
	                    {
	                        LeanTween.rotateAroundLocal(gameObject, Vector3.up, 180f, 0.3f).setOnComplete(TurnLeft); // 캐릭터를 0.3초 동안 180도 회전시킨다.
	                    }
	                    else
	                    {
	                        transform.Translate(Vector3.forward * 8.0f * Time.deltaTime); // 캐릭터가 바라보는 방향으로 이동
	                    }
	                    isMoving = true;
	                }
	            }
	            else if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("horizonDpad_360") > 0.0f) // 오른쪽으로 이동
	            {
	                if (!LeanTween.isTweening(gameObject)) // Tween이 작동하지 않는 상태인지 확인 (작동하면 회전 중이므로)
	                {
	                    if (isGrounded()) anim.Play("Walk"); // RayCast로 땅이 검출됐으면 걷기 동작
	                    else anim.Play("Idle"); // 공중에 떠있으면 Idle 동작 (비행 애니를 따로 만들어도 된다.)

	                    if (dir != Direction.RIGHT) // 왼쪽을 향한 상태면
	                    {
	                        LeanTween.rotateAroundLocal(gameObject, Vector3.up, -180f, 0.3f).setOnComplete(TurnRight); // 캐릭터를 0.3초 동안 180도 회전시킨다.
	                    }
	                    else
	                    {
	                        transform.Translate(Vector3.forward * 8.0f * Time.deltaTime); // 캐릭터가 바라보는 방향으로 이동
	                    }
	                    isMoving = true;
	                }
	            }
	            else
	            {
	                anim.Play("Idle"); // 방향키를 누르지 않았으면 멈춰있는 애니메이션
	                isMoving = false;
				}
	           
	            if (Input.GetKey(KeyCode.Z) || Input.GetButton("A_360")) // 땅에 착지한 상태면 점프시킨다.
	            {
	                bJump = true; // FiexeUpdate 에서 따로 연산을 하기위한 플래그가 작동한다.

	                if (!Boost.main.loop) // 부스터 연기가 loop 상태가 아닐 때만
	                {
	                    Boost.Play(); // 부스터 연기를 발생시킨다.
                        Boost.loop = true; // loop를 true로 설정하여 연기가 계속 나오게 한다.
	                }

	                isMoving = true;
	            }

	            else
	            {
	                bJump = false;
                    Boost.loop = false; // 부스터의 loop를 false로 설정해서 연기가 더이상 나오지 않게 한다.
	            }

	            if (Input.GetKey(KeyCode.X) || Input.GetButton("rightBumper_360")) // X키를 누르면 양쪽 총구에서 총알을 발사한다. (개선필요)
	            {
	                if (!GetComponent<AudioSource>().isPlaying)
	                {
	                    GetComponent<AudioSource>().Play(); // 기관총 소리를 공격이 끝날때까지 Loop로 연주한다.
	                    StartCoroutine("LightControl"); // 라이트 조절을 위한 코루틴을 실행한다.
	                }
	                RightMuzzle.emissionRate = LeftMuzzle.emissionRate = 10; // 총알 파티클 활성화
	                RightFire.emissionRate = LeftFire.emissionRate = 30; // 건파이어 파티클 활성화
	            }
	            else
	            {
	                GetComponent<AudioSource>().Stop(); // 기관총 소리를 끈다.
	                RightMuzzle.emissionRate = RightFire.emissionRate = LeftMuzzle.emissionRate = LeftFire.emissionRate = 0; // 모든 파티클 리셋
	                LeftLight.intensity = RightLight.intensity = 0f; // 라이트 리셋
	                StopCoroutine("LightControl"); // 라이트 코루틴 정지
	            }

	            if (Input.GetKeyDown(KeyCode.C)) // 폭탄 프리펩을 생성한다.
	            {
	                if (!LeanTween.isTweening(gameObject))
	                {
	                    // 오른쪽을 보고 있으면 오른쪽에, 왼쪽을 보고 있으면 왼쪽에 생성시킨다.
	                    Vector3 pos = Vector3.zero;

	                    if (dir == Direction.RIGHT) pos = new Vector3(transform.position.x + 1.0f, transform.position.y + 1.0f, transform.position.z);
	                    if (dir == Direction.LEFT) pos = new Vector3(transform.position.x - 1.0f, transform.position.y + 1.0f, transform.position.z);

	                    for (int i = 0; i < 5; i++) // 한번 공격때마다 5개의 미사일 생성 (오브젝트 풀링으로 교체해야 함)
	                    {
	                        Vector3 origin = pos + Vector3.up * Random.Range(-1f, 1f) + Vector3.left * Random.Range(-1f, 1f);
	                        GameObject temp = Instantiate(Bomb, origin, Quaternion.AngleAxis(dir == Direction.RIGHT ? 0f : 180f, Vector3.up)) as GameObject;
	                        Vector3 tarPos = MissilePoint.position + MissilePoint.forward * 20f + MissilePoint.up * Random.Range(-1f, 1f);
	                        temp.SendMessage("LaunchMissile", tarPos); // Missile.cs의 미사일 발사 함수 실행
	                    }
	                }
	            }
	        }
	    }

	    void TurnArm(float rightArmAngle, float leftArmAngle , float TurnSpeed)
	    {
	        Quaternion rightArmRotation = RightArm.rotation;
	        Quaternion leftArmRotation = LeftArm.rotation;

	        rightArmRotation.eulerAngles = new Vector3(RightArm.eulerAngles.x, RightArm.eulerAngles.y, rightArmAngle);
	        leftArmRotation.eulerAngles = new Vector3(LeftArm.eulerAngles.x, LeftArm.eulerAngles.y, leftArmAngle);

	        RightArm.rotation = Quaternion.Slerp(RightArm.rotation, rightArmRotation, TurnSpeed * Time.deltaTime);
	        LeftArm.rotation = Quaternion.Slerp(LeftArm.rotation, leftArmRotation, TurnSpeed * Time.deltaTime);
	    }

	    void TurnLeft() // 좌측 회전이 완료된 경우
	    {
	        transform.position = new Vector3(transform.position.x, transform.position.y, 0f); // Z값을 초기화
	        dir = Direction.LEFT;
	    }

	    void TurnRight() // 우측 회전이 완료된 경우
	    {
	        transform.position = new Vector3(transform.position.x, transform.position.y, 0f); // Z값을 초기화
	        dir = Direction.RIGHT;
	    }

        void Damage()
        {
            Instantiate(MeshParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            isDie = true;
            Debug.Log("DIE_OK");
        }

        void OnCollisionEnter(Collision col)
		{
			if (col.gameObject.name == "Enemy01") 
			{
				//Debug.Log("COLLISION_OK");
				Damage();
			}
		}

        void FixedUpdate()
        {
            if (bJump)
            {
                GetComponent<Rigidbody>().GetComponent<ConstantForce>().force = Vector3.zero; // constantForce를 0으로 리셋해서 중력 적용을 덜받게 한다.
                if (GetComponent<Rigidbody>().velocity.y < 4f) GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 20f); // 플레이어를 서서히 떠오르게 한다. 파워가 4f를 넘지 않도록 한다.
            }
            else
            {
                GetComponent<Rigidbody>().GetComponent<ConstantForce>().force = new Vector3(0f, -10f, 0f); // 점프키를 뗀 상태면 다시 10f의 중력을 적용한다.
            }
        }

        //RightArm.Rotate(Vector3.back * 200f * Time.deltaTime);
        //LeftArm.Rotate(Vector3.back * 200f * Time.deltaTime);
    }
}