using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//¶³¾îÁ³À» ¶§ Á×´Â ºÎºÐ
public class M_DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<M_Player>())
        {
            M_Player.instance.Hit();
            M_Player.instance.Hit();
        }
        else if (other.gameObject.GetComponent<M_Player2>())
        {
            M_Player2.instance.Hit();
            M_Player2.instance.Hit();
        }
    }
}
