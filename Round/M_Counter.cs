using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총알에 맞은 횟수 카운트
public class M_Counter : MonoBehaviour
{
    public int count = 0;
    private void Update()
    {
        if (count < 0)
            count = 0;
    }
}
