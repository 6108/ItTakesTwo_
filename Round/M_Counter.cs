using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ѿ˿� ���� Ƚ�� ī��Ʈ
public class M_Counter : MonoBehaviour
{
    public int count = 0;
    private void Update()
    {
        if (count < 0)
            count = 0;
    }
}
