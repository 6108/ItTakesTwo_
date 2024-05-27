using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//²Ü ÃÑ¾ËÀÇ ÀÜÇØ¿¡ ´êÀ¸¸é Æø¹ßÇÏ´Â ºÒ ÃÑ¾Ë
public class M_FireBullet : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        Destroy(gameObject, 3);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);
        if (collision.gameObject.tag == "Honey")
        {
            collision.gameObject.GetComponent<M_Honey>().Explosion();
            audioSource.Play();
        }
        GetComponent<MeshRenderer>().enabled = false;
    }
}
