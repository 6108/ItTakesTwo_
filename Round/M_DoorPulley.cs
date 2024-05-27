using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//p1 총알 부딪히면 문 열림
public class M_DoorPulley : MonoBehaviour
{
    ConfigurableJoint joint;
    public GameObject door;
    public bool isOpen = false;
    public float doorSpeed = 1;
    float doorY;
    bool isDown = false;

    void Start()
    {
        doorY = door.transform.position.y;
        joint = GetComponent<ConfigurableJoint>();
    }

    void Update()
    {
        //문 트리거 체크되고 도르래 속도가 0이 아니면 문을 위로 올림
        if (isOpen && joint.targetAngularVelocity != new Vector3(0, 0, 0))
        {
            door.transform.position += Vector3.up * doorSpeed * Time.deltaTime;
        }
        else if (isDown)
        {
            StartCoroutine(IeDown());
            isDown = false;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //P1 총알이 닿으면 문이 안열려있을 때만 도르래 회전
        if (collision.gameObject.name.Contains("P1Bullet") && !isOpen)
        {
            Destroy(collision.gameObject);
            joint.targetAngularVelocity = new Vector3(0, 3, 0);
            isOpen = true;
            StartCoroutine(IeStopPulley());
        }
    }

    //도르래 속도줄이다가 정지
    IEnumerator IeStopPulley()
    {
        yield return new WaitForSeconds(1f);
        joint.targetAngularVelocity = new Vector3(0, 1, 0);
        yield return new WaitForSeconds(1f);
        joint.targetAngularVelocity = new Vector3(0, 0, 0);
        isOpen = false;
        isDown = true;
    }   

    IEnumerator IeDown()
    {
        while (door.transform.position.y > doorY)
        {
            yield return new WaitForSeconds(0.7f);
            door.transform.position -= Vector3.up;
        }
    }
}
