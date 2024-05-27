using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� �Ѿ��� ���ؿ� ������ �����ϴ� �� �Ѿ�
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
