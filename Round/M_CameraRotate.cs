using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ ��� ī�޶�
public class M_CameraRotate : MonoBehaviour
{
    float mouseX;
    float mouseY;
    public GameObject target;

    void Start()
    {
        //������ �� ����ڰ� ������ ���������� ����
        mouseX = transform.eulerAngles.y;
        mouseY = -transform.eulerAngles.x;
    }

    void Update()
    {
        transform.LookAt(target.transform.position);
    }
}