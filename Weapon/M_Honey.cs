using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Honey : MonoBehaviour
{
    public int count = 0; //�� ����, �� �Ѿ˿��� ����
    public GameObject explosionParticle; //���� ��ƼŬ, �巡�׷� �������� 
    bool isExplosion = false; //���� ����?
    bool isAttack = false; //���� ����?
    Vector3 curSize; //���� ������

    void Start()
    {
        curSize = transform.localScale;
    }

    void Update()
    {
        if (transform.parent)
        {
            if (transform.parent.name == "Hip (1)")
                Destroy(gameObject);
            if (transform.parent.name == "Hip")
                GetComponent<SphereCollider>().radius = 2f;
        }
        
        if (transform.localScale.x > 5)
        {
            transform.localScale = new Vector3(5, 5, 5);
        }
        if (count > 10)
            count = 10;
        if (isExplosion && isAttack && transform.parent)
        {
            //���� ���� ����
            BossAttack();
        }
    }

    void BossAttack()
    {
        print(transform.root.name);
        if (transform.root.GetComponent<D_Pattern>())
            transform.root.GetComponent<D_Pattern>().Damage();
        if (transform.parent.name == "Chest")
        {
            D_Boss.instance.chestHP -= count;
        }
        else if (transform.parent.name == "Back")
        {
            D_Boss.instance.backHP -= count;
        }
        else if (transform.parent.name == "Belly")
        {
            D_Boss.instance.bellyHP -= count;
        }
        else if (transform.parent.name == "Hip")
        {
            D_Boss.instance.hipHP -= count;
        }
        else if (transform.parent.name == "Chain")
        {
            D_Boss.instance.chainHP -= count;
        }
        else
            isAttack = false;
    }

    //����
    //�ݶ��̴� ũ�Ⱑ 1 Ŀ���� 
    public void Explosion()
    {
        isExplosion = true;
        isAttack = true;
        GetComponent<SphereCollider>().radius += 1f;
        
        //���� �θ� ���Կ� ���� �޶����� ������Ʈ��� ���� ���Ը� �����ش�.
        if (transform.parent && transform.parent.GetComponent<M_Counter>())
            transform.parent.GetComponent<M_Counter>().count -= count;
        else if (transform.GetComponent<M_Counter>())
            transform.GetComponent<M_Counter>().count -= count;
    }

    //10�� ������ ũ�� Ű��
    public void SizeUp()
    {
        count++;
        if (transform.parent && transform.parent.GetComponent<M_Counter>())
            transform.parent.GetComponent<M_Counter>().count++;
        if (count < 10)
        {
            if (transform.name.Contains("Chain"))
                return;
            transform.localScale = curSize * 1.1f;
            curSize = transform.localScale;
        }
        StartCoroutine(IeSizeUp());
    }

    //�� ũ�Ⱑ Ŀ�� �� �ʱ۰Ÿ��� ���� ���� �ڷ�ƾ
    IEnumerator IeSizeUp()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                yield return new WaitForSeconds(0.01f);
                transform.localScale = curSize * (1f + 0.01f * j);
            }
            yield return new WaitForSeconds(0.05f);
            transform.localScale = curSize * 0.9f;
        }
        transform.localScale = curSize;
    }

    //������ ������ �� �ٸ� ���� �ݶ��̴� �ȿ� ������ �� �ܵ� ���� �����ϰ� ��
    //���� ������ 0.1�� �� �����
    private void OnCollisionEnter(Collision collision)
    {
        if (isExplosion)
        {
            if (collision.gameObject.GetComponent<M_Honey>())
            {
                collision.gameObject.GetComponent<M_Honey>().Explosion();
            }
            //���� ����Ʈ
            GameObject explosion = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            explosion.transform.localScale *= 4 + count / 2f;
            explosion.transform.GetChild(0).localScale *= 4 + count / 2f;
            Destroy(gameObject, 0.1f);
        }
    }

}
