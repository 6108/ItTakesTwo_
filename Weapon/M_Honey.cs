using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Honey : MonoBehaviour
{
    public int count = 0; //꿀 갯수, 꿀 총알에서 접근
    public GameObject explosionParticle; //폭발 파티클, 드래그로 가져오기 
    bool isExplosion = false; //폭발 가능?
    bool isAttack = false; //공격 가능?
    Vector3 curSize; //현재 사이즈

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
            //보스 부위 공격
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

    //폭발
    //콜라이더 크기가 1 커지고 
    public void Explosion()
    {
        isExplosion = true;
        isAttack = true;
        GetComponent<SphereCollider>().radius += 1f;
        
        //만약 부모가 무게에 따라 달라지는 오브젝트라면 꿀의 무게를 덜어준다.
        if (transform.parent && transform.parent.GetComponent<M_Counter>())
            transform.parent.GetComponent<M_Counter>().count -= count;
        else if (transform.GetComponent<M_Counter>())
            transform.GetComponent<M_Counter>().count -= count;
    }

    //10번 까지만 크기 키움
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

    //꿀 크기가 커질 때 탱글거리는 느낌 내는 코루틴
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

    //폭발이 가능할 때 다른 꿀이 콜라이더 안에 들어오면 그 꿀도 폭발 가능하게 함
    //꿀이 터지면 0.1초 뒤 사라짐
    private void OnCollisionEnter(Collision collision)
    {
        if (isExplosion)
        {
            if (collision.gameObject.GetComponent<M_Honey>())
            {
                collision.gameObject.GetComponent<M_Honey>().Explosion();
            }
            //폭발 이펙트
            GameObject explosion = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            explosion.transform.localScale *= 4 + count / 2f;
            explosion.transform.GetChild(0).localScale *= 4 + count / 2f;
            Destroy(gameObject, 0.1f);
        }
    }

}
