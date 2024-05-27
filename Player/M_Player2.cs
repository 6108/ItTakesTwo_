using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PathCreation;

public class M_Player2 : MonoBehaviour
{
    #region 체력
    public int hp = 10;
    public bool isDie = false; //죽었는가?
    #endregion

    #region 이동
    CharacterController cc; //컨트롤러
    public float speed = 5; //이동속도
    public float gravity = -10; //중력 
    public float yVelocity; //현재 높이
    public float jumpPower = 5; //점프 높이
    int jumpCount = 0; //2단 점프, 점프제한
    #endregion

    #region 대시
    public float dashTime = 0.5f;
    public float dashSpeed = 10;
    #endregion

    #region 카메라 회전
    public Camera playerCamera; //플레이어 카메라
    public CinemachineFreeLook freeLookCam; //시네머신
    public float rotSpeed = 205; //회전 속도
    float mouseX = 0;
    float mouseY = 0;
    #endregion

    #region 후크점프
    GameObject hook;
    public bool isHookRange = false;
    public bool isHook = false;
    public Transform hookPosition;
    public GameObject hookPrefab;
    #endregion

    #region 레일
    public PathCreator path1;
    public PathCreator path2;
    public PathCreator curPath;
    EndOfPathInstruction end;
    public float railSpeed = 5;
    public float distance = 0;
    public bool isRail = false;
    bool isRailJump = false;
    Vector3 startPosition;
    Vector3 midPosition;
    Vector3 targetRailPosition;
    #endregion

    #region 자동 추적
    List<GameObject> allEnemies = new List<GameObject>();
    GameObject nearEnemy;
    public GameObject aimImage;
    #endregion

    #region 공격
    bool isAttack = false; //공격 가능한가?
    public GameObject bulletPrefab; //총알
    public Transform firePosition; //총구 위치
    public Vector3 targetPosition; //타겟 위치
    public float attackSpeed = 5; //공격 속도
    float time; //쿨타임 비교용
    AudioSource audioSource;
    public float cameraCenter = 0.75f;
    #endregion

