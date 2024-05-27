using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어를 찍는 카메라
public class M_CameraRotate : MonoBehaviour
{
    float mouseX;
    float mouseY;
    public GameObject target;

    void Start()
    {
        //시작할 때 사용자가 정해준 각도값으로 세팅
        mouseX = transform.eulerAngles.y;
        mouseY = -transform.eulerAngles.x;
    }

    void Update()
    {
        transform.LookAt(target.transform.position);
    }
}