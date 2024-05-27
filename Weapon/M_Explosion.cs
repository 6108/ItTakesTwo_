using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Æø¹ß
public class M_Explosion : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("P1Bullet"))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;

                transform.GetChild(i).GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere);
            }
        }
    }
}
