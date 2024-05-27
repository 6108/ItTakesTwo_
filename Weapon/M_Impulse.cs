using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class M_Impulse : MonoBehaviour
{
    CinemachineImpulseSource impulse;

    void Start()
    {
        impulse = GetComponent<CinemachineImpulseSource>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            Shake();
    }
    void Shake()
    {
        impulse.GenerateImpulse(1f);
    }
}