    //애니메이션
    public Animator anim;
    public static M_Player2 instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mouseX = transform.eulerAngles.y;
        //mouseY = -transform.eulerAngles.x;
        cc = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            allEnemies.Add(enemy);
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDie)
            return;
        if (hp <= 0)
            Die();
        if (Input.GetKeyDown(KeyCode.P))
        {
            Hit();
        }
        ZoomINOut();
        if (GetComponent<D_BeesManager>())
            EnemyCheck();
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartCoroutine(IeDash());
        if (Input.GetButton("Fire1") && isAttack)
        {
            if (time > 0.2f)
            {
                StartCoroutine(IeAttack());
                time = 0;
            }
        }
        Hook();
        if (isRailJump)
            RailJump();
        else if (isRail)
            RailMove();
        else if (isHook)
        {
            HookMove();
        }
        else
            Move();
    }

    //우클릭 화면 확대, 우클릭에서 손 떼면 화면 줌 아웃
    //우클릭 상태에서만 공격 가능
    void ZoomINOut()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(IeZoomIn());
            aimImage.SetActive(true);
            isAttack = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            StartCoroutine(IeZoomOut());
            aimImage.SetActive(false);
            isAttack = false;
        }
    }

    //줌 인
    //화면 확대하고 살짝 왼쪽으로
    IEnumerator IeZoomIn()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.01f);
            freeLookCam.m_Lens.FieldOfView -= 0.5f;
            freeLookCam.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenX -= 0.01f;
            freeLookCam.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenX -= 0.01f;
            freeLookCam.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenX -= 0.01f;

        }
    }

    //줌 아웃
    //화면 축소하고 다시 정면보게
    IEnumerator IeZoomOut()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.01f);
            freeLookCam.m_Lens.FieldOfView += 0.5f;
            freeLookCam.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenX += 0.01f;
            freeLookCam.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenX += 0.01f;
            freeLookCam.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenX += 0.01f;

        }
    }

    void Move()
    {
        //마우스 값 입력 받고 회전
        float mh = Input.GetAxis("Mouse X");
        mouseX += mh * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, mouseX, 0);
        freeLookCam.m_XAxis.Value = mouseX;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir = playerCamera.transform.TransformDirection(dir);
        yVelocity += gravity * Time.deltaTime;
        if (isHookRange && transform.parent)
        {
            dir = transform.forward;
            dir.y = yVelocity;
            cc.Move(dir * speed * Time.deltaTime);
        }
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            jumpCount = 0;
            isHookRange = false;
            transform.parent = null;
            Destroy(hook);
        }
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            anim.SetTrigger("JumpUp");
            dir *= 2;
            yVelocity = jumpPower;
            jumpCount++;
        }
        dir.y = yVelocity;
        if(dir.x == 0 || dir.z == 0)
            anim.SetTrigger("Idle");
        else
            anim.SetTrigger("Run");
        cc.Move(dir * speed * Time.deltaTime);
    }

    IEnumerator IeDash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void Hook()
    {
        if (isHookRange)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!isHook)
                    HookON(hookPosition.position);
                else
                    HookOff();
            }
        }
    }

    public void HookON(Vector3 hookPosition)
    {
        //후크 생성
        hook = Instantiate(hookPrefab, hookPosition, Quaternion.identity);
        transform.parent.transform.parent = hook.transform;
        cc.enabled = false;
        isHook = true;
    }

    public void HookOff()
    {

        cc.enabled = true;
        isHook = false;
        yVelocity = transform.position.y + jumpPower;
        hook.GetComponent<ConfigurableJoint>().breakForce = 0;
        isHook = false;
    }

    void HookMove()
    {
        float mh = Input.GetAxis("Mouse X");
        mouseX += mh * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, mouseX, 0);
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir = playerCamera.transform.TransformDirection(dir);
        transform.parent.transform.parent.GetComponent<Rigidbody>().AddForce(dir);
    }

    void RailMove()
    {
        anim.SetTrigger("Idle");
        if (!curPath)
        {
            curPath = path1;
            //path2 = null;
        }
        if (curPath.GetComponent<M_Rail>().pathDir == -1)
        {
            transform.GetChild(2).localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.GetChild(2).localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (curPath.path.GetClosestTimeOnPath(transform.position) > 0.99f)
            isRail = false;
        if (path1)
        {
            if (path1.GetComponent<M_Rail>().pathDir == -1)
            {
                if (path1.path.GetClosestTimeOnPath(transform.position) < 0.01f)
                    path1 = null;
            }
            else
            {
                if (path1.path.GetClosestTimeOnPath(transform.position) > 0.99f)
                    path1 = null;
            }
        }
        if (path2)
        {
            if (path2.GetComponent<M_Rail>().pathDir == -1)
            {
                if (path2.path.GetClosestTimeOnPath(transform.position) < 0.01f)
                    path2 = null;
            }
            else
            {
                if (path2.path.GetClosestTimeOnPath(transform.position) > 0.99f)
                    path2 = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            startPosition = transform.position;
            if (curPath == path1)
            {
                if (path2)
                {
                    
                    distance = path2.path.GetClosestTimeOnPath(transform.position);
                    curPath = path2;
                    targetRailPosition = curPath.path.GetPointAtTime(distance, end);
                }
                else
                    isRail = false;
            }
            else if (curPath == path2)
            {
                if (path1)
                {
                    distance = path1.path.GetClosestTimeOnPath(transform.position);
                    curPath = path1;
                    targetRailPosition = curPath.path.GetPointAtTime(distance, end);
                }
                else
                    isRail = false;
            }
            time = 0;
            midPosition = Vector3.Lerp(startPosition, targetRailPosition, 0.5f) + new Vector3(0, 5f, 0);
            isRailJump = true;
        }
        if (curPath.GetComponent<M_Rail>().pathDir == -1)
            distance -= railSpeed * Time.deltaTime / curPath.path.length;
        else 
            distance += railSpeed * Time.deltaTime / curPath.path.length;
        transform.position = curPath.path.GetPointAtTime(distance, end);
        transform.rotation = curPath.path.GetRotation(distance, end);
    }

    void RailJump()
    {
        transform.position = Bezier(startPosition, midPosition, targetRailPosition, 2 * time);
        if (transform.position == targetRailPosition)
            isRailJump = false;
    }

    public Vector3 Bezier(Vector3 p1, Vector3 p2, Vector3 p3, float value)
    {
        Vector3 A = Vector3.Lerp(p1, p2, value);
        Vector3 B = Vector3.Lerp(p2, p3, value);
        Vector3 C = Vector3.Lerp(A, B, value);

        return C;
    }

    void EnemyCheck()
    {
        //모든 적 체크
        allEnemies = GetComponent<D_BeesManager>().enemys;
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i] == null)
            {
                if (i == 0)
                    break;
                else
                    i++;
            }
            Vector3 enemyPos = playerCamera.WorldToViewportPoint(allEnemies[i].transform.position);
            //자동 에임 안에 들어온 모든 적 체크, 카메라와 제일 가까운 적 체크 
            if (enemyPos.x > 0.3f && enemyPos.x < 0.7f && enemyPos.y > 0.3f && enemyPos.y < 0.7f)
            {
                if (!nearEnemy)
                    nearEnemy = allEnemies[i];
                else if (Vector3.Distance(transform.position, allEnemies[i].transform.position) <=
                    Vector3.Distance(transform.position, nearEnemy.transform.position))
                {
                    nearEnemy = allEnemies[i];
                }

            }
        }
        if (nearEnemy)
        {
            Vector3 nearEnemyPos = playerCamera.WorldToViewportPoint(nearEnemy.transform.position);
            if (nearEnemyPos.x <= 0.3f || nearEnemyPos.x >= 0.7f || nearEnemyPos.y <= 0.3f || nearEnemyPos.y >= 0.7f)
            {
                nearEnemy = null;
                aimImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }
        }
    }

    void Attack()
    {
        audioSource.Play();
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width * cameraCenter, Screen.height * 0.5f, 0f));
        RaycastHit hit;
        if (nearEnemy)
        {
            targetPosition = nearEnemy.transform.position;
            Vector3 enemyPos = playerCamera.WorldToViewportPoint(nearEnemy.transform.position);
            Vector3 aimPos = new Vector2(500 - (1 - enemyPos.x) * 1000, 500 - (1 - enemyPos.y) * 1000);
            aimImage.GetComponent<RectTransform>().anchoredPosition = aimPos;
            
        }
        else if (Physics.Raycast(ray, out hit))
        {
            print(hit.transform.name);
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = ray.GetPoint(50);
        }
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = firePosition.transform.position;
        bullet.GetComponent<M_HoneyBullet>().SetTarget(targetPosition);
    }

    IEnumerator IeAttack()
    {
        Attack();
        yield return new WaitForSeconds(0.2f);
        //Attack();
    }

    public void Hit()
    {
        hp -= 5;
    }

    IEnumerator IeHit()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.3f);
            hp--;
        }
    }

    public void Die()
    {
        isDie = true;
        M_GameManager.instance.Player2Die();
        freeLookCam.enabled = false;
        gameObject.SetActive(false);
    }
}
