using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//p1 �Ѿ� �ε����� �� ����
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
        //�� Ʈ���� üũ�ǰ� ������ �ӵ��� 0�� �ƴϸ� ���� ���� �ø�
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
        //P1 �Ѿ��� ������ ���� �ȿ������� ���� ������ ȸ��
        if (collision.gameObject.name.Contains("P1Bullet") && !isOpen)
        {
            Destroy(collision.gameObject);
            joint.targetAngularVelocity = new Vector3(0, 3, 0);
            isOpen = true;
            StartCoroutine(IeStopPulley());
        }
    }

    //������ �ӵ����̴ٰ� ����
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
