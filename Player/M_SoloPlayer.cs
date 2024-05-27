using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


//테스트용 1인 플레이어
public class M_SoloPlayer : MonoBehaviour
{
    //플레이어 체력
    public int hp = 10;
    public GameObject diePanel;
    public bool isDie = false;
    //public GameObject playerCineMachine;

    //플레이어 회전 
    public CinemachineFreeLook freeLookCam;
    public float rotSpeed = 205;
    float mouseX;

    //플레이어 이동
    public float speed = 5;
    public float gravity = -10;
    float yVelocity;

    CharacterController cc;
    //public float turnSmoothTime = 0.1f;
    public Camera playerCamera;

    //플레이어 공격
    public GameObject fireBulletPrefab;
    public GameObject honeyBulletPrefab;
    public GameObject firePosition;
    public Vector3 targetPosition;
    public float attackSpeed = 2;
    public GameObject gun;
    float time;

    //Move
    public float jumpPower = 5;
    int jumpCount = 0;

    #region Dash 속성
    public float dashTime = 0.5f;
    public float dashSpeed = 10;
    #endregion

    //후크 점프
    public bool isHookRange = false;
    public bool isHook = false;
    public Transform hookPosition;
    public GameObject hookPrefab;

    private void Start()
    {
        mouseX = transform.eulerAngles.y;
        //mouseY = -transform.eulerAngles.x;
        cc = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isDie)
            return;
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartCoroutine(IeDash());
        if (Input.GetButton("Fire1"))
        {
            if (time > 0.3f)
            {
                StartCoroutine(IeHoneyAttack());
                time = 0;
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            FireAttack();
        }
        Hook();
        if (isHook)
        {
            HookMove();
        }
        else
           Move();
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

    GameObject hook;
    public void HookON(Vector3 hookPosition)
    {
        //후크 생성
        hook = Instantiate(hookPrefab, hookPosition, Quaternion.identity);
        transform.parent.transform.parent = hook.transform;
        cc.enabled = false;
        //transform.parent.transform.parent = null;
        isHook = true;
    }

    public void HookOff()
    {

        cc.enabled = true;
        isHook = false;
        yVelocity = transform.position.y + jumpPower;
        gravity /= 2;
        hook.GetComponent<ConfigurableJoint>().breakForce = 0;
        //transform.parent.transform.parent.GetComponent<Rigidbody>().AddForce(transform.forward * 100);
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

    IEnumerator IeDash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void Move()
    {
        float mh = Input.GetAxis("Mouse X");
        mouseX += mh * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, mouseX, 0);
        freeLookCam.m_XAxis.Value = mouseX;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir = playerCamera.transform.TransformDirection(dir);
        yVelocity += gravity * Time.deltaTime;
        if (isHookRange && transform.parent.transform.parent)
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
            gravity *= 2;
            transform.parent.transform.parent = null;
            Destroy(hook);
        }
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            dir *= 2;
            yVelocity = jumpPower;
            jumpCount++;
        }
        dir.y = yVelocity;
        cc.Move(dir * speed * Time.deltaTime);
    }

    void FireAttack()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = ray.GetPoint(30);
        }
        GameObject bullet = Instantiate(fireBulletPrefab);
        bullet.transform.position = firePosition.transform.position;
        bullet.GetComponent<Rigidbody>().velocity = (targetPosition - firePosition.transform.position).normalized * attackSpeed;
    }

    void HoneyAttack()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = ray.GetPoint(30);
        }
        GameObject bullet = Instantiate(honeyBulletPrefab);
        bullet.transform.position = firePosition.transform.position;
        bullet.GetComponent<Rigidbody>().velocity = (targetPosition - firePosition.transform.position).normalized * attackSpeed;
    }

    IEnumerator IeHoneyAttack()
    {
        HoneyAttack();
        yield return new WaitForSeconds(0.1f);
        HoneyAttack();
    }
}
