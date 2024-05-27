using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RailOff : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player1"))
            M_Player.instance.isRail = false;
        if (other.name.Contains("Player2"))
            M_Player2.instance.isRail = false;
    }
}
