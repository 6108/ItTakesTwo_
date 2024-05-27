using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_NextStage : MonoBehaviour
{
    bool isP1 = false;
    bool isP2 = false;


    // Update is called once per frame
    void Update()
    {
        if (isP1 && isP2)
            M_GameManager.instance.StartCoroutine("IeNextScene");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player1"))
        {
            isP1 = true;
        }
        if (other.name.Contains("Player2"))
            isP2 = true;
    }
}
