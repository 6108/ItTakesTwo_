using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PathCreation;

public class M_Player : MonoBehaviour
{
    #region ü��
    public int hp = 10;
    public bool isDie = false; //�׾��°�?
    #endregion

    #region �̵�
    CharacterController cc; //��Ʈ�ѷ�
    public float speed = 5; //�̵��ӵ�
    public float gravity = -10; //�߷� 
    public float yVelocity; //���� ����
    public float jumpPower = 5; //���� ����
    int jumpCount = 0; //2�� ����, ��������
    #endregion

    #region ���
    public float dashTime = 0.5f; //�뽬 �ð�
    public float dashSpeed = 10; //�뽬 �ӵ�
    #endregion

    #region ī�޶� ȸ��
    public Camera playerCamera; //�÷��̾� ī�޶�
    public CinemachineFreeLook freeLookCam; //�ó׸ӽ�
    float mouseX = 0; //�¿� �Է� ����
    //float mouseY = 0;
    #endregion

    #region ��ũ����
    GameObject hook;
    public bool isHookRange = false;
    public bool isHook = false;
    public Transform hookPosition;
    public GameObject hookPrefab;
    #endregion

    #region ����
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
    float time;
    #endregion

    #region �ڵ� ����
    GameObject[] allEnemies;
    GameObject nearEnemy;
    public GameObject aimImage;
    #endregion

    #region ����
    bool isAttack = false; //���� �����Ѱ�?
    public GameObject bulletPrefab; //�Ѿ�
    public Transform firePosition; //�ѱ� ��ġ
    public Vector3 targetPosition; //Ÿ�� ��ġ
    public float attackSpeed = 5; //���� �ӵ�
    CinemachineImpulseSource impulse;
    AudioSource audioSource;
    public float cameraCenter = 0.25f;
    #endregion
    //�ִϸ��̼�
    public Animator anim;
    public static M_Player instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        impulse = GetComponent<CinemachineImpulseSource>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Honey");
        if (isDie)
            return;
        if (hp <= 0)
            Die();
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.O))
        {
            Hit();
        }
        CameraRotate();
        ZoomINOut();
        EnemyCheck();
        if (Input.GetButtonDown("JoySqure"))
            StartCoroutine(IeDash());
        if (isAttack)
            Attack();
        if (isRailJump)
            RailJump();
        else if (isRail)
            RailMove();
        else
            Move();
    }

    void Move()
    {  
        float h = Input.GetAxis("JoyHorizontal") * 10f;
        float v = Input.GetAxis("JoyVertical") * 10f;
        Vector3 dir = new Vector3(h, 0, v);
        dir = playerCamera.transform.TransformDirection(dir);
        yVelocity += gravity * Time.deltaTime;
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            jumpCount = 0;
        }
        if (Input.GetButtonDown("JoyX") && jumpCount < 2)
        {
            anim.SetTrigger("JumpUp");
            dir *= 2;
            yVelocity = jumpPower;
            jumpCount++;
        }
        dir.y = yVelocity;
        if (dir.x == 0 || dir.z == 0)
            anim.SetTrigger("Idle");
        else
            anim.SetTrigger("Run");
        cc.Move(dir * speed * Time.deltaTime);
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

        if (Input.GetButtonDown("R1"))
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

    IEnumerator IeDash()
    {
        float startTime = Time.time;
        while(Time.time < startTime + dashTime)
        {
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }
    //ī�޶� ȸ��
    void CameraRotate()
    {
        mouseX += Input.GetAxis("JoyMouseX") * 30f;
        transform.eulerAngles = new Vector3(0, mouseX, 0);
        freeLookCam.m_XAxis.Value = mouseX;
    }

    //L2�� ������ ȭ�� Ȯ��, L2���� �� ���� ȭ�� �� �ƿ�
    //L2�� ���� ���¿����� ���� ����
    void ZoomINOut()
    {
        if (Input.GetButtonDown("L2"))
        {
            StartCoroutine(IeZoomIn());
            aimImage.SetActive(true);
            isAttack = true;
        }
        if (Input.GetButtonUp("L2"))
        {
            StartCoroutine(IeZoomOut());
            aimImage.SetActive(false);
            isAttack = false;
        }
    }

    //�� ��
    //ȭ�� Ȯ���ϰ� ��¦ ��������
    IEnumerator IeZoomIn()
    {
        for (int  i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.01f);
            freeLookCam.m_Lens.FieldOfView -= 0.5f;
            freeLookCam.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenX -= 0.01f;
            freeLookCam.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenX -= 0.01f;
            freeLookCam.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenX -= 0.01f;

        }
    }

    //�� �ƿ�
    //ȭ�� ����ϰ� �ٽ� ���麸��
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

    public void AddHoney()
    {

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
        //��� �� üũ
        for (int i = 0; i < allEnemies.Length - 1; i++)
        {
            if (allEnemies[i] == null)
            {
                    i++;
            }
            Vector3 enemyPos = playerCamera.WorldToViewportPoint(allEnemies[i].transform.position);
            //�ڵ� ���� �ȿ� ���� ��� �� üũ, ī�޶�� ���� ����� �� üũ 
            if (enemyPos.x > 0.3f && enemyPos.x < 0.7f && enemyPos.y > 0.3f && enemyPos.y < 0.7f)
            {
                nearEnemy = allEnemies[i];
                if (!nearEnemy)
                    nearEnemy = allEnemies[i];
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

    //����
    void Attack()
    {
        if (Input.GetButtonDown("R2"))
        {
            impulse.GenerateImpulse(1f);
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
                targetPosition = hit.point;
            }
            else
                targetPosition = ray.GetPoint(30);
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = firePosition.transform.position;
            bullet.GetComponent<Rigidbody>().velocity = (targetPosition - firePosition.transform.position).normalized * attackSpeed;
        }     
    }

    public void Hit()
    {
        hp -= 5;
    }

    public void Die()
    {
        isDie = true;
        M_GameManager.instance.Player1Die();
        freeLookCam.enabled = false;
        gameObject.SetActive(false);
    }

}
