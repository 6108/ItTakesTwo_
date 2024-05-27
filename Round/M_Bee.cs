using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Bee : MonoBehaviour
{
    Vector3 dir;

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 5f ;
    }
}
